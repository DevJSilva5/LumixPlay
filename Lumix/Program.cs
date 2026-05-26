using Lumix.Data;
using Lumix.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;


var builder = WebApplication.CreateBuilder(args);

// =======================================================
// SERVICES
// =======================================================

builder.Services.AddControllersWithViews();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Lumix API",
        Version = "v1",
        Description = "API do sistema Lumix"
    });
});

builder.Services.AddHttpClient<TmdbService>();

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromDays(30);

    options.Cookie.HttpOnly = true;

    options.Cookie.IsEssential = true;

    options.Cookie.SameSite = SameSiteMode.Lax;

    options.Cookie.SecurePolicy =
        CookieSecurePolicy.SameAsRequest;
});

var connectionString =
    builder.Configuration.GetConnectionString(
        "DefaultConnection"
    );

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
        connectionString,
        ServerVersion.AutoDetect(connectionString)
    )
);

builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.Limits.MaxRequestBodySize =
        52428800;
});

builder.Services.Configure<IISServerOptions>(options =>
{
    options.MaxRequestBodySize =
        52428800;
});

var app = builder.Build();

// =======================================================
// PIPELINE
// =======================================================

app.UseStaticFiles();

app.UseRouting();

app.UseSession();

/* SWAGGER */

app.UseSwagger();

app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint(
        "/swagger/v1/swagger.json",
        "Lumix API V1"
    );

    c.RoutePrefix = "swagger";
});

/* LOGIN CHECK */

app.Use(async (context, next) =>
{
    var path =
        context.Request.Path.Value?.ToLower();

    bool rotaLiberada =
        path == "/" ||
        path!.StartsWith("/auth/login") ||
        path.StartsWith("/auth/register") ||
        path.StartsWith("/swagger") ||
        path.StartsWith("/css") ||
        path.StartsWith("/js") ||
        path.StartsWith("/images") ||
        path.StartsWith("/uploads") ||
        path.StartsWith("/lib");

    var usuario =
        context.Session.GetString("Usuario");

    if (
        string.IsNullOrEmpty(usuario)
        && !rotaLiberada
    )
    {
        context.Response.Redirect(
            "/Auth/Login"
        );

        return;
    }

    await next();
});

app.UseAuthorization();

app.MapGet("/", context =>
{
    context.Response.Redirect(
        "/Auth/Login"
    );

    return Task.CompletedTask;
});

app.MapControllerRoute(
    name: "default",
    pattern:
    "{controller=Auth}/{action=Login}/{id?}"
);

app.Run();