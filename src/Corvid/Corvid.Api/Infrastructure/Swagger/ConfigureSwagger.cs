using Corvid.Api.Helpers;

namespace Corvid.Api.Infrastructure.Swagger;

internal static class ConfigureSwagger
{
    private static Dictionary<string, int> HttpMethodDisplayOrder =>
        new Dictionary<string, int>()
        {
            { "HttpConnect", 0 },
            { "HttpTrace", 1 },
            { "HttpGet", 2 },
            { "HttpHead", 3 },
            { "HttpOptions", 4 },
            { "HttpPost", 5 },
            { "HttpPatch", 6 },
            { "HttpPut", 7 },
            { "HttpDelete", 8 },
            { "HttpMethod", 9 }
        };
    
    public static IServiceCollection ConfigureSwaggerServices(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
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