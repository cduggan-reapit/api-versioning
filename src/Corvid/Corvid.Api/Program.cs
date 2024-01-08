using System.Reflection;
using Asp.Versioning;
using Corvid.Api.Infrastructure.Swagger;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Corvid.Api;

public static class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services
            .AddApiVersioning(c =>
            {
                c.ReportApiVersions = true;
                c.ApiVersionReader = new HeaderApiVersionReader("api-version");
            })
            .AddApiExplorer(options =>
            {
                options.GroupNameFormat = "G";
            });
        
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options => 
            options.OperationFilter<ApiVersionOperationFilter>());
        
        builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerGenOptions>();

        builder.Services.AddControllers();
        
        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                foreach (var description in app.DescribeApiVersions())
                {
                    var url = $"/swagger/{description.GroupName}/swagger.json";
                    options.SwaggerEndpoint(url, description.GroupName);
                }
            });
        }

        app.UseHttpsRedirection();
        
        app.MapControllers();

        app.Run();
    }
}