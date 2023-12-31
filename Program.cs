using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor.Services;
using NaughtyChoppersDA.Globals;
using NaughtyChoppersDA.Repositories;
using NaughtyChoppersDA.Services;
using OpenAI.Net;

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

            builder.Services.AddOpenAIServices(o =>
            {
                o.ApiKey = builder.Configuration["OpenAI:ApiKey"]; //ChatGPT API key value from secrets.json
            });

            AccessToDb.ConnectionString = builder.Configuration.GetConnectionString("NaughtyChoppersDB");

            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IProfileService, ProfileService>();

            builder.Services.AddTransient<IUserRepository, UserRepository>();
            builder.Services.AddTransient<IProfileRepository, ProfileRepository>();
            builder.Services.AddTransient<IMatchingRepository, MatchingRepository>();

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