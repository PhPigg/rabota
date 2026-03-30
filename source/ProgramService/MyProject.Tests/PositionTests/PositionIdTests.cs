using Domain.PositionsContext.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace MyProject.Tests.PositionTests
{
    public class PositionIdTests
    {
        /// <summary>
        /// Тест успешного создания через фабричный метод Create
        /// </summary>
        [Fact]
        public void Create_WithValidGuid_ShouldCreateInstance()
        {
            // Arrange
            var expectedGuid = Guid.NewGuid();

            // Act
            var positionId = PositionId.Create(expectedGuid);

            // Assert
            Assert.NotNull(positionId);
            Assert.Equal(expectedGuid, positionId.Value);
        }

        /// <summary>
        /// Тест создания нового уникального ID
        /// </summary>
        [Fact]
        public void CreateNew_ShouldCreateNewInstanceWithUniqueGuid()
        {
            // Act
            var positionId1 = PositionId.CreateNew();
            var positionId2 = PositionId.CreateNew();

            // Assert
            Assert.NotNull(positionId1);
            Assert.NotNull(positionId2);
            Assert.NotEqual(positionId1.Value, positionId2.Value);
            Assert.NotEqual(Guid.Empty, positionId1.Value);
            Assert.NotEqual(Guid.Empty, positionId2.Value);
        }

        /// <summary>
        /// Тест проверки валидации при создании с пустым GUID
        /// </summary>
        [Fact]
        public void Create_WithEmptyGuid_ShouldThrowArgumentException()
        {
            // Arrange
            var emptyGuid = Guid.Empty;

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(
                () => PositionId.Create(emptyGuid)
            );

            // Проверяем детали исключения
            Assert.Equal("value", exception.ParamName);
            Assert.Contains("не может быть пустым", exception.Message);
        }

        /// <summary>
        /// Тест проверки валидации с разными невалидными значениями
        /// </summary>
        [Theory]
        [InlineData("00000000-0000-0000-0000-000000000000")] // Пустой GUID
        public void Create_WithInvalidGuids_ShouldThrowException(string invalidGuidString)
        {
            // Arrange
            var invalidGuid = Guid.Parse(invalidGuidString);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => PositionId.Create(invalidGuid));
        }
    }
}
