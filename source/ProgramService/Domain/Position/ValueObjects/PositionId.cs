using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Position.ValueObjects
{
    public class PositionId
    {
        public Guid Value { get; }

        private PositionId(Guid value)
        {
            Value = value;
        }
        public static PositionId Create(Guid value)
        {
            if (value == Guid.Empty)
            {
                throw new ArgumentException("Идентификатор не может быть пустым.", nameof(value));
            }

            return new PositionId(value);
        }
    }
}
