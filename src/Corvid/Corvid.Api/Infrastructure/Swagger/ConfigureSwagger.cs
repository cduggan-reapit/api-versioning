using System.Reflection;
using Corvid.Api.Helpers;

namespace Corvid.Api.Infrastructure.Swagger;

internal static class ConfigureSwagger
{
    public static IServiceCollection ConfigureSwaggerServices(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.OperationFilter<ApiVersionFilter>();
            c.OperationFilter<DeprecatedEndpointFilter>();
            
            var filePath = Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml");
            c.IncludeXmlComments(filePath);
        });
        services.ConfigureOptions<SwaggerGenOptionsConfiguration>();
        
        return services;
    }

    public static WebApplication UseConfiguredSwagger(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            // Limit swagger to latest daily, past three monthlies, and all yearlies
            var displayVersions = new[] { ApiVersionHelper.GetLatestDailyVersion() }
                        .Concat(ApiVersionHelper.GetMonthlyVersions(3))
                        .Concat(ApiVersionHelper.GetYearlyVersions());
            
            var swaggerVersions = app.DescribeApiVersions()
                .Join(displayVersions, description => description.ApiVersion.GroupVersion, versionDate => versionDate, (version, _) => version)
                .OrderByDescending(v => v.ApiVersion.GroupVersion);

            foreach (var description in swaggerVersions)
            {
                var url = $"/swagger/{description.GroupName}/swagger.json";
                options.SwaggerEndpoint(url, description.GroupName);
            }
        });

        return app;
    }
}