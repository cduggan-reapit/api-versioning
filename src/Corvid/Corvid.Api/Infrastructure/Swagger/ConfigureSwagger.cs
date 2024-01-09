using Corvid.Api.Infrastructure.Versioning;

namespace Corvid.Api.Infrastructure.Swagger;

public static class ConfigureSwagger
{
    public static IServiceCollection ConfigureSwaggerServices(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        
        services.AddSwaggerGen(c =>
        {
            // Hide the api-version parameter from swagger doc
            c.OperationFilter<ApiVersionFilter>();
        });
        
        services.ConfigureOptions<SwaggerGenOptionsConfiguration>();
        
        return services;
    }

    public static WebApplication UseConfiguredSwagger(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            var displayVersions = ApiVersionHelper.GetSwaggerDisplayVersions();
            var swaggerVersions = app.DescribeApiVersions()
                .Join(displayVersions, 
                    version => version.ApiVersion.GroupVersion, 
                    display => display,
                    (version, _) => version)
                .OrderByDescending(v => v.ApiVersion.GroupVersion)
                .ToList();

            foreach (var description in swaggerVersions)
            {
                var url = $"/swagger/{description.GroupName}/swagger.json";
                options.SwaggerEndpoint(url, description.GroupName);
            }
        });

        return app;
    }
}