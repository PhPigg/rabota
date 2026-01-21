using Domain.Shered;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Department.ValueObject
{
    public class DepartmentIdentifier
    {
        public string Value { get; }
        private DepartmentIdentifier(string value)
        {
            Value = value;
        }
        public static DepartmentIdentifier Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException($"Название псевдонима не может быть пустым.", nameof(value));
            if (value.Any(x=>x<'A' || x < 'a'))
                throw new ArgumentException("Название псевдонима только латиницой.", nameof(value));
            if (value.Any(x => x > 'Z' || x > 'z'))
                throw new ArgumentException("Название псевдонима только латиницой.", nameof(value));
            return new DepartmentIdentifier(value);
        }
    }
}
