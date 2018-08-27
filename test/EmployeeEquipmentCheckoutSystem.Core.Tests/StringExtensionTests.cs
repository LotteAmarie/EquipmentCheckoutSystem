using Xunit;
using EmployeeEquipmentCheckoutSystem.Core.Extensions;

namespace EmployeeEquipmentCheckoutSystem.Core.Tests
{
    public class StringExtensionTests
    {
        [Fact]
        public void RemoveWhitespace_RemovesWhitespace()
        {
            //Given
            var s = "here is some whitespace";
            var expected = "hereissomewhitespace";

            //When
            var actual = s.RemoveWhitespace();

            //Then
            Assert.Equal(expected, actual);
        }
    }
}