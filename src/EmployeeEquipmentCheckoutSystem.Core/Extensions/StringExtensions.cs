using System.Linq;

namespace EmployeeEquipmentCheckoutSystem.Core.Extensions
{
    public static class StringExtensions
    {
        public static string RemoveWhitespace(this string input)
        {
            return new string(input.ToCharArray()
                .Where(c => !char.IsWhiteSpace(c))
                .ToArray());
        }

        public static string RemoveDigits(this string input)
        {
            return new string(input.ToCharArray()
                .Where(c => !char.IsDigit(c))
                .ToArray());
        }
    }
}