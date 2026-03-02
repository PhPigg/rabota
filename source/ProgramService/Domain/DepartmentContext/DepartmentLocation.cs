using Domain.DepartmentContext.ValueObject;
using Domain.LocationContext;
using Domain.LocationContext.ValueObjects;

namespace Domain.DepartmentContext
{
    public class DepartmentLocation
    {
        public DepartmentLocation(Department department, Location location)
        {
            Department = department;
            Location = location;
            DepartmentId = department.Id;
            LocationId = location.Id;
        }

        public DepartmentId DepartmentId { get; }
        public LocationId LocationId { get; }
        public Department Department { get; }
        public Location Location { get; }
    }
}

