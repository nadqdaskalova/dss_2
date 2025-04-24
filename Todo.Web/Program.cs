using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Refit;
using System;
using Todo.Web.Clients.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Todo.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddLogging(opt =>
            {
                opt.ClearProviders();
                opt.SetMinimumLevel(LogLevel.Debug);
                opt.AddConsole();
            });
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
                {
                    options.AccessDeniedPath = "/Error/403";
                    options.LoginPath = "/Login/Index";
                    options.Cookie = new CookieBuilder
                    {
                        SameSite = SameSiteMode.Strict,
                        HttpOnly = true,
                        MaxAge = TimeSpan.FromDays(1),
                        IsEssential = true,
                        SecurePolicy = CookieSecurePolicy.SameAsRequest,
                        Name = "TodoWebSession"
                    };
                });

            builder.Services.AddAuthorization(e =>
            {
                e.AddPolicy("Administrator", e =>
                {
                    e.RequireRole("Administrator");
                });

                e.DefaultPolicy = new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(CookieAuthenticationDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser()
                    .Build();
            });

            builder.Services.AddRefitClient<IUserClient>()
                .ConfigureHttpClient(e =>
                {
                    var apiUrl = Environment.GetEnvironmentVariable("WEB_API_URL") ?? "https://localhost:5001/";
                    e.BaseAddress = new Uri($"{apiUrl}api/user");                
                });
                builder.Services.AddRefitClient<ITodoListClient>()
                    .ConfigureHttpClient(e =>
                    {
                        var apiUrl = Environment.GetEnvironmentVariable("WEB_API_URL") ?? "https://localhost:5001/";
                        e.BaseAddress = new Uri($"{apiUrl}api/todolist");
                    });

                builder.Services.AddRefitClient<ITodoTaskClient>()
                    .ConfigureHttpClient(e =>
                    {
                        var apiUrl = Environment.GetEnvironmentVariable("WEB_API_URL") ?? "https://localhost:5001/";
                        e.BaseAddress = new Uri($"{apiUrl}api/todotask");
                    });


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication().UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}