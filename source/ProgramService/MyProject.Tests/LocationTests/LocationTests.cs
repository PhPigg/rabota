using Domain.LocationContext;
using Domain.LocationContext.ValueObjects;
using Domain.Shared;

namespace MyProject.Tests.LocationTests;

/// <summary>
/// Тесты для сущности Location
/// </summary>
public class LocationTests
{
    #region Вспомогательные классы

    /// <summary>
    /// Fake-реализация критерия уникальности для тестирования
    /// </summary>
    private class FakeLocationUniquenessCriteria : ILocationUniquenessCriteria
    {
        private readonly HashSet<string> _existingNames = new(StringComparer.OrdinalIgnoreCase);
        private readonly HashSet<string> _existingAddresses = new(StringComparer.OrdinalIgnoreCase);

        public void AddExistingName(string name) => _existingNames.Add(name);
        public void AddExistingAddress(string address) => _existingAddresses.Add(address);

        public bool IsSatisfiedBy(NotEmptyName name) => !_existingNames.Contains(name.Value);
        public bool IsSatisfiedBy(LocationAddress address) => !_existingAddresses.Contains(address.Value);
    }

    #endregion

    #region CreateNew Tests

    /// <summary>
    /// Тест успешного создания локации
    /// </summary>
    [Fact]
    public void CreateNew_WithValidData_ShouldCreateLocation()
    {
        // Arrange
        var criteria = new FakeLocationUniquenessCriteria();
        var address = LocationAddress.Create("Москва, Ленина 1");
        var timeZone = IanaTimeZone.Create("Europe/Moscow");
        var name = NotEmptyName.Create("Тестовая локация");

        // Act
        var location = Location.CreateNew(criteria, address, timeZone, name);

        // Assert
        Assert.NotNull(location);
        Assert.NotEqual(Guid.Empty, location.Id.Value);
        Assert.Equal(name.Value, location.Name.Value);
        Assert.Equal(address.Value, location.Address.Value);
        Assert.Equal(timeZone.Value, location.TimeZone.Value);
        Assert.True(location.LifeTime.IsActive);
        Assert.NotNull(location.LifeTime.CreatedAt);
    }

    /// <summary>
    /// Тест ошибки при создании локации с существующим именем
    /// </summary>
    [Fact]
    public void CreateNew_WithDuplicateName_ShouldThrowArgumentException()
    {
        // Arrange
        var criteria = new FakeLocationUniquenessCriteria();
        var existingName = NotEmptyName.Create("Существующая локация");
        criteria.AddExistingName(existingName.Value);

        var address = LocationAddress.Create("Москва, Ленина 1");
        var timeZone = IanaTimeZone.Create("Europe/Moscow");

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(
            () => Location.CreateNew(criteria, address, timeZone, existingName)
        );

        Assert.Contains("Название локации уже существует", exception.Message);
    }

    /// <summary>
    /// Тест ошибки при создании локации с существующим адресом
    /// </summary>
    [Fact]
    public void CreateNew_WithDuplicateAddress_ShouldThrowArgumentException()
    {
        // Arrange
        var criteria = new FakeLocationUniquenessCriteria();
        var existingAddress = LocationAddress.Create("Москва, Ленина 1");
        criteria.AddExistingAddress(existingAddress.Value);

        var name = NotEmptyName.Create("Новая локация");
        var timeZone = IanaTimeZone.Create("Europe/Moscow");

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(
            () => Location.CreateNew(criteria, existingAddress, timeZone, name)
        );

        Assert.Contains("Адрес локации уже существует", exception.Message);
    }

    #endregion

    #region ChangeIanaTimeZone Tests

    /// <summary>
    /// Тест успешного изменения часового пояса
    /// </summary>
    [Fact]
    public void ChangeIanaTimeZone_WithActiveLocation_ShouldChangeTimeZone()
    {
        // Arrange
        var criteria = new FakeLocationUniquenessCriteria();
        var address = LocationAddress.Create("Москва, Ленина 1");
        var oldTimeZone = IanaTimeZone.Create("Europe/Moscow");
        var newTimeZone = IanaTimeZone.Create("Europe/London");
        var name = NotEmptyName.Create("Тестовая локация");

        var location = Location.CreateNew(criteria, address, oldTimeZone, name);
        var originalUpdatedAt = location.LifeTime.UpdatedAt;

        // Act
        location.ChangeIanaTimeZone(newTimeZone);

        // Assert
        Assert.Equal(newTimeZone.Value, location.TimeZone.Value);
        Assert.NotEqual(originalUpdatedAt, location.LifeTime.UpdatedAt);
    }

    /// <summary>
    /// Тест ошибки при изменении часового пояса неактивной локации
    /// </summary>
    [Fact]
    public void ChangeIanaTimeZone_WithArchivedLocation_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var criteria = new FakeLocationUniquenessCriteria();
        var address = LocationAddress.Create("Москва, Ленина 1");
        var timeZone = IanaTimeZone.Create("Europe/Moscow");
        var name = NotEmptyName.Create("Тестовая локация");

        var location = Location.CreateNew(criteria, address, timeZone, name);
        location.Archive(); // Архивируем локацию

        var newTimeZone = IanaTimeZone.Create("Europe/London");

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(
            () => location.ChangeIanaTimeZone(newTimeZone)
        );

