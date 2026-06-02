namespace Application.Location;

public sealed record RegisterLocationCommand(
    IReadOnlyList<string> LocationNames,
    string Address,
    string IanaTimeZone);
