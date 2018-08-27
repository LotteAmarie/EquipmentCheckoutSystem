using System.Collections.Generic;
using EmployeeEquipmentCheckoutSystem.Core.Data;
using EmployeeEquipmentCheckoutSystem.Core.Extensions;
using Xunit;

namespace EmployeeEquipmentCheckoutSystem.Core.Tests
{
    public class EmployeeExtensionTests
    {
        [Fact]
        public void IsAuthorizedFor_TrueWhenSafetyLevelIsSufficient()
        {
            //Given
            var employee = new Employee 
            {
                Id = 001,
                CheckedItems = new List<ICheckable> { },
                CheckedItemHistory = new List<ICheckable> { },
                EMailAddress = "001@somewhere.com",
                MaximumSafetyClearance = SafetyLevel.A
            };

            var equipment = new Equipment 
            { 
                SerialNumber = 100, 
                Location = "Warehouse 1", 
                LastCheckedById = 0, 
                RequestedByIds = new List<int> { },
                IsAvailable = true, 
                RequiredSafetyLevel = SafetyLevel.A 
            };

            //When
            var result = employee.IsAuthorizedFor(equipment);

            //Then
            Assert.True(result);
        }

        [Fact]
        public void IsAuthorizedFor_FalseWhenSafetyLevelIsInsufficient()
        {
            //Given
            var employee = new Employee 
            {
                Id = 001,
                CheckedItems = new List<ICheckable> { },
                CheckedItemHistory = new List<ICheckable> { },
                EMailAddress = "001@somewhere.com",
                MaximumSafetyClearance = SafetyLevel.C
            };

            var equipment = new Equipment 
            { 
                SerialNumber = 100, 
                Location = "Warehouse 1", 
                LastCheckedById = 0, 
                RequestedByIds = new List<int> { },
                IsAvailable = true, 
                RequiredSafetyLevel = SafetyLevel.A 
            };

            //When
            var result = employee.IsAuthorizedFor(equipment);

            //Then
            Assert.False(result);
        }
    }
}