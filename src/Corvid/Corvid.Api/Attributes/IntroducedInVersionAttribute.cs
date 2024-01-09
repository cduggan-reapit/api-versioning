namespace Corvid.Api.Attributes;

public class IntroducedInVersionAttribute: Attribute
{
    public IntroducedInVersionAttribute(int year, int month, int day)
    {
        Version = new DateOnly(year, month, day);
    }

    public IntroducedInVersionAttribute(string date)
    {
        Version = DateOnly.Parse(date);
    }
    
    public DateOnly Version { get; }
}