using Persistence;
using System.Globalization; // thu vien format tien 
using BL;
namespace UI
{
    public class Ultilities
    {
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
            Console.WriteLine("Ｔｈｅ ＣｏｆｆｅｅＳｈｏｐ           🆅 🅴 🆁 0.1");
            Console.ForegroundColor = ConsoleColor.White;
            System.Console.WriteLine(title + " ");
            Console.WriteLine("\n 𝑆𝑡𝑎𝑓𝑓 : " + staff.StaffName);
            Line();
        }
        public void Line()
        {
            System.Console.WriteLine("-===-===-===-===-===-===-===-===-===-===-===-===-===-===-===-===-");
        }
        public void PressAnyKeyToContinue()
        {
            Console.Write("Press Any Key to continue...");
            Console.ReadKey();
            Console.Clear();
        }
        public void CreateOrderTitle()

        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("Ｔｈｅ ＣｏｆｆｅｅＳｈｏｐ           🆅 🅴 🆁 0.1");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(@"
                                                                    ▒█▀▀█ █▀▀█ █▀▀ █▀▀ █▀▀ █▀▀ ▒█▀▀▀█ █░░█ █▀▀█ █▀▀█ 　 ▒█▀▄▀█ █▀▀ █▀▀▄ █░░█ 
                                                                    ▒█░░░ █░░█ █▀▀ █▀▀ █▀▀ █▀▀ ░▀▀▀▄▄ █▀▀█ █░░█ █░░█ 　 ▒█▒█▒█ █▀▀ █░░█ █░░█ 
                                                                    ▒█▄▄█ ▀▀▀▀ ▀░░ ▀░░ ▀▀▀ ▀▀▀ ▒█▄▄▄█ ▀░░▀ ▀▀▀▀ █▀▀▀ 　 ▒█░░▒█ ▀▀▀ ▀░░▀ ░▀▀▀");

            Console.WriteLine("-===-===-===-===-===-===-===-===-===-===-===-===-===-===-===-===--===-===-===-===-===-===-===-===-===-===-===-===-===-===-===-===--===-===-===-===-===-===-===-===-===-===-===-===-===-===-===-===-===-===-===-");
            Console.WriteLine("| {0, 10} | {1, 35} | {2, 35} | {3, 18} | {4, 88} |", "Product ID", "Product Name", "Product Description", "Product Status", "Size - Status - Price");
            Console.WriteLine("-===-===-===-===-===-===-===-===-===-===-===-===-===-===-===-===--===-===-===-===-===-===-===-===-===-===-===-===-===-===-===-===--===-===-===-===-===-===-===-===-===-===-===-===-===-===-===-===-===-===-===-");

        }
        public void ShowListProduct(List<Product> products)
        {
            ProductBL productBL = new ProductBL();
            CultureInfo cultureInfo = new CultureInfo("vi-VN");
            foreach (Product item in products)
            {
                Console.Write("| {0, 10} | {1, 35} | {2, 35} | {3, 15} |", item.ProductID, item.ProductName, item.Description, (item.Status == 0) ? "Available For Sale" : "Not For Sale");
                foreach (ProductSize ps in productBL.GetProductSizeByProductID(item.ProductID))
                {
                    Console.Write(" {0, 2} - {1, 10} - {2, 10} |", ps.Size.size.ToUpper(), (ps.ProductSizeStatus == 0) ? "In Stock" : ((ps.ProductSizeStatus == 1) ? "Out Of Stock" : ""), string.Format(cultureInfo, "{0:N0} VND", ps.Price));
                }
                Console.Write("\n");
            }
            Console.WriteLine("-===-===-===-===-===-===-===-===-===-===-===-===-===-===-===-===--===-===-===-===-===-===-===-===-===-===-===-===-===-===-===-===--===-===-===-===-===-===-===-===-===-===-===-===-===-===-===-===-===-===-===-");
        }
    }





}

