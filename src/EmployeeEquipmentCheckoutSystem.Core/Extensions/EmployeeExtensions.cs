using EmployeeEquipmentCheckoutSystem.Core.Data;

namespace EmployeeEquipmentCheckoutSystem.Core
{
    public static class EmployeeExtensions
    {
        public static bool IsAuthorizedFor(this Employee employee, ICheckable item)
        {
            if (employee.MaximumSafetyClearance >= item.RequiredSafetyLevel)
                return true;
            return false;
        }
    }
}