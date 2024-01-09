using System.Reflection;
using Corvid.Api.Attributes;
using Corvid.Api.Controllers;
using Microsoft.AspNetCore.Mvc.Routing;

namespace Corvid.Api.Helpers;

internal static class ApiVersionHelper
{
    /// <summary>
    /// Initialize a static instance of the <see cref="ApiVersionHelper"/> class
    /// </summary>
    static ApiVersionHelper()
    {
        ApiVersions = GetAllVersions();
    }
    
    /// <summary>
    /// Collection of api versions available for the current executing assembly
    /// </summary>
    public static readonly IEnumerable<DateOnly> ApiVersions;

    /// <summary>
    /// Get the most recent daily of the api 
    /// </summary>
    public static DateOnly GetLatestDailyVersion() 
        => ApiVersions.Max();

    /// <summary>
    /// Get a collection of the most recent monthly versions of the api
    /// </summary>
    /// <param name="number">The number of versions to return</param>
    /// <returns></returns>
    public static IEnumerable<DateOnly> GetMonthlyVersions(int number)
        => ApiVersions.Except([GetLatestDailyVersion()])
            .Where(date => date.Day == 1)
            .OrderByDescending(date => date)
            .Take(number);

    /// <summary>
    /// Get a collection of all yearly versions of the api
    /// </summary>
    /// <returns></returns>
    public static IEnumerable<DateOnly> GetYearlyVersions()
        => ApiVersions.Where(date => date is { Day: 1, Month: 1 });
    
    /// <summary>
    /// Find a <see cref="IntroducedInVersionAttribute"/> in a collection of attributes. Returns null if attribute not found.
    /// </summary>
    /// <param name="attributes"></param>
    /// <returns></returns>
    public static IntroducedInVersionAttribute? GetIntroductionDate(this IEnumerable<Attribute> attributes)
        => attributes.OfType<IntroducedInVersionAttribute>().SingleOrDefault();
    
    
    /// <summary>
    /// Find a <see cref="RemovedInVersionAttribute"/> in a collection of attributes. Returns null if attribute not found.
    /// </summary>
    /// <param name="attributes"></param>
    /// <returns></returns>
    public static RemovedInVersionAttribute? GetRemovalDate(this IEnumerable<Attribute> attributes)
        => attributes.OfType<RemovedInVersionAttribute>().SingleOrDefault();
    
    #region Private Methods
    
    /// <summary>
    /// Get all versions reported in the executing assembly
    /// </summary>
    /// <returns></returns>
    private static IEnumerable<DateOnly> GetAllVersions()
    {
        var reportedVersions = GetControllerClasses()
            .SelectMany(c => c.GetControllerVersions());
        
        var allVersions = GetCalendar(reportedVersions.Min(), DateOnlyHelper.Today);

        return allVersions;
    }

    /// <summary>
    /// Get all reported versions for a given controller type
    /// </summary>
    /// <param name="controller">The type of the controller to interrogate</param>
    /// <returns></returns>
    private static IEnumerable<DateOnly> GetControllerVersions(this Type controller)
    {
        var controllerIntroduced = controller.GetIntroductionDate();
        var controllerRemoved = controller.GetRemovalDate();
            
        foreach (var action in controller.GetActionMethods())
        {
            var actionIntroduced = action.GetType().GetIntroductionDate() ?? controllerIntroduced;
            var actionRemoved = action.GetType().GetRemovalDate() ?? controllerRemoved;
                
            if(actionIntroduced != null)
                yield return actionIntroduced.Version;
                
            if(actionRemoved != null)
                yield return actionRemoved.Version;
        }
    }
    
    /// <summary>
    /// Get all non-abstract methods in the application which implement the ControllerBase class
    /// </summary>
    /// <returns></returns>
    private static IEnumerable<Type> GetControllerClasses()
        => Assembly.GetExecutingAssembly().GetTypes()
            .Where(t => t is { IsClass: true, IsAbstract: false } && t.IsSubclassOf(typeof(ApiControllerBase)));

    /// <summary>
    /// Get all methods within type which implement an <see cref="HttpMethodAttribute"/>
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    private static IEnumerable<MethodInfo> GetActionMethods(this Type type)
        => type.GetMethods()
            .Where(method => method.IsPublic && method.GetCustomAttributes().Any(attribute => attribute is HttpMethodAttribute));

    /// <summary>
    /// Get the <see cref="IntroducedInVersionAttribute"/> value for a type. Returns null if attribute not found.
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    private static IntroducedInVersionAttribute? GetIntroductionDate(this MemberInfo type)
        => type.GetCustomAttributes().GetIntroductionDate();
    
    /// <summary>
    /// Get the <see cref="RemovedInVersionAttribute"/> value for a type.  Returns null if attribute not found.
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    private static RemovedInVersionAttribute? GetRemovalDate(this MemberInfo type)
        => type.GetCustomAttributes().GetRemovalDate();
    
    /// <summary>
    /// Get a collection of all dates in a given range (inclusive)
    /// </summary>
    /// <param name="dateFrom">The start date of the collection</param>
    /// <param name="dateTo">The end date of the collection</param>
    /// <returns></returns>
    private static IEnumerable<DateOnly> GetCalendar(DateOnly dateFrom, DateOnly dateTo)
    {
        while (dateFrom <= dateTo)
        {
            yield return dateFrom;
            dateFrom = dateFrom.AddDays(1);
        }
    }
    
    #endregion
}