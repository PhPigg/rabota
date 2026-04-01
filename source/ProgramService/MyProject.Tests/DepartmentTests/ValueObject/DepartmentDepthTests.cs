using Domain.DepartmentContext.ValueObject;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyProject.Tests.DepartmentTests.ValueObject
{
    /**
 * <summary>
 * Содержит unit-тесты для Value Object <see cref="DepartmentDepth"/>.
 * </summary>
 */
    public class DepartmentDepthTests
    {
        /**
         * <summary>
         * Проверяет, что метод <see cref="DepartmentDepth.Create"/> успешно создает экземпляр
         * для всех валидных значений глубины вложенности.
         * </summary>
         * <param name="validValue">Валидное значение глубины (1 или выше).</param>
         */
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(5)]
        [InlineData(10)]
        [InlineData(100)]
        [InlineData(short.MaxValue)]
        public void Create_WithValidValue_ShouldReturnDepartmentDepthInstance(short validValue)
        {
            // Act
            var depth = DepartmentDepth.Create(validValue);

            // Assert
            Assert.NotNull(depth);
            Assert.Equal(validValue, depth.Value);
        }

        /**
         * <summary>
         * Проверяет, что метод <see cref="DepartmentDepth.Create"/> выбрасывает исключение
         * <see cref="ArgumentException"/> при передаче значения меньше 1.
         * </summary>
         * <param name="invalidValue">Невалидное значение глубины (0 или отрицательное).</param>
         */
        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-5)]
        [InlineData(-100)]
        [InlineData(short.MinValue)]
        public void Create_WithValueLessThanOne_ShouldThrowArgumentException(short invalidValue)
        {
            // Act & Assert
            var exception = Assert.Throws<ArgumentException>
            (
                () => DepartmentDepth.Create(invalidValue)
            );

            Assert.Contains("Глубина должна быть положительным числом больше нуля", exception.Message);
            Assert.Equal("value", exception.ParamName);
        }

        /**
         * <summary>
         * Проверяет, что <see cref="DepartmentDepth"/> как record-тип обеспечивает
         * сравнение экземпляров по значению, а не по ссылке.
         * </summary>
         */
        [Fact]
        public void DepartmentDepth_ShouldBeRecord_WithValueEquality()
        {
            // Arrange
            var depth1 = DepartmentDepth.Create(5);
            var depth2 = DepartmentDepth.Create(5);
            var depth3 = DepartmentDepth.Create(10);

            // Assert
            Assert.Equal(depth1, depth2);
            Assert.NotEqual(depth1, depth3);
            Assert.True(depth1 == depth2);
            Assert.False(depth1 == depth3);
            Assert.Equal(depth1.GetHashCode(), depth2.GetHashCode());
            Assert.NotEqual(depth1.GetHashCode(), depth3.GetHashCode());
        }

        /**
         * <summary>
         * Проверяет корректность работы с граничными значениями глубины:
         * минимальное значение (1) и максимальное значение (short.MaxValue).
         * </summary>
         */
        [Fact]
        public void Create_WithBoundaryValues_ShouldWorkCorrectly()
        {
            // Arrange & Act
            var minDepth = DepartmentDepth.Create(1);
            var maxDepth = DepartmentDepth.Create(short.MaxValue);

            // Assert
            Assert.Equal(1, minDepth.Value);
            Assert.Equal(short.MaxValue, maxDepth.Value);
        }

        /**
         * <summary>
         * Проверяет, что при многократном создании экземпляров с одинаковым значением
         * возвращаются разные объекты, которые равны по значению.
         * </summary>
         * <param name="value">Значение глубины для тестирования.</param>
         */
        [Theory]
        [InlineData(1)]
        [InlineData(50)]
        public void MultipleCreations_ShouldReturnDifferentInstances(short value)
        {
            // Act
            var depth1 = DepartmentDepth.Create(value);
            var depth2 = DepartmentDepth.Create(value);

            // Assert
            Assert.NotSame(depth1, depth2);
            Assert.Equal(depth1, depth2);
        }

        /**
         * <summary>
         * Проверяет, что свойство <see cref="DepartmentDepth.Value"/> доступно только для чтения
         * и не может быть изменено после создания объекта.
         * </summary>
         */
        [Fact]
        public void DepartmentDepth_ShouldBeImmutable()
        {
            // Arrange
            var depth = DepartmentDepth.Create(5);

            // Assert
            // Проверяем, что свойство Value не имеет setter'а (компилируется)
            // и значение соответствует заданному
            Assert.Equal(5, depth.Value);

            // Дополнительная проверка: объект не может быть изменен через рефлексию
            // (Value property should be init-only or readonly)
            var propertyInfo = typeof(DepartmentDepth).GetProperty("Value");
            Assert.NotNull(propertyInfo);
            Assert.False(propertyInfo.CanWrite, "Свойство Value не должно иметь setter'а");
        }
    }
}
