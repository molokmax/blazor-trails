using BlazingTrails.Api.Persistence;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;

namespace BlazingTrails.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var connectionString = builder.Configuration.GetConnectionString("BlazingTrailsContext");
            if (!string.IsNullOrEmpty(connectionString))
            {
                builder.Services.AddDbContext<BlazingTrailsContext>(options =>
                    options.UseSqlite(connectionString));
            }

            builder.Services
                .AddControllers();
            builder.Services
                .AddFluentValidationAutoValidation()
                .AddValidatorsFromAssembly(Assembly.Load("BlazingTrails.Shared"));

            builder.Services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.Authority = builder.Configuration["Auth0:Authority"];
                    options.Audience = builder.Configuration["Auth0:ApiIdentifier"];
                });

            var app = builder.Build();


            if (app.Environment.IsDevelopment())
                app.UseWebAssemblyDebugging();

            app.UseHttpsRedirection();

            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"Images")),
                RequestPath = new PathString("/Images")
            });


            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();
            app.MapFallbackToFile("index.html");

            app.Run();
        }
    }
}