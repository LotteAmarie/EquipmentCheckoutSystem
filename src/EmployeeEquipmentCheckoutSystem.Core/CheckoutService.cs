using System;
using System.Collections.Generic;
using System.Linq;
using EmployeeEquipmentCheckoutSystem.Core.Data;

namespace EmployeeEquipmentCheckoutSystem.Core
{
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

        public bool Checkout(int employeeId, IEnumerable<int> itemSerials)
        {
            // TODO:
            throw new NotImplementedException();
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

        public bool CheckIn(int employeeId, IEnumerable<int> itemSerials)
        {
            // TODO:
            throw new NotImplementedException();
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

        public bool RequestItem(int employeeId, IEnumerable<int> itemSerials)
        {
            // TODO:
            throw new NotImplementedException();
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
            _context.Employees.Add(employee);
            _context.SaveChanges();
        }

        public void AddCheckable(ICheckable checkable)
        {

        }
    }
}