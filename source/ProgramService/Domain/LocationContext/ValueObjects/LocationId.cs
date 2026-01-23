using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.LocationContext.ValueObjects
{
    public class LocationId
    {
        public Guid Value { get; }

        private LocationId(Guid value)
        {
            Value = value;
        }
        public static LocationId Create(Guid value)
        {
            if (value == Guid.Empty)
            {
                throw new ArgumentException("Идентификатор не может быть пустым.", nameof(value));
            }
            
            return new LocationId(value);
        }
    }
}
