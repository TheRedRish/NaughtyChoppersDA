using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor.Services;
using NaughtyChoppersDA.Globals;
using NaughtyChoppersDA.Repositories;
using NaughtyChoppersDA.Services;

namespace NaughtyChoppersDA
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorPages();
            builder.Services.AddMudServices();
            builder.Services.AddServerSideBlazor();

            AccessToDb.ConnectionString = builder.Configuration.GetConnectionString("NaughtyChoppersDB");

            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IProfileService, ProfileService>();

            builder.Services.AddTransient<IUserRepository, UserRepository>();
            builder.Services.AddTransient<IProfileRepository, ProfileRepository>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.MapBlazorHub();
            app.MapFallbackToPage("/_Host");

            app.Run();
        }
    }
}