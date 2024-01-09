using Corvid.Api.Infrastructure.Middleware;
using Corvid.Api.Infrastructure.Swagger;
using Corvid.Api.Infrastructure.Versioning;

namespace Corvid.Api;

public static class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.ConfigureVersioningServices()
            .ConfigureSwaggerServices();
        
        builder.Services.AddControllers();
        
        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseConfiguredSwagger();
        }
        
        app.UseHttpsRedirection();

        app.UseRouting();
        app.UseMiddleware<ApiVersionMiddleware>();
        app.UseEndpoints(endpoints => endpoints.MapControllers());
        
        
        
        app.Run();
    }
}