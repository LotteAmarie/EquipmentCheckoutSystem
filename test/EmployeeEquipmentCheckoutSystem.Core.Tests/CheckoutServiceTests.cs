using System;
using System.Collections.Generic;
using System.Linq;
using EmployeeEquipmentCheckoutSystem.Core.Data;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace EmployeeEquipmentCheckoutSystem.Core.Tests
{
    public class CheckoutServiceTests 
    {
        private CheckoutContext _context;

        public CheckoutServiceTests()
        {
            var equipmentData = new List<Equipment>
            {
                new Equipment { SerialNumber = 100, Location = "Warehouse 1", LastCheckedBy = null, IsAvailable = true },
                new Equipment { SerialNumber = 200, Location = "Warehouse 2", LastCheckedBy = null, IsAvailable = true },
                new Equipment { SerialNumber = 300, Location = "Warehouse 3", LastCheckedBy = null, IsAvailable = false }
            }.AsQueryable();
            var employeeData = new List<Employee>
            {
                new Employee 
                {
                    Id = 001,
                    CheckedItems = new List<Equipment> { },
                    RequestedItems = new List<Equipment> { },
                    CheckedItemHistory = new List<Equipment> { equipmentData.First(e => e.SerialNumber == 100) },
                    EMailAddress = "001@gmail.com",
                    MaximumSafetyClearance = SafetyLevel.A
                },
                new Employee
                {
                    Id = 002,
                    CheckedItems = new List<Equipment> { },
                    RequestedItems = new List<Equipment> { equipmentData.First(e => e.SerialNumber == 300) },
                    CheckedItemHistory = new List<Equipment> { },
                    EMailAddress = "002@gmail.com",
                    MaximumSafetyClearance = SafetyLevel.B
                },
                new Employee 
                { 
                    Id = 003,
                    CheckedItems = new List<Equipment> { equipmentData.First(e => e.SerialNumber == 300) },
                    RequestedItems = new List<Equipment> { },
                    CheckedItemHistory = new List<Equipment> { },
                    EMailAddress = "003@gmail.com",
                    MaximumSafetyClearance = SafetyLevel.C
                }
            }.AsQueryable();

            var mockEquipment = new Mock<DbSet<Equipment>>();
            mockEquipment.As<IQueryable<Equipment>>().Setup(m => m.Provider).Returns(equipmentData.Provider);
            mockEquipment.As<IQueryable<Equipment>>().Setup(m => m.Expression).Returns(equipmentData.Expression);
            mockEquipment.As<IQueryable<Equipment>>().Setup(m => m.ElementType).Returns(equipmentData.ElementType);
            mockEquipment.As<IQueryable<Equipment>>().Setup(m => m.GetEnumerator()).Returns(equipmentData.GetEnumerator());
            var mockEmployees = new Mock<DbSet<Employee>>();
            mockEmployees.As<IQueryable<Employee>>().Setup(m => m.Provider).Returns(employeeData.Provider);
            mockEmployees.As<IQueryable<Employee>>().Setup(m => m.Expression).Returns(employeeData.Expression);
            mockEmployees.As<IQueryable<Employee>>().Setup(m => m.ElementType).Returns(employeeData.ElementType);
            mockEmployees.As<IQueryable<Employee>>().Setup(m => m.GetEnumerator()).Returns(employeeData.GetEnumerator());

            var mockContext = new Mock<CheckoutContext>();
            mockContext.Setup(m => m.Employees).Returns(mockEmployees.Object);
            mockContext.Setup(m => m.Equipment).Returns(mockEquipment.Object);

            _context = mockContext.Object;
        }

        [Fact]
        public void CheckoutSuccessful_WhenItemIsAvailable()
        {
            //Given

            //When
            
            //Then
        }
    }
}