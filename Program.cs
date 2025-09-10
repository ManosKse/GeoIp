using GeoIpProject.Api.Services;
using GeoIpProject.Clients;
using GeoIpProject.Services;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics.CodeAnalysis;

namespace GeoIpProject
{

    [ExcludeFromCodeCoverage]
    public class Program
    {
        protected Program() { }

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddLogging();

            builder.Services.AddProblemDetails(ConfigureProblemDetails);

            builder.Services.AddDbContext(builder.Configuration);

            // Add FreeGeoIpClient and Hosted service
            builder.Services.AddFreeGeoIpClient(builder.Configuration);
            builder.Services.AddHostedService(builder.Configuration);

            // Add GeoIpProjectServices
            builder.Services.AddGeoIpProjectServices();

            // Add GeoIpApiServices
            builder.Services.AddGeoIpApiServices();

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                db.Database.Migrate();
            }

            app.UseProblemDetails()
               .UseHsts()
               .UseHttpsRedirection()
               .UseRouting()
               .UseAuthorization()
               .UseEndpoints(endpoints => endpoints.MapControllers())
               .UseSwagger()
               .UseSwaggerUI();

            app.Run();
        }

        public static void ConfigureProblemDetails(Hellang.Middleware.ProblemDetails.ProblemDetailsOptions options)
        {
            options.Map<Exception>(exception => new Microsoft.AspNetCore.Mvc.ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Detail = exception.Message
            });
        }
    }
}