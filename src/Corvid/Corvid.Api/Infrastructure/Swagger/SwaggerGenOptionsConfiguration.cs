using Asp.Versioning.ApiExplorer;
using Corvid.Api.Helpers;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Corvid.Api.Infrastructure.Swagger;

internal class SwaggerGenOptionsConfiguration : IConfigureNamedOptions<SwaggerGenOptions>
{
    private readonly IApiVersionDescriptionProvider _provider;

    public SwaggerGenOptionsConfiguration(IApiVersionDescriptionProvider provider)
    {
        _provider = provider;
    }
    
    public void Configure(string? name, SwaggerGenOptions options)
        => Configure(options);
    
    public void Configure(SwaggerGenOptions options)
    {
        foreach (var description in _provider.ApiVersionDescriptions)
        {
            options.SwaggerDoc(description.GroupName, CreateVersionInfo(description));
        }
    }
    
    private OpenApiInfo CreateVersionInfo(ApiVersionDescription description)
    {
        var info = new OpenApiInfo()
        {
            Title = "Corvid API",
            Version = description.ApiVersion.ToString()
        };

        if (description.ApiVersion.GroupVersion == DateOnlyHelper.Today || description.ApiVersion.GroupVersion?.Day != 1)
        {
            info.Title += " (live)";
        }

        if (description.IsDeprecated)
        {
            info.Description += " This API version has been deprecated.";
        }

        return info;
    }
}