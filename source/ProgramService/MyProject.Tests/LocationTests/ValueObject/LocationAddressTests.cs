using Domain.LocationContext.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyProject.Tests.LocationTests.ValueObject
{
    public class LocationAddressTests
    {
        [Fact]
        public void Create_WithValidFullAddress_ShouldCreateLocationAddress()
        {
            // Arrange
            string address = "Москва, ул. Тверская, д. 15, кв. 10";

            // Act
            var locationAddress = LocationAddress.Create(address);

            // Assert
            Assert.NotNull(locationAddress);
            Assert.Equal(address, locationAddress.Value);
            Assert.Equal(4, locationAddress.AddressParts.Count);
            Assert.Equal("Москва", locationAddress.AddressParts[0]);
            Assert.Equal("ул. Тверская", locationAddress.AddressParts[1]);
            Assert.Equal("д. 15", locationAddress.AddressParts[2]);
            Assert.Equal("кв. 10", locationAddress.AddressParts[3]);
        }

        [Fact]
        public void Create_WithSinglePart_ShouldCreateLocationAddress()
        {
            // Arrange
            string address = "Москва";

            // Act
            var locationAddress = LocationAddress.Create(address);

            // Assert
            Assert.NotNull(locationAddress);
            Assert.Equal(address, locationAddress.Value);
            Assert.Single(locationAddress.AddressParts);
            Assert.Equal("Москва", locationAddress.AddressParts[0]);
        }

        [Fact]
        public void Create_WithExtraSpaces_ShouldTrimParts()
        {
            // Arrange
            string address = "  Москва  ,  ул. Тверская  ,  д. 15  ";

            // Act
            var locationAddress = LocationAddress.Create(address);

            // Assert
            Assert.Equal("Москва, ул. Тверская, д. 15", locationAddress.Value);
            Assert.Equal(3, locationAddress.AddressParts.Count);
            Assert.Equal("Москва", locationAddress.AddressParts[0]);
            Assert.Equal("ул. Тверская", locationAddress.AddressParts[1]);
            Assert.Equal("д. 15", locationAddress.AddressParts[2]);
        }

        [Fact]
        public void Create_WithEmptyPartsAfterSplit_ShouldFilterEmptyParts()
        {
            // Arrange
            string address = "Москва,, ,ул. Тверская,,";

            // Act
            var locationAddress = LocationAddress.Create(address);

            // Assert
            Assert.Equal("Москва, ул. Тверская", locationAddress.Value);
            Assert.Equal(2, locationAddress.AddressParts.Count);
            Assert.Equal("Москва", locationAddress.AddressParts[0]);
            Assert.Equal("ул. Тверская", locationAddress.AddressParts[1]);
        }

        [Fact]
        public void Create_WithNullValue_ShouldThrowArgumentNullException()
        {
            // Arrange
            string address = null;

            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(
                () => LocationAddress.Create(address)
            );
            Assert.Contains("Адрес локации не может быть пустым", exception.Message);
            Assert.Equal("value", exception.ParamName);
        }

        [Fact]
        public void Create_WithEmptyString_ShouldThrowArgumentNullException()
        {
            // Arrange
            string address = "";

            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(
                () => LocationAddress.Create(address)
            );
            Assert.Contains("Адрес локации не может быть пустым", exception.Message);
            Assert.Equal("value", exception.ParamName);
        }

        [Fact]
        public void Create_WithWhiteSpaceOnly_ShouldThrowArgumentNullException()
        {
            // Arrange
            string address = "   ";

            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(
                () => LocationAddress.Create(address)
            );
            Assert.Contains("Адрес локации не может быть пустым", exception.Message);
            Assert.Equal("value", exception.ParamName);
        }

        [Fact]
        public void Create_WithOnlyCommas_ShouldThrowArgumentException()
        {
            // Arrange
            string address = ",,,";

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(
                () => LocationAddress.Create(address)
            );
            Assert.Contains(
                "Адрес локации должен содержать хотя бы одну значимую часть",
                exception.Message
            );
            Assert.Equal("value", exception.ParamName);
        }

        [Fact]
        public void Create_WithOnlyWhiteSpaceParts_ShouldThrowArgumentException()
        {
            // Arrange
            string address = "  , ,  , ";

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(
                () => LocationAddress.Create(address)
            );
            Assert.Contains(
                "Адрес локации должен содержать хотя бы одну значимую часть",
                exception.Message
            );
            Assert.Equal("value", exception.ParamName);
        }

        [Fact]
        public void AddressParts_ShouldBeReadOnly()
        {
            // Arrange
            var locationAddress = LocationAddress.Create("Москва, ул. Тверская");

            // Act & Assert
            Assert.IsAssignableFrom<IReadOnlyList<string>>(locationAddress.AddressParts);
        }

        [Fact]
        public void Value_ShouldReturnFormattedAddress()
        {
            // Arrange
            var locationAddress = LocationAddress.Create("Москва, ул. Тверская, д. 15");

            // Act
            string value = locationAddress.Value;

            // Assert
            Assert.Equal("Москва, ул. Тверская, д. 15", value);
        }

        [Fact]
        public void TwoInstances_WithSameAddress_ShouldBeEqual()
        {
            // Arrange
            string address = "Москва, ул. Тверская";
            var address1 = LocationAddress.Create(address);
            var address2 = LocationAddress.Create(address);

            // Act & Assert
            Assert.Equal(address1, address2);
            Assert.True(address1 == address2);
            Assert.Equal(address1.GetHashCode(), address2.GetHashCode());
        }

        [Fact]
        public void TwoInstances_WithDifferentAddresses_ShouldNotBeEqual()
        {
            // Arrange
            var address1 = LocationAddress.Create("Москва, ул. Тверская");
            var address2 = LocationAddress.Create("Санкт-Петербург, Невский пр.");

            // Act & Assert
            Assert.NotEqual(address1, address2);
            Assert.True(address1 != address2);
        }
    }
}
