using Lumix.Data;
using Lumix.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddHttpClient<TmdbService>();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(
            builder.Configuration.GetConnectionString("DefaultConnection")
        )
    )
);

/* SESSION */

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromDays(30);

    options.Cookie.HttpOnly = true;

    options.Cookie.IsEssential = true;
});

var app = builder.Build();

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseSession();

/* BLOQUEIA ACESSO SEM LOGIN */

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

    var usuario = context.Session.GetString("Usuario");

    if (string.IsNullOrEmpty(usuario) && !rotaLiberada)
    {
        context.Response.Redirect("/Auth/Login");
        return;
    }

    await next();
});

app.UseAuthorization();

/* localhost -> login */

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