using System;
using System.Collections.Generic;
using System.Linq;
using EmployeeEquipmentCheckoutSystem.Core.Data;
using EmployeeEquipmentCheckoutSystem.Core.Extensions;
using Microsoft.EntityFrameworkCore;

namespace EmployeeEquipmentCheckoutSystem.Core
{
    // TODO: Async
    public class CheckoutService
    {
        private CheckoutContext _context;

        public CheckoutService(CheckoutContext context)
        {
            _context = context;
        }

        public bool Checkout(int employeeId, int itemSerial)
        {
            var employee = GetEmployeeById(employeeId);
            var item = GetCheckableBySerial(itemSerial);

            if (item.IsAvailable && employee.IsAuthorizedFor(item))
            {
                employee.CheckedItems.Add(item);
                item.IsAvailable = false;
                item.LastCheckedById = employee.Id;

                _context.SaveChanges();

                return true;
            }

            return false;
        }

        public bool CheckIn(int employeeId, int itemSerial)
        {
            var employee = GetEmployeeById(employeeId);
            var item = GetCheckableBySerial(itemSerial);

            if (!item.IsAvailable && item.LastCheckedById == employeeId)
            {
                item.IsAvailable = true;
                employee.CheckedItems.Remove(item);
                employee.CheckedItemHistory.Add(item);

                _context.SaveChanges();
                return true;
            }

            return false;
        }

        public bool RequestItem(int employeeId, int itemSerial)
        {
            var employee = GetEmployeeById(employeeId);
            var item = GetCheckableBySerial(itemSerial);

            if (!item.IsAvailable && 
                !item.RequestedByIds.Contains(employeeId) && 
                employee.IsAuthorizedFor(item))
            {
                item.RequestedByIds.Add(employeeId);

                _context.SaveChanges();
                return true;
            }

            return false;
        }

        /// <exception cref="System.ArgumentException">Thrown when given invalid id</exception>
        public Employee GetEmployeeById(int id)
        {
            try
            {
                return _context.Employees.Single(e => e.Id == id);
            }
            catch (InvalidOperationException ex)
            {
                throw new ArgumentException("Invalid employee ID given.", ex);
            }
        }

        /// <exception cref="System.ArgumentException">Thrown when given invalid serial</exception>
        public ICheckable GetCheckableBySerial(int serial)
        {
            try
            {
                return _context.Equipment.Single(e => e.SerialNumber == serial);
            }
            catch (InvalidOperationException ex)
            {
                throw new ArgumentException("Invalid employee ID given.", ex);
            }
        }

        public void AddEmployee(Employee employee)
        {
            if (_context.Employees.Any(e => e.Id == employee.Id))
                throw new ArgumentException("Cannot enter duplicate employee");

            _context.Employees.Add(employee);
            _context.SaveChanges();
        }

        public void AddCheckable(ICheckable checkable)
        {
            if (_context.Equipment.Any(e => e.SerialNumber == checkable.SerialNumber))
                throw new ArgumentException("Cannot enter duplicate checkable");

            _context.Equipment.Add(checkable);
            _context.SaveChanges();
        }

        public void RemoveEmployee(Employee employee)
        {
            _context.Employees.Attach(employee);
            _context.Employees.Remove(employee);
            _context.SaveChanges();
        }

        public void RemoveCheckable(ICheckable checkable)
        {
            _context.Equipment.Attach(checkable);
            _context.Equipment.Remove(checkable);
            _context.SaveChanges();
        }
    }
}