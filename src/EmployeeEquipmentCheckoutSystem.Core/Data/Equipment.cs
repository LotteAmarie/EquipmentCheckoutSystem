namespace EmployeeEquipmentCheckoutSystem.Core.Data
{
    public class Equipment : ICheckable
    {
        public int SerialNumber { get; set; }
        public string Location { get; set; }
        public Employee LastCheckedBy { get; set; }
        public bool IsAvailable { get; set; }

    }
}