        Assert.Contains("находится в архиве", exception.Message);
    }

    #endregion

    #region ChangeLocationName Tests

    /// <summary>
    /// Тест успешного изменения имени локации
    /// </summary>
    [Fact]
    public void ChangeLocationName_WithUniqueName_ShouldChangeName()
    {
        // Arrange
        var criteria = new FakeLocationUniquenessCriteria();
        var address = LocationAddress.Create("Москва, Ленина 1");
        var timeZone = IanaTimeZone.Create("Europe/Moscow");
        var oldName = NotEmptyName.Create("Старое имя");
        var newName = NotEmptyName.Create("Новое имя");

        var location = Location.CreateNew(criteria, address, timeZone, oldName);
        var originalUpdatedAt = location.LifeTime.UpdatedAt;

        // Act
        location.ChangeLocationName(criteria, newName);

        // Assert
        Assert.Equal(newName.Value, location.Name.Value);
        Assert.NotEqual(originalUpdatedAt, location.LifeTime.UpdatedAt);
    }

    /// <summary>
    /// Тест ошибки при изменении имени на уже существующее
    /// </summary>
    [Fact]
    public void ChangeLocationName_WithDuplicateName_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var criteria = new FakeLocationUniquenessCriteria();
        var existingName = NotEmptyName.Create("Существующее имя");
        criteria.AddExistingName(existingName.Value);

        var address = LocationAddress.Create("Москва, Ленина 1");
        var timeZone = IanaTimeZone.Create("Europe/Moscow");
        var currentName = NotEmptyName.Create("Текущее имя");

        var location = Location.CreateNew(criteria, address, timeZone, currentName);

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(
            () => location.ChangeLocationName(criteria, existingName)
        );

        Assert.Contains("Название локации уже используется", exception.Message);
    }

    /// <summary>
    /// Тест ошибки при изменении имени неактивной локации
    /// </summary>
    [Fact]
    public void ChangeLocationName_WithArchivedLocation_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var criteria = new FakeLocationUniquenessCriteria();
        var address = LocationAddress.Create("Москва, Ленина 1");
        var timeZone = IanaTimeZone.Create("Europe/Moscow");
        var name = NotEmptyName.Create("Тестовая локация");

        var location = Location.CreateNew(criteria, address, timeZone, name);
        location.Archive(); // Архивируем локацию

        var newName = NotEmptyName.Create("Новое имя");

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(
            () => location.ChangeLocationName(criteria, newName)
        );

        Assert.Contains("находится в архиве", exception.Message);
    }

    #endregion

    #region ChangeLocationAddress Tests

    /// <summary>
    /// Тест успешного изменения адреса локации
    /// </summary>
    [Fact]
    public void ChangeLocationAddress_WithUniqueAddress_ShouldChangeAddress()
    {
        // Arrange
        var criteria = new FakeLocationUniquenessCriteria();
        var oldAddress = LocationAddress.Create("Москва, Ленина 1");
        var newAddress = LocationAddress.Create("Санкт-Петербург, Невский 2");
        var timeZone = IanaTimeZone.Create("Europe/Moscow");
        var name = NotEmptyName.Create("Тестовая локация");

        var location = Location.CreateNew(criteria, oldAddress, timeZone, name);
        var originalUpdatedAt = location.LifeTime.UpdatedAt;

        // Act
        location.ChangeLocationAddress(criteria, newAddress);

        // Assert
        Assert.Equal(newAddress.Value, location.Address.Value);
        Assert.NotEqual(originalUpdatedAt, location.LifeTime.UpdatedAt);
    }

    /// <summary>
    /// Тест ошибки при изменении адреса на уже существующий
    /// </summary>
    [Fact]
    public void ChangeLocationAddress_WithDuplicateAddress_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var criteria = new FakeLocationUniquenessCriteria();
        var existingAddress = LocationAddress.Create("Москва, Ленина 1");
        criteria.AddExistingAddress(existingAddress.Value);

        var currentAddress = LocationAddress.Create("Москва, Тверская 2");
        var timeZone = IanaTimeZone.Create("Europe/Moscow");
        var name = NotEmptyName.Create("Тестовая локация");

        var location = Location.CreateNew(criteria, currentAddress, timeZone, name);

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(
            () => location.ChangeLocationAddress(criteria, existingAddress)
        );

        Assert.Contains("Адрес локации уже используется", exception.Message);
    }

    /// <summary>
    /// Тест ошибки при изменении адреса неактивной локации
    /// </summary>
    [Fact]
    public void ChangeLocationAddress_WithArchivedLocation_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var criteria = new FakeLocationUniquenessCriteria();
        var address = LocationAddress.Create("Москва, Ленина 1");
        var timeZone = IanaTimeZone.Create("Europe/Moscow");
        var name = NotEmptyName.Create("Тестовая локация");

        var location = Location.CreateNew(criteria, address, timeZone, name);
        location.Archive(); // Архивируем локацию

        var newAddress = LocationAddress.Create("Санкт-Петербург, Невский 2");

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(
            () => location.ChangeLocationAddress(criteria, newAddress)
        );

        Assert.Contains("находится в архиве", exception.Message);
    }

    #endregion
}
