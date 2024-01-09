using Corvid.Api.Infrastructure.Versioning;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Corvid.Api.Infrastructure.Swagger;

public class ApiVersionFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var parameterToRemove = operation.Parameters
            .SingleOrDefault(p => p.Name == ConfigureVersioning.ApiVersionHeaderKey);

        if (parameterToRemove != null)
            operation.Parameters.Remove(parameterToRemove);
    }
}