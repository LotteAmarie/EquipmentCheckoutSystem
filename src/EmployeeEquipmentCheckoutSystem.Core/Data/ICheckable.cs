namespace EmployeeEquipmentCheckoutSystem.Core.Data
{
    public interface ICheckable
    {
        int SerialNumber { get; set; }
        bool IsAvailable { get; set; }
    }
}