using Domain.DepartmentContext.ValueObject;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyProject.Tests.DepartmentTests.ValueObject
{
    /**
 * <summary>
 * Содержит unit-тесты для Value Object <see cref="DepartmentPath"/>.
 * </summary>
 */
    public class DepartmentPathTests
    {
        /**
         * <summary>
         * Проверяет, что метод <see cref="DepartmentPath.Create"/> успешно создает экземпляр
         * для всех валидных строковых представлений пути.
         * </summary>
         * <param name="validValue">Валидная строка пути.</param>
         */
        [Theory]
        [InlineData("Головной офис")]
        [InlineData("Головной офис/IT")]
        [InlineData("Головной офис/IT/Разработка")]
        [InlineData("Sales")]
        [InlineData("Sales/North Region")]
        [InlineData("IT/Backend/API")]
        [InlineData("Руководство/Финансовый отдел/Бухгалтерия")]
        [InlineData("Products/Electronics/Smartphones")]
        [InlineData("HR/Recruitment/Technical Recruiters")]
        [InlineData("Отдел разработки/Команда фронтенда")]
        [InlineData("/Корневой отдел/Дочерний")] // Путь может начинаться с разделителя
        [InlineData("Отдел 1/Отдел 2/Отдел 3/Отдел 4/Отдел 5")] // Глубокая вложенность
        [InlineData("Простой путь")]
        [InlineData("123/456/789")] // Путь с цифрами
        [InlineData("path_with_underscores")]
        [InlineData("path-with-dashes")]
        public void Create_WithValidPath_ShouldReturnDepartmentPathInstance(string validValue)
        {
            // Act
            var path = DepartmentPath.Create(validValue);

            // Assert
            Assert.NotNull(path);
            Assert.Equal(validValue, path.Value);
        }

        /**
         * <summary>
         * Проверяет, что метод <see cref="DepartmentPath.Create"/> выбрасывает исключение
         * <see cref="ArgumentException"/> при передаче пустой строки, null или строки, 
         * состоящей только из пробелов.
         * </summary>
         * <param name="invalidValue">Пустая строка, null или строка с пробелами.</param>
         */
        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("  ")]
        [InlineData("\t")]
        [InlineData("\n")]
        [InlineData("\r\n")]
        [InlineData(null)]
        public void Create_WithNullOrWhiteSpace_ShouldThrowArgumentException(string invalidValue)
        {
            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(
                () => DepartmentPath.Create(invalidValue)
            );

            Assert.Contains("Путь не может быть пустым или состоять из пробелов", exception.Message);
            Assert.Equal("value", exception.ParamName);
        }

        /**
         * <summary>
         * Проверяет, что <see cref="DepartmentPath"/> как record-тип обеспечивает
         * сравнение экземпляров по значению, а не по ссылке.
         * </summary>
         */
        [Fact]
        public void DepartmentPath_ShouldBeRecord_WithValueEquality()
        {
            // Arrange
            var path1 = DepartmentPath.Create("Головной офис/IT/Разработка");
            var path2 = DepartmentPath.Create("Головной офис/IT/Разработка");
            var path3 = DepartmentPath.Create("Головной офис/IT/Тестирование");

            // Assert
            Assert.Equal(path1, path2);
            Assert.NotEqual(path1, path3);
            Assert.True(path1 == path2);
            Assert.False(path1 == path3);
            Assert.Equal(path1.GetHashCode(), path2.GetHashCode());
            Assert.NotEqual(path1.GetHashCode(), path3.GetHashCode());
        }

        /**
         * <summary>
         * Проверяет, что при многократном создании экземпляров с одинаковым значением
         * возвращаются разные объекты, которые равны по значению.
         * </summary>
         * <param name="value">Значение пути для тестирования.</param>
         */
        [Theory]
        [InlineData("IT")]
        [InlineData("Головной офис/IT")]
        [InlineData("Sales/North Region/Manager")]
        public void MultipleCreations_ShouldReturnDifferentInstances(string value)
        {
            // Act
            var path1 = DepartmentPath.Create(value);
            var path2 = DepartmentPath.Create(value);

            // Assert
            Assert.NotSame(path1, path2);
            Assert.Equal(path1, path2);
        }

        /**
         * <summary>
         * Проверяет, что свойство <see cref="DepartmentPath.Value"/> доступно только для чтения
         * и не может быть изменено после создания объекта.
         * </summary>
         */
        [Fact]
        public void DepartmentPath_ShouldBeImmutable()
        {
            // Arrange
            var path = DepartmentPath.Create("Головной офис/IT");

            // Assert
            Assert.Equal("Головной офис/IT", path.Value);

            // Проверяем, что свойство Value не имеет setter'а
            var propertyInfo = typeof(DepartmentPath).GetProperty("Value");
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
            var maxLengthString = new string('A', 10000); // Очень длинная строка пути

            // Act
            var minPath = DepartmentPath.Create(singleChar);
            var maxPath = DepartmentPath.Create(maxLengthString);

            // Assert
            Assert.Equal(singleChar, minPath.Value);
            Assert.Equal(maxLengthString, maxPath.Value);
        }

        /**
         * <summary>
         * Проверяет, что путь может содержать различные допустимые символы:
         * буквы, цифры, разделители, пробелы, дефисы, подчеркивания.
         * </summary>
         */
        [Fact]
        public void Create_WithVariousValidCharacters_ShouldSucceed()
        {
            // Arrange
            var validPaths = new[]
            {
            "Отдел разработки/Team Alpha/Секция 1",
            "IT Department/Backend Team/API Development",
            "Products/2024/Electronics",
            "HR/Recruitment_2024",
            "Sales/North-Region",
            "Головной офис/Финансы/Бухгалтерия/Налоги",
            "R&D/Lab-3/Project_X",
            "Support/Клиентская поддержка/Чат-поддержка"
        };

            // Act & Assert
            foreach (var pathValue in validPaths)
            {
                var path = DepartmentPath.Create(pathValue);
                Assert.NotNull(path);
                Assert.Equal(pathValue, path.Value);
            }
        }

        /**
         * <summary>
         * Проверяет, что путь может содержать символы различных алфавитов.
         * </summary>
         */
        [Theory]
        [InlineData("English/Russian/Русский")]
        [InlineData("Deutsch/Österreich/Swiss")]
        [InlineData("Français/Belgique/Suisse")]
        [InlineData("Español/México/Argentina")]
        [InlineData("中文/简体/繁體")]
        [InlineData("日本語/東京/大阪")]
        [InlineData("한국어/서울/부산")]
        public void Create_WithInternationalCharacters_ShouldSucceed(string value)
        {
            // Act
            var path = DepartmentPath.Create(value);

            // Assert
            Assert.NotNull(path);
            Assert.Equal(value, path.Value);
        }

        /**
         * <summary>
         * Проверяет, что путь может содержать специальные символы, часто используемые
         * в названиях отделов.
         * </summary>
         */
        [Theory]
        [InlineData("IT & Development/Backend")]
        [InlineData("R&D/Lab (Research)")]
        [InlineData("Sales/Marketing/Customer Support")]
        [InlineData("HR/Recruitment [Technical]")]
        [InlineData("Finance/Accounts Payable/Accounts Receivable")]
        [InlineData("Legal/Compliance/Regulatory Affairs")]
        public void Create_WithSpecialCharacters_ShouldSucceed(string value)
        {
            // Act
            var path = DepartmentPath.Create(value);

            // Assert
            Assert.NotNull(path);
            Assert.Equal(value, path.Value);
        }

        /**
         * <summary>
         * Проверяет, что путь может содержать числовые значения и комбинации с буквами.
         * </summary>
         */
        [Theory]
        [InlineData("Floor 1/Floor 2/Office 3")]
        [InlineData("Building A/Section B/Room 123")]
        [InlineData("2024/Projects/Project_001")]
        [InlineData("Version 2.0/Release Candidate")]
        public void Create_WithNumbersAndAlphanumeric_ShouldSucceed(string value)
        {
            // Act
            var path = DepartmentPath.Create(value);

            // Assert
            Assert.NotNull(path);
            Assert.Equal(value, path.Value);
        }

        /**
         * <summary>
         * Проверяет, что путь может содержать пробелы и сохраняет их как есть.
         * </summary>
         */
        [Theory]
        [InlineData("Головной офис", "Головной офис")]
        [InlineData("  Начальный пробел", "  Начальный пробел")]
        [InlineData("Конечный пробел  ", "Конечный пробел  ")]
        [InlineData("Внутренние   множественные   пробелы", "Внутренние   множественные   пробелы")]
        public void Create_WithSpaces_PreservesSpaces(string input, string expected)
        {
            // Act
            var path = DepartmentPath.Create(input);

            // Assert
            Assert.Equal(expected, path.Value);
        }

        /**
         * <summary>
         * Проверяет, что путь может содержать различные разделители и комбинации путей.
         * </summary>
         */
        [Theory]
        [InlineData("Parent/Child")]
        [InlineData("Parent/Child/Grandchild")]
        [InlineData("Parent\\Child")] // Обратный слеш
        [InlineData("Parent|Child|Grandchild")] // Вертикальная черта
        [InlineData("Parent.Child.Grandchild")] // Точка
        [InlineData("Parent-Child-Grandchild")] // Дефис
        public void Create_WithDifferentDelimiters_ShouldSucceed(string value)
        {
            // Act
            var path = DepartmentPath.Create(value);

            // Assert
            Assert.NotNull(path);
            Assert.Equal(value, path.Value);
        }

        /**
         * <summary>
         * Проверяет, что метод Create выбрасывает исключение при попытке создать путь,
         * состоящий только из пробелов после Trim'а.
         * </summary>
         */
        [Theory]
        [InlineData("   ")]
        [InlineData("\t\t\t")]
        [InlineData("\n\n\n")]
        [InlineData(" \t \n ")]
        public void Create_WithOnlyWhitespace_ShouldThrowArgumentException(string invalidValue)
        {
            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(
                () => DepartmentPath.Create(invalidValue)
            );

            Assert.Contains("Путь не может быть пустым или состоять из пробелов", exception.Message);
            Assert.Equal("value", exception.ParamName);
        }
    }
}
