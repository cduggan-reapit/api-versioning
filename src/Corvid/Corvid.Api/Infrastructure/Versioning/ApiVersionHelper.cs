using System.Diagnostics;
using System.Reflection;
using Corvid.Api.Attributes;
using Corvid.Api.Controllers;
using Corvid.Api.Helpers;
using Microsoft.AspNetCore.Mvc.Routing;

namespace Corvid.Api.Infrastructure.Versioning;

internal static class ApiVersionHelper
{
    static ApiVersionHelper()
    {
        ApiVersions = GetAllVersions();
    }
    
    public static readonly IEnumerable<DateOnly> ApiVersions;

    public static IEnumerable<DateOnly> GetSwaggerDisplayVersions()
    {
        var include = ApiVersions
            .Where(v => v == ApiVersions.Max() || v.Day == 1)
            .OrderByDescending(v => v)
            .Take(4)
            .ToList();
        
        include.AddRange(ApiVersions.Where(v => v < include.Min() && v is { Day: 1, Month: 1 }));

        return include.Distinct();
    }
    
    private static IEnumerable<DateOnly> GetAllVersions()
    {
        Console.WriteLine(">> DISCOVERING API VERSIONS");
        var sw = new Stopwatch();
        sw.Start();
        
        var reportedVersions = new List<DateOnly>();
        
        foreach (var controller in GetControllerClasses())
        {
            var controllerRange = controller.GetVersionRange();
            foreach (var action in controller.GetActionMethods())
            {
                var versionRange = action.GetVersionRange() ?? controllerRange;
                
                if(versionRange == null)
                    continue;
                
                reportedVersions.AddRange(versionRange.Removed.HasValue 
                    ? [versionRange.Introduced, versionRange.Introduced]
                    : [ versionRange.Introduced ]);
            }
        }
        
        reportedVersions = reportedVersions.Distinct().ToList();
        
        var allVersions = GetCalendar(reportedVersions.Min(), DateOnlyHelper.Today);
        
        sw.Stop();
        Console.WriteLine($"\tCOMPLETE IN {sw.ElapsedMilliseconds}ms");

        return allVersions;
    }
    
    private static VersionRange? GetVersionRange(this MemberInfo controllerType)
        => controllerType.GetCustomAttributes().GetVersionRange();

    private static IEnumerable<Type> GetControllerClasses()
        => Assembly.GetExecutingAssembly().GetTypes()
            .Where(t => t is { IsClass: true, IsAbstract: false } && t.IsSubclassOf(typeof(CorvidControllerBase)));

    private static IEnumerable<MethodInfo> GetActionMethods(this Type controllerType)
        => controllerType.GetMethods()
            .Where(method => method.IsPublic && method.GetCustomAttributes().Any(attribute => attribute is HttpMethodAttribute));
    private static VersionRange? GetVersionRange(this IEnumerable<Attribute> attributes)
    {
        var attributeList = attributes.ToList();
        
        var introducedAttribute = GetIntroductionDate(attributeList);
        if (introducedAttribute == null)
            return null;

        var removedAttribute = GetRemovalDate(attributeList);

        return new VersionRange(introducedAttribute.Version, removedAttribute?.Version);
    }

    private static IntroducedInVersionAttribute? GetIntroductionDate(this IEnumerable<Attribute> attributes)
        => attributes.SingleOrDefault(attribute => attribute is IntroducedInVersionAttribute) as IntroducedInVersionAttribute;
    
    private static RemovedInVersionAttribute? GetRemovalDate(this IEnumerable<Attribute> attributes)
        => attributes.SingleOrDefault(attribute => attribute is RemovedInVersionAttribute) as RemovedInVersionAttribute;

    private static IEnumerable<DateOnly> GetCalendar(DateOnly dateFrom, DateOnly dateTo)
    {
        while (dateFrom <= dateTo)
        {
            yield return dateFrom;
            dateFrom = dateFrom.AddDays(1);
        }
    }
}

public record VersionRange(DateOnly Introduced, DateOnly? Removed);