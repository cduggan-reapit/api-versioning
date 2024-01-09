using Corvid.Api.Infrastructure.Versioning;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Corvid.Api.Infrastructure.Swagger;

/// <summary>
/// Custom operation filter to exclude "api-version" property from swagger definition
/// </summary>
internal class ApiVersionFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var parameterToRemove = operation.Parameters
            .SingleOrDefault(p => p.Name == ConfigureVersioning.ApiVersionHeaderKey);

        if (parameterToRemove != null)
            operation.Parameters.Remove(parameterToRemove);
    }
}