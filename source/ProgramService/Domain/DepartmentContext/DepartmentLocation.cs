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

        //департамент ответственный за локацию
        public DepartmentId DepartmentId { get; }
        public Department Department { get; }
        
        //локация относящаяся к департаменту
        public LocationId LocationId { get; }
        public Location Location { get; }
    }
}

