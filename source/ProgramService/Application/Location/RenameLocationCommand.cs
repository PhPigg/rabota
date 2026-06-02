namespace Application.Location;

public sealed record RenameLocationCommand(
    Guid Id,
    string NewName);
