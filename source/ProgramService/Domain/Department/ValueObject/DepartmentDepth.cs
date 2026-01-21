using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Department.ValueObject
{
    public class DepartmentDepth
    {
        public short Value { get; }
        private DepartmentDepth(short value)
        {
            Value = value;
        }
        public static DepartmentDepth Create(short value)
        {
            if (value < 1)
            {
                throw new ArgumentException("Глубина не может быть отрицательной.", nameof(value));
            }
            return new DepartmentDepth(value);
        }
    }
}
