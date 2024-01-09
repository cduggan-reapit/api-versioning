using Asp.Versioning;
using Corvid.Api.Attributes;

namespace Corvid.Api.Infrastructure.Middleware;

public class ApiVersionMiddleware
{
    private readonly RequestDelegate _next;

    private const string DateOnlyFormat = "O";
    
    public ApiVersionMiddleware(RequestDelegate next)
        => _next = next;

    public async Task Invoke(HttpContext context)
    {
        SetApiVersionHeader(context);
        
        var controllerIntroducedInVersion = context.GetEndpoint()?.Metadata.GetMetadata<IntroducedInVersionAttribute>();
        if(controllerIntroducedInVersion != null)
            context.Response.Headers.Add("api-version-from", controllerIntroducedInVersion.Version.ToString(DateOnlyFormat));
        
        var controllerRemovedInVersion = context.GetEndpoint()?.Metadata.GetMetadata<RemovedInVersionAttribute>();
        if(controllerRemovedInVersion != null)
            context.Response.Headers.Add("api-version-to", controllerRemovedInVersion.Version.ToString(DateOnlyFormat));
        
        await _next(context);
    }

    private static void SetApiVersionHeader(HttpContext context)
    {
        var feature = context.Features.FirstOrDefault(f => f.Key == typeof(IApiVersioningFeature));
        
        if (feature.Key == null) 
            return;

        if (feature.Value is not IApiVersioningFeature featureValue) 
            return;
        
        // this is misleadingly named, and actually returns the matched api version (if it has been able to match it)
        var apiVersion = featureValue.RequestedApiVersion;
                
        if (apiVersion != null && apiVersion.GroupVersion.HasValue)
            context.Response.Headers.Add("api-version", apiVersion.GroupVersion.Value.ToString(DateOnlyFormat));
    }

}