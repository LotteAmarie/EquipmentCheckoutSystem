namespace EmployeeEquipmentCheckoutSystem.Core.Data
{
    /// <summary>
    /// Using the PPE Classification System Levels from OSHA and EPA
    /// Found Here: http://www.gryphonscientific.com/course/pdf/3_PPE_Levels_OSHA_EPA.pdf
    /// </summary>
    public enum SafetyLevel
    {
        // TODO: Research different safety classification systems.
        // Letters are reversed to map them to the enum's base number for value comparison.
        D, C, B, A
    }
}