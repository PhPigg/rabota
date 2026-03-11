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

        //департамент, ответственный за должность
        public DepartmentId DepartmentId { get; }
        public Department Department { get; }

        //должность, относящаяся к департаменту
        public PositionId PositionId { get; }
        public Position Position { get; }
    }
}

