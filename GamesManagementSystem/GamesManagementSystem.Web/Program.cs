using GamesManagementSystem.Application.Interfaces;
using GamesManagementSystem.Infrastructure.Data;
using GamesManagementSystem.Infrastructure.Repositories;
using GamesManagementSystem.Infrastructure.Services;
using GamesManagementSystem.Web.Filters;
using GamesManagementSystem.Web.Middleware;
using Microsoft.EntityFrameworkCore;

namespace GamesManagementSystem.Web
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            // 1. Register the DbContext
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(connectionString));

            // 2. Register our repository for DI
            builder.Services.AddScoped<IGameRepository, GameRepository>();
            builder.Services.AddScoped<IFileService, FileService>();

            builder.Services.AddMemoryCache();

            builder.Services.AddControllersWithViews(options => options.Filters.Add<CustomExceptionFilter>());

            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseMiddleware<RequestLoggingMiddleware>();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
