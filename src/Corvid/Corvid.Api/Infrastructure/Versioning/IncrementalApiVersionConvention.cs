using Asp.Versioning;
using Asp.Versioning.Conventions;
using Corvid.Api.Attributes;
using Corvid.Api.Helpers;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace Corvid.Api.Infrastructure.Versioning;

internal class IncrementalApiVersionConvention : IControllerConvention
{
    public bool Apply(IControllerConventionBuilder builder, ControllerModel controller)
    {
        var controllerIntroduced = controller.Attributes.OfType<Attribute>().GetIntroductionDate();
        var controllerRemoved = controller.Attributes.OfType<Attribute>().GetRemovalDate();
        
        foreach (var action in controller.Actions)
        {
            // Cascade - action configuration, controller configuration
            // As this attribute is required for versioning, if it is null, we don't add any versions
            var actionIntroduced = action.Attributes.OfType<Attribute>().GetIntroductionDate() 
                                ?? controllerIntroduced;
            
            // Cascade - action configuration, controller configuration, a far-off date in the future
            // Since this isn't used as a logic gate, if it's not defined we assume it is perennially live
            var actionRemoved = action.Attributes.OfType<Attribute>().GetRemovalDate() 
                                ?? controllerRemoved;

            if (actionIntroduced == null)
                continue;
            
            var actionVersions = ApiVersionHelper.ApiVersions
                .Where(date => date >= actionIntroduced.Version && date < (actionRemoved?.Version ?? DateOnly.MaxValue))
                .Select(date => new ApiVersion(date));

            if (actionRemoved == null)
            {
                builder.Action(action.ActionMethod).HasApiVersions(actionVersions);
            }
            else
            {
                builder.Action(action.ActionMethod).HasDeprecatedApiVersions(actionVersions);
            }
        }

        return true;
    }
}