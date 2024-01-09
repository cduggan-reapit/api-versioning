namespace Corvid.Api.Attributes;

public class IntroducedInVersionAttribute: Attribute
{
    public IntroducedInVersionAttribute(int year, int month)
    {
        Version = new(year, month, 1);
    }

    public IntroducedInVersionAttribute(string date)
    {
        Version = DateOnly.Parse(date);
    }
    
    public DateOnly Version { get; }
}