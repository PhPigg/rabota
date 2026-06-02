namespace Application.Location;

public sealed record UpdateLocationAddressCommand(
    Guid Id,
    string Address);
