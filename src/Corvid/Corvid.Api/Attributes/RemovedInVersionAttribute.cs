namespace Corvid.Api.Attributes;

public class RemovedInVersionAttribute(int year, int month) : Attribute
{
    public DateOnly Version { get; } = new(year, month, 1);
}