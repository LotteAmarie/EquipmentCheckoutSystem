namespace EmployeeEquipmentCheckoutSystem.Core.Data
{
    public class Equipment : ICheckable
    {
        public int SerialNumber { get; set; }
        public string Location { get; set; }
        // TODO: Band-aid fix, look into FK relationships in EF Core
        public int LastCheckedById { get; set; }
        public bool IsAvailable { get; set; }
        // TODO: Band-aid fix, look into FK relationships in EF Core
        public int RequestedById { get; set; }
        public SafetyLevel RequiredSafetyLevel { get; set; }
    }
}