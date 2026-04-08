using Domain.PositionsContext;
using Domain.PositionsContext.ValueObjects;
using Domain.Shared;

namespace MyProject.Tests.PositionTests;

/// <summary>
/// Тесты для сущности Position
/// </summary>
public class PositionTests
{
    #region Вспомогательные классы

    /// <summary>
    /// Fake-реализация критерия уникальности для тестирования
    /// </summary>
    private class FakePositionNameUniquenessCriteria : IPositionNameUniquenessCriteria
    {
        private readonly HashSet<string> _existingNames = new(StringComparer.OrdinalIgnoreCase);

        public void AddExistingName(string name) => _existingNames.Add(name);

        public bool IsSatisfiedBy(NotEmptyName name) => !_existingNames.Contains(name.Value);
    }

    #endregion

    #region CreateNew Tests

    /// <summary>
    /// Тест успешного создания должности
    /// </summary>
    [Fact]
    public void CreateNew_WithValidData_ShouldCreatePosition()
    {
        // Arrange
        var criteria = new FakePositionNameUniquenessCriteria();
        var name = NotEmptyName.Create("Старший разработчик");
        var description = PositionDescription.Create("Разработка и поддержка ПО");

        // Act
        var position = Position.CreateNew(criteria, name, description);

        // Assert
        Assert.NotNull(position);
        Assert.NotEqual(Guid.Empty, position.Id.Value);
        Assert.Equal(name.Value, position.Name.Value);
        Assert.Equal(description.Value, position.Description.Value);
        Assert.True(position.LifeTime.IsActive);
        Assert.True(position.LifeTime.CreatedAt != default);
    }

    /// <summary>
    /// Тест ошибки при создании должности с существующим именем
    /// </summary>
    [Fact]
    public void CreateNew_WithDuplicateName_ShouldThrowArgumentException()
    {
        // Arrange
        var criteria = new FakePositionNameUniquenessCriteria();
        var existingName = NotEmptyName.Create("Старший разработчик");
        criteria.AddExistingName(existingName.Value);

        var description = PositionDescription.Create("Разработка и поддержка ПО");

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(
            () => Position.CreateNew(criteria, existingName, description)
        );

        Assert.Contains("Название должности уже существует", exception.Message);
    }

    #endregion

    #region ChangePositionName Tests

    /// <summary>
    /// Тест успешного изменения названия должности
    /// </summary>
    [Fact]
    public void ChangePositionName_WithUniqueName_ShouldChangeName()
    {
        // Arrange
        var criteria = new FakePositionNameUniquenessCriteria();
        var oldName = NotEmptyName.Create("Младший разработчик");
        var newName = NotEmptyName.Create("Старший разработчик");
        var description = PositionDescription.Create("Разработка и поддержка ПО");

        var position = Position.CreateNew(criteria, oldName, description);
        var originalUpdatedAt = position.LifeTime.UpdatedAt;

        // Act
        position.ChangePositionName(criteria, newName);

        // Assert
        Assert.Equal(newName.Value, position.Name.Value);
        Assert.NotEqual(originalUpdatedAt, position.LifeTime.UpdatedAt);
    }

    /// <summary>
    /// Тест ошибки при изменении названия на уже существующее
    /// </summary>
    [Fact]
    public void ChangePositionName_WithDuplicateName_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var criteria = new FakePositionNameUniquenessCriteria();
        var existingName = NotEmptyName.Create("Существующая должность");
        criteria.AddExistingName(existingName.Value);

        var currentName = NotEmptyName.Create("Текущая должность");
        var description = PositionDescription.Create("Описание должности");

        var position = Position.CreateNew(criteria, currentName, description);

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(
            () => position.ChangePositionName(criteria, existingName)
        );

        Assert.Contains("Название должности уже используется", exception.Message);
    }

    /// <summary>
    /// Тест ошибки при изменении названия неактивной должности
    /// </summary>
    [Fact]
    public void ChangePositionName_WithArchivedPosition_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var criteria = new FakePositionNameUniquenessCriteria();
        var name = NotEmptyName.Create("Тестовая должность");
        var description = PositionDescription.Create("Описание должности");

        var position = Position.CreateNew(criteria, name, description);
        position.Archive(); // Архивируем должность

        var newName = NotEmptyName.Create("Новое имя");

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(
            () => position.ChangePositionName(criteria, newName)
        );

        Assert.Contains("находится в архиве", exception.Message);
    }

    #endregion
}
