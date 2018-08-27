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

        [Fact]
        public void RemoveDigits_RemovesDigits()
        {
            //Given
            var s = "here are some digits 1234";
            var expected = "here are some digits ";

            //When
            var actual = s.RemoveDigits();

            //Then
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void OnlyDigits_ReturnsOnlyDigits()
        {
            //Given
            var s = "here are some digits 1234";
            var expected = "1234";

            //When
            var actual = s.OnlyDigits();

            //Then
            Assert.Equal(expected, actual);
        }
    }
}