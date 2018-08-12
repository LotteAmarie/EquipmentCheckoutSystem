using System.Collections.Generic;

namespace EmployeeEquipmentCheckoutSystem.Core.Data
{
    public class Employee
    {
        public int Id { get; set; }
        public IEnumerable<ICheckable> CheckedItems { get; set; }
        public IEnumerable<ICheckable> RequestedItems { get; set; }
        public IEnumerable<ICheckable> CheckedItemHistory { get; set; }
        public SafetyLevel MaximumSafetyClearance { get; set; }
    }
}