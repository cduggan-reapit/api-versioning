using Asp.Versioning;
using Asp.Versioning.Conventions;
using Corvid.Api.Attributes;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace Corvid.Api.Infrastructure.Versioning;

public class CustomApiVersionConvention : IControllerConvention
{
    public bool Apply(IControllerConventionBuilder builder, ControllerModel controller)
    {
        Console.WriteLine(new string('*', 50));
        Console.WriteLine($"Registering ApiVersions for Controller: {controller.ControllerName}");
        
        var controllerIntroduced = controller.Attributes
            .SingleOrDefault(a => a.GetType() == typeof(IntroducedInVersionAttribute)) as IntroducedInVersionAttribute;

        var controllerRemoved = controller.Attributes
            .SingleOrDefault(a => a.GetType() == typeof(RemovedInVersionAttribute)) as RemovedInVersionAttribute;
        
        Console.WriteLine($" - Controller Introduced: {controllerIntroduced?.Version}");
        Console.WriteLine($" - Removed: {controllerRemoved?.Version}");

        foreach (var action in controller.Actions)
        {
            Console.WriteLine($"\tAction: {action.ActionName}");
            
            var actionIntroduced = action.Attributes
                .SingleOrDefault(a => a.GetType() == typeof(IntroducedInVersionAttribute)) as IntroducedInVersionAttribute
                ?? controllerIntroduced;
            
            var actionRemoved = action.Attributes
                .SingleOrDefault(a => a.GetType() == typeof(RemovedInVersionAttribute)) as RemovedInVersionAttribute
                ?? controllerRemoved;

            if (actionIntroduced == null)
                continue;
            
            var actionVersions = ApiVersionHelper.ApiVersions
                .Where(date => date >= actionIntroduced!.Version
                        && (actionRemoved == null || date < actionRemoved.Version))
                .Select(date => new ApiVersion(date))
                .ToList();
            
            Console.WriteLine($"\t - Introduced: {actionIntroduced?.Version}");
            Console.WriteLine($"\t - Removed: {actionRemoved?.Version}");
            Console.WriteLine($"\t - Versions: {actionVersions.Count()}");
            
            builder.Action(action.ActionMethod).HasApiVersions(actionVersions);
        }

        return true;
    }
}