namespace Application.Position;

public sealed record RenamePositionCommand(
    Guid Id,
    string NewName);
