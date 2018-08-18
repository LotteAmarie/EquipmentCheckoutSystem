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

            if (item.IsAvailable)
            {
                return true;
            }

            return false;
        }

        public void Checkout(int employeeId, IEnumerable<int> itemSerials)
        {
            // TODO:
        }

        public void CheckIn(int employeeId, int itemSerial)
        {
            // TODO:
        }

        public void CheckIn(int employeeId, IEnumerable<int> itemSerials)
        {
            // TODO:
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