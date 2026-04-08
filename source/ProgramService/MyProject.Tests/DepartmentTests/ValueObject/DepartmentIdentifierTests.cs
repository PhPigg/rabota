using Domain.DepartmentContext.ValueObject;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyProject.Tests.DepartmentTests.ValueObject
{
    /**
 * <summary>
 * Содержит unit-тесты для Value Object <see cref="DepartmentIdentifier"/>.
 * </summary>
 */
    public class DepartmentIdentifierTests
    {
        /**
         * <summary>
         * Проверяет, что метод <see cref="DepartmentIdentifier.Create"/> успешно создает экземпляр
         * для всех валидных строк, состоящих только из латинских букв.
         * </summary>
         * <param name="validValue">Валидная строка, содержащая только латинские буквы.</param>
         */
        [Theory]
        [InlineData("IT")]
        [InlineData("HR")]
        [InlineData("Sales")]
        [InlineData("Development")]
        [InlineData("Backend")]
        [InlineData("Frontend")]
        [InlineData("DevOps")]
        [InlineData("QA")]
        [InlineData("RND")]
        [InlineData("a")]
        [InlineData("Z")]
        [InlineData("AbCdEfG")]
        [InlineData("xyz")]
        public void Create_WithValidLatinString_ShouldReturnDepartmentIdentifierInstance(string validValue)
        {
            // Act
            var identifier = DepartmentIdentifier.Create(validValue);

            // Assert
            Assert.NotNull(identifier);
            Assert.Equal(validValue, identifier.Value);
        }

        /**
         * <summary>
         * Проверяет, что метод <see cref="DepartmentIdentifier.Create"/> выбрасывает исключение
         * <see cref="ArgumentException"/> при передаче пустой строки, null или строки, состоящей только из пробелов.
         * </summary>
         * <param name="invalidValue">Пустая строка, null или строка с пробелами.</param>
         */
        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("  ")]
        [InlineData("\t")]
        [InlineData("\n")]
        [InlineData(null)]
        public void Create_WithNullOrWhiteSpace_ShouldThrowArgumentException(string invalidValue)
        {
            // Act & Assert
            var exception = Assert.Throws<ArgumentException>
            (
                () => DepartmentIdentifier.Create(invalidValue)
            );

            Assert.Contains("Название псевдонима не может быть пустым", exception.Message);
            Assert.Equal("value", exception.ParamName);
        }

        /**
         * <summary>
         * Проверяет, что метод <see cref="DepartmentIdentifier.Create"/> выбрасывает исключение
         * <see cref="ArgumentException"/> при передаче строк, содержащих нелатинские символы.
         * </summary>
         * <param name="invalidValue">Строка, содержащая нелатинские символы.</param>
         */
        [Theory]
        [InlineData("ИТ")]              // Русские буквы
        [InlineData("Отдел")]           // Русские буквы
        [InlineData("123")]             // Цифры
        [InlineData("IT123")]           // Латинские буквы с цифрами
        [InlineData("HR_Department")]   // Символ подчеркивания
        [InlineData("IT-Department")]   // Дефис
        [InlineData("Sales.")]          // Точка
        [InlineData("IT@company")]      // Специальные символы
        [InlineData("IT ")]             // Пробел в конце
        [InlineData(" IT")]             // Пробел в начале
        [InlineData("IT Department")]   // Пробел внутри
        [InlineData("123456")]          // Только цифры
        [InlineData("тест")]            // Русские буквы
        [InlineData("混合")]             // Китайские иероглифы
        [InlineData("ñ")]              // Испанская буква
        [InlineData("ç")]              // Португальская буква
        [InlineData("ü")]              // Немецкий умлаут
        [InlineData("é")]              // Французская буква
        public void Create_WithNonLatinCharacters_ShouldThrowArgumentException(string invalidValue)
        {
            // Act & Assert
            var exception = Assert.Throws<ArgumentException>
            (
                () => DepartmentIdentifier.Create(invalidValue)
            );

            Assert.Contains("Название псевдонима должно содержать только латинские буквы.", exception.Message);
            Assert.Equal("value", exception.ParamName);
        }

        /**
         * <summary>
         * Проверяет, что <see cref="DepartmentIdentifier"/> как record-тип обеспечивает
         * сравнение экземпляров по значению, а не по ссылке.
         * </summary>
         */
        [Fact]
        public void DepartmentIdentifier_ShouldBeRecord_WithValueEquality()
        {
            // Arrange
            var identifier1 = DepartmentIdentifier.Create("IT");
            var identifier2 = DepartmentIdentifier.Create("IT");
            var identifier3 = DepartmentIdentifier.Create("HR");

            // Assert
            Assert.Equal(identifier1, identifier2);
            Assert.NotEqual(identifier1, identifier3);
            Assert.True(identifier1 == identifier2);
            Assert.False(identifier1 == identifier3);
            Assert.Equal(identifier1.GetHashCode(), identifier2.GetHashCode());
            Assert.NotEqual(identifier1.GetHashCode(), identifier3.GetHashCode());
        }

        /**
        * <summary>
        * Проверяет, что метод <see cref="DepartmentIdentifier.ToString"/> возвращает строку.
        * </summary>
        */
        [Fact]
        public void ToString_ShouldReturnString()
        {
            // Arrange
            var identifier = DepartmentIdentifier.Create("DevOps");

            // Act
            var result = identifier.ToString();

            // Assert
            Assert.IsType<string>(result); // Проверяет, что result имеет тип string
        }

        /**
         * <summary>
         * Проверяет, что при создании экземпляра значение сохраняется без изменений
         * и с сохранением регистра символов.
         * </summary>
         * <param name="value">Тестовое значение с разным регистром.</param>
         */
        [Theory]
        [InlineData("IT")]
        [InlineData("it")]
        [InlineData("It")]
        [InlineData("iT")]
        [InlineData("DevOps")]
        [InlineData("DEVOPS")]
        [InlineData("devops")]
        public void Create_PreservesOriginalCase(string value)
        {
            // Act
            var identifier = DepartmentIdentifier.Create(value);

            // Assert
            Assert.Equal(value, identifier.Value);
        }

        /**
         * <summary>
         * Проверяет, что при многократном создании экземпляров с одинаковым значением
         * возвращаются разные объекты, которые равны по значению.
         * </summary>
         * <param name="value">Значение идентификатора для тестирования.</param>
         */
        [Theory]
        [InlineData("IT")]
        [InlineData("HR")]
        [InlineData("Sales")]
        public void MultipleCreations_ShouldReturnDifferentInstances(string value)
        {
            // Act
            var identifier1 = DepartmentIdentifier.Create(value);
            var identifier2 = DepartmentIdentifier.Create(value);

            // Assert
            Assert.NotSame(identifier1, identifier2);
            Assert.Equal(identifier1, identifier2);
        }

        /**
         * <summary>
         * Проверяет, что свойство <see cref="DepartmentIdentifier.Value"/> доступно только для чтения
         * и не может быть изменено после создания объекта.
         * </summary>
         */
        [Fact]
        public void DepartmentIdentifier_ShouldBeImmutable()
        {
            // Arrange
            var identifier = DepartmentIdentifier.Create("IT");

            // Assert
            Assert.Equal("IT", identifier.Value);

            // Проверяем, что свойство Value не имеет setter'а
            var propertyInfo = typeof(DepartmentIdentifier).GetProperty("Value");
            Assert.NotNull(propertyInfo);
            Assert.False(propertyInfo.CanWrite, "Свойство Value не должно иметь setter'а");
        }

        /**
         * <summary>
         * Проверяет корректность работы с граничными значениями:
         * минимальная длина (1 символ) и максимальная возможная длина.
         * </summary>
         */
        [Fact]
        public void Create_WithBoundaryValues_ShouldWorkCorrectly()
        {
            // Arrange
            var singleChar = "A";
            var maxLengthString = new string('A', 1000); // Длинная строка из латинских букв

            // Act
            var minIdentifier = DepartmentIdentifier.Create(singleChar);
            var maxIdentifier = DepartmentIdentifier.Create(maxLengthString);

            // Assert
            Assert.Equal(singleChar, minIdentifier.Value);
            Assert.Equal(maxLengthString, maxIdentifier.Value);
        }

        /**
         * <summary>
         * Проверяет, что метод <see cref="DepartmentIdentifier.Create"/> правильно обрабатывает
         * строки, содержащие только латинские буквы в верхнем и нижнем регистре.
         * </summary>
         */
        [Fact]
        public void Create_WithMixedCaseLatinLetters_ShouldSucceed()
        {
            // Arrange
            var mixedCase = "AbCdEfGhIjKlMnOpQrStUvWxYz";

            // Act
            var identifier = DepartmentIdentifier.Create(mixedCase);

            // Assert
            Assert.NotNull(identifier);
            Assert.Equal(mixedCase, identifier.Value);
        }
    }
}
