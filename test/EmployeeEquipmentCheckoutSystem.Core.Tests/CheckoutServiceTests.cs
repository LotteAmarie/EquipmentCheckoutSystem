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
        #region TestSetup
        private CheckoutContext _context;

        public CheckoutServiceTests()
        {
            // TODO: Consider refactoring data lists.
            var equipmentData = new List<ICheckable>
            {
                new Equipment 
                { 
                    SerialNumber = 100, 
                    Location = "Warehouse 1", 
                    LastCheckedById = 0, 
                    RequestedByIds = new List<int> { },
                    IsAvailable = true, 
                    RequiredSafetyLevel = SafetyLevel.A 
                },
                new Equipment 
                { 
                    SerialNumber = 200, 
                    Location = "Warehouse 2", 
                    LastCheckedById = 0, 
                    RequestedByIds = new List<int> { },
                    IsAvailable = true, 
                    RequiredSafetyLevel = SafetyLevel.B 
                },
                new Equipment 
                { 
                    SerialNumber = 300, 
                    Location = "Warehouse 3", 
                    LastCheckedById = 003, 
                    RequestedByIds = new List<int> { },
                    IsAvailable = false, 
                    RequiredSafetyLevel = SafetyLevel.C 
                }
            };
            var employeeData = new List<Employee>
            {
                new Employee 
                {
                    Id = 001,
                    CheckedItems = new List<ICheckable> { },
                    CheckedItemHistory = new List<ICheckable> { equipmentData.First(e => e.SerialNumber == 100) },
                    EMailAddress = "001@somewhere.com",
                    MaximumSafetyClearance = SafetyLevel.A
                },
                new Employee
                {
                    Id = 002,
                    CheckedItems = new List<ICheckable> { },
                    CheckedItemHistory = new List<ICheckable> { },
                    EMailAddress = "002@somewhere.com",
                    MaximumSafetyClearance = SafetyLevel.B
                },
                new Employee 
                { 
                    Id = 003,
                    CheckedItems = new List<ICheckable> { equipmentData.First(e => e.SerialNumber == 300) },
                    CheckedItemHistory = new List<ICheckable> { },
                    EMailAddress = "003@somewhere.com",
                    MaximumSafetyClearance = SafetyLevel.C
                },
                new Employee
                {
                    Id = 004,
                    CheckedItems = new List<ICheckable> { },
                    CheckedItemHistory = new List<ICheckable> { },
                    EMailAddress = "004@somewhere.com",
                    MaximumSafetyClearance = SafetyLevel.D
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
            dbSet.Setup(m => m.Remove(It.IsAny<T>())).Callback<T>((entity) => sourceList.Remove(entity));

            return dbSet.Object;
        }

        #endregion
        #region CheckoutTests
        [Fact]
        public void CheckoutSuccessful_WhenItemIsAvailable()
        {
            //Given
            var service = new CheckoutService(_context);

            //When
            var result = service.Checkout(001, 100);

            //Then
            Assert.True(result);
        }

        [Fact]
        public void CheckoutUnsuccessful_WhenItemIsUnavailable()
        {
            //Given
            var service = new CheckoutService(_context);

            //When
            var result = service.Checkout(001, 300);
            
            //Then
            Assert.False(result);
        }

        [Fact]
        public void CheckoutSuccessful_WhenEmployeeMeetsSafetyRequirement()
        {
            //Given
            var service = new CheckoutService(_context);

            //When
            var result = service.Checkout(001, 100);
            
            //Then
            Assert.True(result);
        }

        [Fact]
        public void CheckoutUnsuccessful_WhenEmployeeFailsSafetyRequirement()
        {
            //Given
            var service = new CheckoutService(_context);

            //When
            var result = service.Checkout(002, 100);
            
            //Then
            Assert.False(result);
        }

        [Fact]
        public void Checkout_MarksItemAsUnavailable()
        {
            //Given
            var service = new CheckoutService(_context);
            var expected = false;

            //When
            service.Checkout(001, 100);
            var actual = _context.Equipment.First(e => e.SerialNumber == 100).IsAvailable;
            
            //Then
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Checkout_AddsItemToEmployeeCheckedItems()
        {
            //Given
            var service = new CheckoutService(_context);
            var employee = _context.Employees.First(e => e.Id == 001);
            var item = _context.Equipment.First(e => e.SerialNumber == 100);

            //When
            service.Checkout(001, 100);
            
            //Then
            Assert.True(employee.CheckedItems.Contains(item));
        }

        [Fact]
        public void Checkout_AddsEmployeeLastCheckedBy()
        {
            //Given
            var service = new CheckoutService(_context);
            var expected = _context.Employees.First(e => e.Id == 001).Id;

            //When
            service.Checkout(001, 100);
            var actual = _context.Equipment.First(e => e.SerialNumber == 100).LastCheckedById;

            //Then
            Assert.Equal(expected, actual);
        }
        #endregion
        #region CheckInTests
        [Fact]
        public void CheckIn_SuccessfulWhenItemIsUnavailable()
        {
            //Given
            var service = new CheckoutService(_context);

            //When
            var result = service.CheckIn(003, 300);

            //Then
            Assert.True(result);
        }

        [Fact]
        public void CheckIn_UnsuccessfulWhenItemIsAvailable()
        {
            //Given
            var service = new CheckoutService(_context);

            //When
            var result = service.CheckIn(003, 200);

            //Then
            Assert.False(result);
        }

        [Fact]
        public void CheckIn_FailsWithIncorrectEmployeeId()
        {
            //Given
            var service = new CheckoutService(_context);

            //When
            var result = service.CheckIn(002, 300);
            
            //Then
            Assert.False(result);
        }
        
        [Fact]
        public void CheckIn_SuccessfulWithCorrectEmployeeId()
        {
            //Given
            var service = new CheckoutService(_context);

            //When
            var result = service.CheckIn(003, 300);
            
            //Then
            Assert.True(result);
        }

        [Fact]
        public void CheckIn_MarksItemAvailable()
        {
            //Given
            var service = new CheckoutService(_context);
            var expected = true;

            //When
            service.CheckIn(003, 300);
            var actual = _context.Equipment.First(e => e.SerialNumber == 300).IsAvailable;
            
            //Then
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CheckIn_RemovesItemFromEmployeeCheckedItems()
        {
            //Given
            var service = new CheckoutService(_context);
            var employee = _context.Employees.First(e => e.Id == 003);
            var item = _context.Equipment.First(e => e.SerialNumber == 300);

            //When
            service.CheckIn(003, 300);
            
            //Then
            Assert.False(employee.CheckedItems.Contains(item));
        }

        [Fact]
        public void CheckIn_AddsItemToEmployeeCheckedItemsHistory()
        {
            //Given
            var service = new CheckoutService(_context);
            var employee = _context.Employees.First(e => e.Id == 003);
            var item = _context.Equipment.First(e => e.SerialNumber == 300);

            //When
            service.CheckIn(003, 300);
            
            //Then
            Assert.True(employee.CheckedItemHistory.Contains(item));
        }

        [Fact]
        public void CheckIn_ContactsWaitList()
        {
            // TODO: https://stackoverflow.com/questions/7766083/unit-test-check-if-a-method-is-being-called#7766839
            //Given
            
            //When
            
            //Then
        }
        #endregion
        #region RequestItemTests
        [Fact]
        public void RequestItem_FailsWhenItemAvailable()
        {
            //Given
            var service = new CheckoutService(_context);
            
            //When
            var result = service.RequestItem(002, 200);
            
            //Then
            Assert.False(result);
        }

        [Fact]
        public void RequestItem_SuccessfulWhenItemIsUnavailable()
        {
            //Given
            var service = new CheckoutService(_context);

            //When
            var result = service.RequestItem(002, 300);
            
            //Then
            Assert.True(result);
        }

        [Fact]
        public void RequestItem_AddsEmployeeIdToRequestedById()
        {
            //Given
            var service = new CheckoutService(_context);
            var item = _context.Equipment.First(e => e.SerialNumber == 300);

            //When
            service.RequestItem(002, 300);
            
            //Then
            Assert.True(item.RequestedByIds.Contains(002));
        }

        [Fact]
        public void RequestItem_FailsUponDuplicateEmployeeRequest()
        {
            //Given
            var service = new CheckoutService(_context);

            //When
            service.RequestItem(002, 300);
            var result = service.RequestItem(002, 300);
            
            //Then
            Assert.False(result);
        }
        [Fact]
        public void RequestItem_FailsUponRequestWithInsufficientSecurityLevel()
        {
            //Given
            var service = new CheckoutService(_context);

            //When
            var result = service.RequestItem(004, 300);

            //Then
            Assert.False(result);
        }
        #endregion
        #region GetEmployeeByIdTests
        [Fact]
        public void GetEmployeeById_ReturnsCorrectEmployee()
        {
            //Given
            var service = new CheckoutService(_context);
            var expected = _context.Employees.Single(e => e.Id == 001);

            //When
            var actual = service.GetEmployeeById(001);
            
            //Then
            // TODO: Consider overloading equality on employee
            Assert.Equal(expected.Id, actual.Id);
            Assert.Equal(expected.EMailAddress, actual.EMailAddress);
            Assert.Equal(expected.MaximumSafetyClearance, actual.MaximumSafetyClearance);
        }
        
        [Fact]
        public void GetEmployeeById_ThrowsInvalidArgumentExceptionGivenNonexistantId()
        {
            //Given
            var service = new CheckoutService(_context);

            //When
            Action act = () => service.GetEmployeeById(009);
            
            //Then
            Assert.Throws<ArgumentException>(act);
        }
        #endregion
        #region GetCheckableBySerialTests
        [Fact]
        public void GetEquipmentBySerial_ReturnsCorrectEquipment()
        {
            //Given
            var service = new CheckoutService(_context);
            var expected = _context.Equipment.Single(e => e.SerialNumber == 100);

            //When
            var actual = service.GetCheckableBySerial(100);

            //Then
            // TODO: consider overloading equality on equipment
            Assert.Equal(expected.SerialNumber, actual.SerialNumber);
            Assert.Equal(expected.Location, actual.Location);
            Assert.Equal(expected.LastCheckedById, actual.LastCheckedById);
            Assert.Equal(expected.IsAvailable, actual.IsAvailable);
            Assert.Equal(expected.RequiredSafetyLevel, actual.RequiredSafetyLevel);
        }
        [Fact]
        public void GetEquipmentBySerial_ThrowsInvalidArgumentExceptionGivenNonexistantSerial()
        {
            //Given
            var service = new CheckoutService(_context);

            //When
            Action act = () => service.GetCheckableBySerial(009);
            
            //Then
            Assert.Throws<ArgumentException>(act);
        }
        #endregion
        #region AddEmployeeTests
        [Fact]
        public void AddEmployee_SuccessfullyAddsEmployee()
        {
            //Given
            var service = new CheckoutService(_context);
            var employee = new Employee
            {
                Id = 005,
                CheckedItems = new List<ICheckable> { },
                CheckedItemHistory = new List<ICheckable> { },
                EMailAddress = "005@somewhere.com",
                MaximumSafetyClearance = SafetyLevel.C
            };

            //When
            service.AddEmployee(employee);
            
            //Then
            Assert.Contains(employee, _context.Employees);
        }
        [Fact]
        public void AddEmployee_ThrowsArgumentExceptionOnIdCollision()
        {
            //Given
            var service = new CheckoutService(_context);
            var employee = new Employee 
            {
                Id = 001,
                CheckedItems = new List<ICheckable> { },
                CheckedItemHistory = new List<ICheckable> { },
                EMailAddress = "001@somewhere.com",
                MaximumSafetyClearance = SafetyLevel.A
            };
            
            //When
            Action act = () => service.AddEmployee(employee);
            
            //Then
            Assert.Throws<ArgumentException>(act);
        }

        #endregion
        #region AddCheckableTests
        [Fact]
        public void AddCheckable_SuccessfullyAddsCheckable()
        {
            //Given
            var service = new CheckoutService(_context);
            var item = new Equipment 
            { 
                SerialNumber = 900, 
                Location = "Warehouse 1", 
                LastCheckedById = 0, 
                RequestedByIds = new List<int> { },
                IsAvailable = true, 
                RequiredSafetyLevel = SafetyLevel.A 
            };

            //When
            service.AddCheckable(item);

            //Then
            Assert.Contains(item, _context.Equipment);
        }
        [Fact]
        public void AddCheckable_ThrowsArgumentExceptionOnSerialCollision()
        {
            //Given
            var service = new CheckoutService(_context);
            var item = new Equipment 
            { 
                SerialNumber = 300, 
                Location = "Warehouse 1", 
                LastCheckedById = 0, 
                RequestedByIds = new List<int> { },
                IsAvailable = true, 
                RequiredSafetyLevel = SafetyLevel.A 
            };
            
            //When
            Action act = () => service.AddCheckable(item);
            
            //Then
            Assert.Throws<ArgumentException>(act);
        }
        #endregion
        #region RemoveEmployeeTests
        [Fact]
        public void RemoveEmployee_SuccessfullyRemovesEmployee()
        {
            //Given
            var service = new CheckoutService(_context);
            var employee = _context.Employees.First(e => e.Id == 001);

            //When
            service.RemoveEmployee(employee);
            
            //Then
            Assert.DoesNotContain(employee, _context.Employees);
        }
        #endregion
        #region RemoveCheckableTests
        [Fact]
        public void RemoveCheckable_SuccessfullyRemovesCheckable()
        {
            //Given
            var service = new CheckoutService(_context);
            var item = _context.Equipment.First(e => e.SerialNumber == 100);

            //When
            service.RemoveCheckable(item);
            
            //Then
            Assert.DoesNotContain(item, _context.Equipment);
        }
        #endregion
    }
}