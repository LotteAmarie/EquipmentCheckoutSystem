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
            var item = _context.Equipment.First(e => e.SerialNumber == itemSerial);
            var employee = _context.Employees.First(e => e.Id == employeeId);

            if (item.IsAvailable && employee.MaximumSafetyClearance >= item.RequiredSafetyLevel)
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
            var item = _context.Equipment.First(e => e.SerialNumber == itemSerial);
            var employee = _context.Employees.First(e => e.Id == employeeId);

            if (!item.IsAvailable && item.LastCheckedById == employeeId)
            {
                item.IsAvailable = true;

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

        public void RequestItem(int employeeId, int itemSerial)
        {
            // TODO:
        }

        public void RequestItem(int employeeId, IEnumerable<int> itemSerials)
        {
            // TODO:
        }
    }
}