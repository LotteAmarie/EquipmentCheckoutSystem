using System.Collections.Generic;

namespace EmployeeEquipmentCheckoutSystem.Core.Data
{
    public class Employee
    {
        public int Id { get; set; }
        public List<ICheckable> CheckedItems { get; set; }
        public List<ICheckable> RequestedItems { get; set; }
        //TODO: Checked Item History should likely contain data about time. Possibly just use a tuple here?
        public List<ICheckable> CheckedItemHistory { get; set; }
        public string EMailAddress { get; set; }
        public SafetyLevel MaximumSafetyClearance { get; set; }
    }
}