using UI;
using DAL;
using BL;
using Persistence;

public class Program
{
    static Staff loginstaff = new Staff();
    public static void Main()
    {
        Ultilities UI = new Ultilities();
        ProductBL pBL = new ProductBL();
        string[] cashierMenu = { "Create Order", "Show Completed Order", "Log Out" };
        string[] bartenderMenu = { "Unprocessed Orders", "Processing Orders", "Log Out" };
        string[] cashierSubMenu = { "Add Product To Order", "Show Order", "Back To Main Menu" };

        string username, pwd;
        StaffBL uBL = new StaffBL();
        Console.Clear();
        Console.WriteLine(@"

▀▀█▀▀ █░░█ █▀▀   ▒█▀▀█ █▀▀█ █▀▀ █▀▀ █▀▀ █▀▀   ▒█▀▀▀█ █░░█ █▀▀█ █▀▀█ 
░▒█░░ █▀▀█ █▀▀   ▒█░░░ █░░█ █▀▀ █▀▀ █▀▀ █▀▀   ░▀▀▀▄▄ █▀▀█ █░░█ █░░█ 
░▒█░░ ▀░░▀ ▀▀▀   ▒█▄▄█ ▀▀▀▀ ▀░░ ▀░░ ▀▀▀ ▀▀▀   ▒█▄▄▄█ ▀░░▀ ▀▀▀▀ █▀▀▀ 
                                                       🆅 🅴 🆁 : 0.1");
        UI.Line();
        Console.Write("Username: ");
        username = Console.ReadLine();
        Console.Write("Password: ");
        var pass = string.Empty;
        ConsoleKey key;
        do
        {
            var keyInfo = Console.ReadKey(intercept: true);
            key = keyInfo.Key;

            if (key == ConsoleKey.Backspace && pass.Length > 0)
            {
                Console.Write("\b \b");
                pass = pass[0..^1];
            }
            else if (!char.IsControl(keyInfo.KeyChar))
            {
                Console.Write("*");
                pass += keyInfo.KeyChar;
            }
        } while (key != ConsoleKey.Enter);

        loginstaff = uBL.Authorize(username, pass);
        if (loginstaff.RoleID == 1)
        {
            string[] unprocessedAction = { "Change Status To Processing", "Back To Previous Menu" };
            string[] processingAction = { "Change Status To Completed", "Back To Previous Menu" };
            int ordID;
            bool active = true;
            bool activeSub = true;
            while (active)
            {
                int bartenderChoice = UI.MenuHandle(@"
▒█▀▀█ █▀▀█ █▀▀█ ▀▀█▀▀ █▀▀ █▀▀▄ █▀▀▄ █▀▀ █▀▀█ 
▒█▀▀▄ █▄▄█ █▄▄▀ ░░█░░ █▀▀ █░░█ █░░█ █▀▀ █▄▄▀ 
▒█▄▄█ ▀░░▀ ▀░▀▀ ░░▀░░ ▀▀▀ ▀░░▀ ▀▀▀░ ▀▀▀ ▀░▀▀", bartenderMenu, loginstaff);
                switch (bartenderChoice)
                {
                    case 1:

                        break;
                    case 2:
                        break;
                    case 3:
                        active = false;
                        break;
                }
            }
        }
        else if (loginstaff.RoleID == 2)
        {
            OrderBL oBL = new OrderBL();
            bool active = true, activeSub = true, isAddMore = false;
            int productID = 0, quantity = 0, isValidSize = 0, addMoreChoice = 0;
            string size = "";
            Order order = new Order();
            List<Product> products = new List<Product>();
            List<ProductSize> productSizesInOrder = new List<ProductSize>();
            ProductSize productSizeInOrder = new ProductSize();
            string answer;

            while (active)
            {
                int cashierChoice = UI.MenuHandle(@"
▒█▀▀█ █▀▀█ █▀▀ █░░█ ░▀░ █▀▀ █▀▀█ 
▒█░░░ █▄▄█ ▀▀█ █▀▀█ ▀█▀ █▀▀ █▄▄▀ 
▒█▄▄█ ▀░░▀ ▀▀▀ ▀░░▀ ▀▀▀ ▀▀▀ ▀░▀▀", cashierMenu, loginstaff);
                switch (cashierChoice)
                {
                    case 1:
                        do
                        {

                            UI.CreateOrderTitle();
                            UI.ShowListProduct(pBL.GetAllProduct());
                            do
                            {
                                Console.Write("Choose Product By ID: ");
                                int.TryParse(Console.ReadLine(), out productID);
                                if (pBL.GetProductByID(productID).Status == 1 || productID <= 0 || productID > pBL.GetAllProduct().Count())
                                {
                                    if (pBL.GetProductByID(productID).Status == 1)
                                        Console.WriteLine("This Product Not For Sale!");
                                    if (productID <= 0 || productID > pBL.GetAllProduct().Count())
                                        Console.WriteLine("Invalid Choice!");
                                    UI.PressAnyKeyToContinue();
                                    UI.CreateOrderTitle();
                                    UI.ShowListProduct(pBL.GetAllProduct());
                                }
                                else
                                {
                                    products.Add(pBL.GetProductByID(productID));
                                }
                            } while (pBL.GetProductByID(productID).Status == 1 || productID <= 0 || productID > pBL.GetAllProduct().Count());
                            do
                            {
                                isValidSize = 1;
                                Console.Write("Enter Product Size S/M/L: ");
                                size = (Console.ReadLine() ?? "").ToLower();

                                foreach (ProductSize item in pBL.GetProductSizeByProductID(productID))
                                {
                                    if (item.ProductSizeStatus == 1)
                                    {
                                        isValidSize = 0;
                                    }
                                }
                                if (isValidSize == 0 || !(size == "s" || size == "m" || size == "l"))
                                {
                                    if (isValidSize == 0)
                                        Console.WriteLine("This Product Is Out Of Stock!");
                                    if (!(size == "s" || size == "m" || size == "l"))
                                        Console.WriteLine("Invalid Choice!");
                                    UI.PressAnyKeyToContinue();
                                    UI.CreateOrderTitle();
                                    UI.ShowListProduct(pBL.GetAllProduct());
                                    Console.WriteLine("Product ID: " + productID);
                                }
                                else
                                {
                                    productSizeInOrder = pBL.GetProductSizeByProductIDAndSizeID(productID, size);
                                }
                            } while (isValidSize == 0 || !(size == "s" || size == "m" || size == "l"));
                            do
                            {
                                Console.Write("Enter Quantity: ");
                                int.TryParse(Console.ReadLine(), out quantity);
                                if (quantity <= 0)
                                {
                                    Console.WriteLine("Invalid Quantity!");
                                    UI.CreateOrderTitle();
                                    UI.ShowListProduct(pBL.GetAllProduct());
                                    Console.WriteLine("Product ID: " + productID);
                                    Console.WriteLine("Product Size: {0}", size);
                                }
                                else
                                {
                                    productSizeInOrder.Quantity = quantity;
                                }
                            } while (quantity <= 0);
                            do
                            {
                                Console.Write("Press '1' To Add More, '2' To Create Order: ");
                                int.TryParse(Console.ReadLine(), out addMoreChoice);


                                if (addMoreChoice <= 0 || addMoreChoice > 2)
                                {
                                    Console.WriteLine("Invalid Choice!");
                                    UI.CreateOrderTitle();
                                    UI.ShowListProduct(pBL.GetAllProduct());
                                    Console.WriteLine("Product ID: " + productID);
                                    Console.WriteLine("Product Size: {0}", size);
                                    Console.WriteLine("Quantity: " + quantity);
                                }
                            } while (addMoreChoice <= 0 || addMoreChoice > 2);
                            productSizesInOrder.Add(productSizeInOrder);
                        } while (addMoreChoice == 1);
                        order.CreateBy = loginstaff;
                        order.ProductsSize = productSizesInOrder;
                        if (addMoreChoice == 2)
                        {
                            Console.Clear();
                            Console.WriteLine(@"

▀▀█▀▀ █░░█ █▀▀   ▒█▀▀█ █▀▀█ █▀▀ █▀▀ █▀▀ █▀▀   ▒█▀▀▀█ █░░█ █▀▀█ █▀▀█ 
░▒█░░ █▀▀█ █▀▀   ▒█░░░ █░░█ █▀▀ █▀▀ █▀▀ █▀▀   ░▀▀▀▄▄ █▀▀█ █░░█ █░░█ 
░▒█░░ ▀░░▀ ▀▀▀   ▒█▄▄█ ▀▀▀▀ ▀░░ ▀░░ ▀▀▀ ▀▀▀   ▒█▄▄▄█ ▀░░▀ ▀▀▀▀ █▀▀▀ 
                                                       🆅 🅴 🆁 : 0.1");

                            Console.WriteLine(@"{0,20}", @"
                  
                ░█▀▀█ █▀▀ █▀▀ █▀▀ ─▀─ █▀▀█ ▀▀█▀▀ 
                ░█▄▄▀ █▀▀ █── █▀▀ ▀█▀ █──█ ──█── 
                ░█─░█ ▀▀▀ ▀▀▀ ▀▀▀ ▀▀▀ █▀▀▀ ──▀──
                ");

                            UI.Line();
                            Console.WriteLine("ADD: 18 Tam Trinh- Hai Ba Trung- Ha Noi");
                            Console.WriteLine("Phone: 0852394222 - 0969055545");
                            int dateTimeHandle = 0;
                            int ordIDh = 0;
                            DateTime currentDateTime = DateTime.Now;
                            Console.WriteLine("Order Date Time: " + currentDateTime);
                            UI.Line();
                            Console.WriteLine("| {0,19} | {1, 10} | {2, 10} | {3, 10} |", "Product", "Quantity", "Size", "Price");
                            foreach (var item in productSizesInOrder)
                            {
                                Console.WriteLine("| {0,19} | {1, 10} | {2, 10} | {3, 10} |", item.Product.ProductName, item.Quantity, item.Size.size, item.Price);
                            }
                            UI.Line();
                            Console.WriteLine("{0,64}", "Total Price: " + oBL.CalculateTotalPriceInOrder(productSizesInOrder) + " VND");
                            UI.Line();
                            Console.WriteLine("\n\t \t🌸 THANK YOU, SEE YOU AGAIN 🌸");
                        }
                        // Console.WriteLine(oBL.CreateOrder(order) ? "Create Order Completed" : "Create Order Failed");
                        Console.WriteLine(" Create Order Success");
                        UI.PressAnyKeyToContinue();
                        productSizesInOrder = new List<ProductSize>();

                        break;
                    case 2:
                        break;
                    case 3:
                        active = false;
                        break;

                }
            }
        }
        else
        {
            Console.WriteLine("Invalid User Name Or Password!");
            Main();
        }
        return;

    }
}