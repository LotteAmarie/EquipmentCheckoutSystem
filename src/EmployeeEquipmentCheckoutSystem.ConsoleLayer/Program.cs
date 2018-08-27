using System;
using EmployeeEquipmentCheckoutSystem.Core;
using EmployeeEquipmentCheckoutSystem.Core.Data;
using EmployeeEquipmentCheckoutSystem.Core.Extensions;

namespace EmployeeEquipmentCheckoutSystem.ConsoleLayer
{
    class Program
    {
        static void Main(string[] args)
        {
            bool loop = true;

            while (loop)
            {             
                Console.WriteLine("Employee Equipment Checkout System v1.00 -- ecs h for help");
                Console.WriteLine("Please login using 'ecs lin' + your employee id.");

                var userInput = Console.ReadLine().RemoveWhitespace();
                Employee user = null;

                switch (userInput.RemoveDigits())
                {
                    case "ecsh":
                    case "ecshelp":
                        PrintAllCommands(user);
                        break;
                    case "ecslin":
                    case "ecslogin":
                        using (var context = new CheckoutContext())
                        {
                            var service = new CheckoutService(context);

                            try
                            {
                                if (int.TryParse(userInput.OnlyDigits(), out int employeeId))
                                {
                                    user = service.GetEmployeeById(employeeId);
                                }
                            }
                            catch (ArgumentException)
                            {
                                Console.WriteLine("Error: Employee Id given is either invalid or does not exist.");
                            }
                        }
                        break;
                    case "ecslout":
                    case "ecslogout":
                        user = null;
                        break;
                    case "ecsout":
                    case "ecscheckout":
                        using (var context = new CheckoutContext())
                        {
                            var service = new CheckoutService(context);

                            try
                            {
                                if (int.TryParse(userInput.OnlyDigits(), out int itemSerial))
                                {
                                    service.Checkout(user.Id, itemSerial);
                                }
                            }
                            catch (ArgumentException)
                            {
                                Console.WriteLine("Error: Item serial given is either invalid or does not exist.");
                            }
                        }
                        break;
                    case "ecsin":
                    case "ecscheckin":
                        using (var context = new CheckoutContext())
                        {
                            var service = new CheckoutService(context);

                            try
                            {
                                if (int.TryParse(userInput.OnlyDigits(), out int itemSerial))
                                {
                                    service.CheckIn(user.Id, itemSerial);
                                }
                            }
                            catch (ArgumentException) // TODO: More detailed error messages
                            {
                                Console.WriteLine("Error: Item serial given is either invalid or does not exist.");
                            }
                        }
                        break;
                    case "ecsexit":
                        loop = false;
                        break;
                    default:
                        break;
                }
            }
        }

        static void PrintAllCommands(Employee user)
        {
            System.Console.WriteLine(@"
                ecs lin or ecs login {employeeId} : Logs the given employee for future transactions.\n
                ecs lout or ecs logout {employeeId} : Logs out the given employee from all future transactions.\n
                ecs out or ecs checkout {itemSerial} : Checks out given item to the logged employee.\n
                ecs in or ecs checkin {itemSerial} : Checks in given item from the logged employee.\n
                ecs exit : exits the ecs.
            ");
        }
    }
}
