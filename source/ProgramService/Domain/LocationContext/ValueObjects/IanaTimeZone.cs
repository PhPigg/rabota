using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.LocationContext.ValueObjects
{
    public class IanaTimeZone
    {
        public string Value { get; }

        private IanaTimeZone(string value)
        {
            Value = value;
        }
        public static IanaTimeZone Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentNullException("Значение было пустым.", nameof(value));
            if (!value.Contains('/'))
            {
                throw new ArgumentException("Значение было введено не корректно.", nameof(value));
            }
            string[] parts = value.Split('/');
            if (parts.Length != 2)
            {
                throw new ArgumentException("Значение не корректное.", nameof(value));
            }
            if (parts.Any(p => string.IsNullOrWhiteSpace(p)))
            {
                throw new ArgumentException("Значение не корректное.", nameof(value));
            }
            return new IanaTimeZone(value);
        }
    }
}
