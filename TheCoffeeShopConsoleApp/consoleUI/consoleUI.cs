using Persistence;
using System.Globalization; // thu vien format tien 
using BL;
using Enum;
using System.Runtime.Serialization;
using System.Runtime.InteropServices;

namespace UI
{
    public class Ultilities
    {
        StaffBL staffBL = new StaffBL();
        public int MenuHandle(string? title, string[] menuItem, Staff staff)
        {
            int i = 0, choice;
            if (title != null)
                Title(title, staff);

            for (; i < menuItem.Count(); i++)
            {
                System.Console.WriteLine(" " + (i + 1) + ". " + menuItem[i]);
            }
            Line();
            do
            {
                System.Console.Write(" Your choice: ");
                int.TryParse(System.Console.ReadLine(), out choice);
            } while (choice <= 0 || choice > menuItem.Count());
            return choice;
        }
        public void Title(string title, Staff staff)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("Ｔｈｅ ＣｏｆｆｅｅＳｈｏｐ          Version : 1.0");
            Console.ForegroundColor = ConsoleColor.White;
            System.Console.WriteLine(title + " ");
            Console.WriteLine("\n STAFF: " + staff.StaffName);
            // Line();

        }
        public void Line()
        {
            System.Console.WriteLine("-===-===-===-===-===-===-===-===-===-===-===-===-===-===-===-===-===-===-===");
        }
        public void PressAnyKeyToContinue()
        {
            Console.Write("Press Any Key to continue...");
            Console.ReadKey();
            Console.Clear();
        }
        public void ShowSuccess(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(msg + "!");
            Console.ForegroundColor = ConsoleColor.White;
        }
        public void ShowError(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(msg + "!");
            Console.ForegroundColor = ConsoleColor.White;
        }

