using Domain.DepartmentContext.ValueObject;
using Domain.PositionsContext;
using Domain.PositionsContext.ValueObjects;

namespace Domain.DepartmentContext
{
    public class DepartmentPosition
    {
        public DepartmentPosition(Department department, Position position)
        {
            Department = department;
            Position = position;
            DepartmentId = department.Id;
            PositionId = position.Id;
        }

        public DepartmentId DepartmentId { get; }
        public PositionId PositionId { get; }
        public Department Department { get; }
        public Position Position { get; }
    }
}

