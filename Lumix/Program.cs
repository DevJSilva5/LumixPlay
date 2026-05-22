using Lumix.Data;
using Lumix.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddHttpClient<TmdbService>();

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromDays(30);

    options.Cookie.HttpOnly = true;

    options.Cookie.IsEssential = true;

    options.Cookie.SameSite = SameSiteMode.Lax;

    options.Cookie.SecurePolicy =
        CookieSecurePolicy.Always;

    options.Cookie.MaxAge = null;
});

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(
            builder.Configuration.GetConnectionString("DefaultConnection")
        )
    )
);

var app = builder.Build();


app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.Use(async (context, next) =>
{
    var path = context.Request.Path.Value?.ToLower();

    bool rotaLiberada =
        path == "/" ||
        path!.StartsWith("/auth/login") ||
        path.StartsWith("/auth/register") ||
        path.StartsWith("/css") ||
        path.StartsWith("/js") ||
        path.StartsWith("/images") ||
        path.StartsWith("/lib");

    var usuario =
        context.Session.GetString("Usuario");

    if (string.IsNullOrEmpty(usuario) && !rotaLiberada)
    {
        context.Response.Redirect("/Auth/Login");
        return;
    }

    await next();
});

app.UseAuthorization();

app.MapGet("/", context =>
{
    context.Response.Redirect("/Auth/Login");

    return Task.CompletedTask;
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Login}/{id?}"
);

app.Run();