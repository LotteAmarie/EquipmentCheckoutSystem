using System.Collections.Generic;

namespace EmployeeEquipmentCheckoutSystem.Core.Data
{
    public interface ICheckable
    {
        int SerialNumber { get; set; }
        string Location { get; set; }
        bool IsAvailable { get; set; }
        List<int> RequestedByIds { get; set; }
        int LastCheckedById { get; set; }
        SafetyLevel RequiredSafetyLevel { get; set; }
    }
}