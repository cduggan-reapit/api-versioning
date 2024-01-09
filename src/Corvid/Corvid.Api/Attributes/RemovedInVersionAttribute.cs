namespace Corvid.Api.Attributes;

public class RemovedInVersionAttribute : Attribute
{
    public RemovedInVersionAttribute(int year, int month, int day)
    {
        Version = new DateOnly(year, month, day);
    }

    public RemovedInVersionAttribute(string date)
    {
        Version = DateOnly.Parse(date);
    }
    
    public DateOnly Version { get; }
}