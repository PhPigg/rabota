public class LocationAddress
{
    private readonly List<string> _addressParts = [];

    public string Value { get; }

    public IReadOnlyList<string> AddressParts => _addressParts.AsReadOnly();

    private LocationAddress(IEnumerable<string> parts)
    {
        _addressParts = [.. parts];
        Value = string.Join(", ", parts);
    }
    public static LocationAddress Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentNullException("Адрес локации не может быть пустым.", nameof(value));
        List<string> parts =
        [
            .. value
                .Split(',')
                .Select(part => part.Trim())
                .Where(part => !string.IsNullOrWhiteSpace(part)),
        ];
        if (parts.Count == 0)
            throw new ArgumentException(
                "Адрес локации должен содержать хотя бы одну часть.",
                nameof(value)
            );
        return new LocationAddress(parts);
    }
}