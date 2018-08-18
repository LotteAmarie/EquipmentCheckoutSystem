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
            };
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
            };
          
            _context = Mock.Of<CheckoutContext>(m => 
                m.Equipment == GetQueryableMockDbSet(equipmentData) &&
                m.Employees == GetQueryableMockDbSet(employeeData));
        }

        private static DbSet<T> GetQueryableMockDbSet<T>(List<T> sourceList) where T : class
        {
            var queryable = sourceList.AsQueryable();

            var dbSet = new Mock<DbSet<T>>();
            dbSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
            dbSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
            dbSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            dbSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(() => queryable.GetEnumerator());
            dbSet.Setup(d => d.Add(It.IsAny<T>())).Callback<T>((s) => sourceList.Add(s));

            return dbSet.Object;
        }

        [Fact]
        public void CheckoutSuccessful_WhenItemIsAvailable()
        {
            //Given
            var service = new CheckoutService(_context);

            //When
            var actual = service.Checkout(001, 100);

            //Then
            Assert.True(actual);
        }
    }
}