        public void PrintTimeLine(string[] phase, int currentPhase)
        {
            int itemCount = 0;
            Console.WriteLine("----------------------------------------------------------------------------------------------------------");
            foreach (string item in phase)
            {
                itemCount++;
                if (itemCount == currentPhase)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write(((itemCount == currentPhase) ? " > " + item : " > " + item));
                    Console.ForegroundColor = ConsoleColor.White;
                }
                else
                {
                    Console.Write(((itemCount == currentPhase) ? " > " + item : " > " + item));
                }
                if (itemCount == phase.Length)
                    Console.Write("\n");
            }
            Console.WriteLine("----------------------------------------------------------------------------------------------------------");
            itemCount = 0;
        }
        public void CreateOrderTitle()

        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("Ｔｈｅ ＣｏｆｆｅｅＳｈｏｐ           Version : 1.0");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(@"
                                     ▒█▀▀█ █▀▀█ █▀▀ █▀▀ █▀▀ █▀▀ ▒█▀▀▀█ █░░█ █▀▀█ █▀▀█ 　 ▒█▀▄▀█ █▀▀ █▀▀▄ █░░█ 
                                     ▒█░░░ █░░█ █▀▀ █▀▀ █▀▀ █▀▀ ░▀▀▀▄▄ █▀▀█ █░░█ █░░█ 　 ▒█▒█▒█ █▀▀ █░░█ █░░█ 
                                     ▒█▄▄█ ▀▀▀▀ ▀░░ ▀░░ ▀▀▀ ▀▀▀ ▒█▄▄▄█ ▀░░▀ ▀▀▀▀ █▀▀▀ 　 ▒█░░▒█ ▀▀▀ ▀░░▀ ░▀▀▀");

            Console.WriteLine("-===-===-===-===-===-===-===-===-===-===-===-===-===-===-===-===--===-===-===-===-===-===-===-===-===-===-===-===-===-===-===-===-===-===-===-==");
            Console.WriteLine("| {0, 10} | {1, 35} | {2, 35} | {3,51} |", "Product ID", "Product Name", "Product Description", "Size - Price");
            Console.WriteLine("-===-===-===-===-===-===-===-===-===-===-===-===-===-===-===-===--===-===-===-===-===-===-===-===-===-===-===-===-===-===-===-===-===-===-===-==");

        }
        public void ShowListProduct(List<Product> products)
        {
            ProductBL productBL = new ProductBL();
            foreach (Product item in products)
            {
                Console.Write("| {0, 10} | {1, 35} | {2, 35} |", item.ProductID, item.ProductName, item.Description);
                foreach (ProductSize ps in productBL.GetProductSizeByProductID(item.ProductID))
                {
                    Console.Write(" {0, 2} - {1, 10} |", ps.Size.size.ToUpper(), FormatPrice(ps.Price));
                }
                Console.Write("\n");
            }
            Console.WriteLine("-===-===-===-===-===-===-===-===-===-===-===-===-===-===-===-===--===-===-===-===-===-===-===-===-===-===-===-===-===-===-===-===-===-===-===-==");
        }
        public string FormatPrice(decimal price)
        {
            CultureInfo cultureInfo = new CultureInfo("vi-VN");
            return string.Format(cultureInfo, "{0:N0} VND", price);
        }
        public void PrintListOrder(List<Order> orders)
        {
            Console.WriteLine("-===-===-===-===-===-===-===-===-===-===-===-===-===-===-===-===--===-===-===-===-===-===-");
            Console.WriteLine("| {0, 10} | {1, 35} | {2, 35} |", "Order ID", "Create At", "Order Status");
            Console.WriteLine("-===-===-===-===-===-===-===-===-===-===-===-===-===-===-===-===--===-===-===-===-===-===-");
            foreach (var item in orders)
            {
                Console.WriteLine("| {0, 10} | {1, 35} | {2, 35} |", item.OrderID, item.CreateAt, item.Status);
            }
        }
        public void PrintOrder(Order order, Staff currentStaff)
        {
            OrderBL oBL = new OrderBL();
            Console.WriteLine(@"
▀▀█▀▀ █░░█ █▀▀   ▒█▀▀█ █▀▀█ █▀▀ █▀▀ █▀▀ █▀▀   ▒█▀▀▀█ █░░█ █▀▀█ █▀▀█ 
░▒█░░ █▀▀█ █▀▀   ▒█░░░ █░░█ █▀▀ █▀▀ █▀▀ █▀▀   ░▀▀▀▄▄ █▀▀█ █░░█ █░░█ 
░▒█░░ ▀░░▀ ▀▀▀   ▒█▄▄█ ▀▀▀▀ ▀░░ ▀░░ ▀▀▀ ▀▀▀   ▒█▄▄▄█ ▀░░▀ ▀▀▀▀ █▀▀▀ 
                                                       Version : 1.0");

            Console.WriteLine("ADD: 18 Tam Trinh- Hai Ba Trung- Ha Noi");
            Console.WriteLine("Phone: 0852394222 - 0969055545");
            Line();

            if (currentStaff.RoleID == 2)
            {
                Console.WriteLine(@"{0,20}", @"   
                      ░█▀▀█ █▀▀ █▀▀ █▀▀ ─▀─ █▀▀█ ▀▀█▀▀ 
                      ░█▄▄▀ █▀▀ █── █▀▀ ▀█▀ █──█ ──█── 
                      ░█─░█ ▀▀▀ ▀▀▀ ▀▀▀ ▀▀▀ █▀▀▀ ──▀──
                ");

            }
            else if (currentStaff.RoleID == 1)
            {
                Console.WriteLine(@"
                              ▒█▀▀▀ █░█ █▀▀█ █▀▀█ 
                              ▒█▀▀▀ ▄▀▄ █░░█ █░░█ 
                              ▒█▄▄▄ ▀░▀ █▀▀▀ ▀▀▀▀");
            }
            Console.WriteLine("Staff: " + staffBL.GetStaffById(order.StaffID).StaffName + " - ID: " + order.StaffID);
            int dateTimeHandle = 0;
            int ordIDh = 0;
            DateTime currentDateTime = DateTime.Now;
            Console.WriteLine("Order Date Time: " + currentDateTime);
            if (order.PaymentMethod != "")
                Console.WriteLine("Payment Method: " + order.PaymentMethod);
            if (currentStaff.RoleID == 2)
            {
                Line();
                Console.WriteLine("| {0,35} | {1, 10} | {2, 8} | {3, 10} |", "Product", "Quantity", "Size", "Price");
                Line();
                foreach (var item in order.ProductsSize)
                {
                    Console.WriteLine("| {0,35} | {1, 10} | {2, 8} | {3, 10} |", item.Product.ProductName, item.Quantity, item.Size.size, FormatPrice(item.Price));
                }
                Line();
                Console.WriteLine("{0,75}", "Total Price: " + FormatPrice(oBL.CalculateTotalPriceInOrder(order.ProductsSize)));
                Line();

                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine(@"     
                             Ｔｈａｎｋ ｙｏｕ !
                     Share your opinion with us by mail 
                      hungnd.114010122014@vtc.edu.vn");
                Console.ForegroundColor = ConsoleColor.White;
                Line();
            }
            else if (currentStaff.RoleID == 1)
            {
                Console.WriteLine("Order ID: " + order.OrderID);
                Line();
                Console.WriteLine("| {0,35} | {1, 10} | {2, 20} |", "Product", "Quantity", "Size");
                Line();
                foreach (var item in order.ProductsSize)
                {
                    Console.WriteLine("| {0,35} | {1, 10} | {2, 20} |", item.Product.ProductName, item.Quantity, item.Size.size);
                }
                Line();
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine(@"     
                              Ｔｈａｎｋ ｙｏｕ !
                      Share your opinion with us by mail 
                       trungnd.114010122035@vtc.edu.vn");
                Console.ForegroundColor = ConsoleColor.White;
            }
        }
    }

}

