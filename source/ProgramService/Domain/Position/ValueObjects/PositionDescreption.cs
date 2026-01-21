using Domain.LocationContext.ValueObjects;

public class PositionDescreption
{
    public string Value { get; }

    private PositionDescreption(string value)
    {
        Value = value;
    }
    public static PositionDescreption Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentNullException("Значение было пустым.", nameof(value));

        if (value.Length > 500)
            throw new ArgumentException("Строка привышает длину символов.", nameof(value));

        return new PositionDescreption(value);
    }
}