using Corvid.Api.Attributes;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Corvid.Api.Infrastructure.Swagger;

/// <summary>
/// Custom operation filter to correctly designate deprecated api endpoints 
/// </summary>
internal class DeprecatedEndpointFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        // Try to get the method that represents the endpoint
        if (!context.ApiDescription.TryGetMethodInfo(out var methodInfo))
            return;

        // Get any [RemovedInVersion] attributes from the action
        var actionRemovedAttribute = methodInfo.GetCustomAttributes(inherit: true)
            .OfType<RemovedInVersionAttribute>()
            .SingleOrDefault();
        
        // Get any [RemovedInVersion] attributes from the controller in which the action is declared
        var controllerRemovedAttribute = context.MethodInfo.DeclaringType?
            .GetCustomAttributes(true)
            .OfType<RemovedInVersionAttribute>()
            .SingleOrDefault();
     
        // If neither the action nor it's declaring controller have a [RemovedInVersion] attribute, take no action
        if (actionRemovedAttribute == null && controllerRemovedAttribute == null)
            return;

        // Otherwise mark the endpoint as deprecated
        operation.Deprecated = true;
    }
}