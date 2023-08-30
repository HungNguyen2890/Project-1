using UI;
using DAL;
using BL;
using Persistence;

public class Program
{
    static Staff? loginstaff = null;
    public static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Ultilities UI = new Ultilities();
        ProductBL pBL = new ProductBL();
        OrderBL oBL = new OrderBL();
        string[] cashierMenu = { "Create Order", "Orders Pending", "Log Out" };
        string[] bartenderMenu = { "Confirm Order", "Handle Order", "Log Out" };
        while (true)
        {
            string username, pwd;
            int orderID = 0;
            StaffBL uBL = new StaffBL();
            Console.Clear();

            Console.WriteLine(@"

▀▀█▀▀ █░░█ █▀▀   ▒█▀▀█ █▀▀█ █▀▀ █▀▀ █▀▀ █▀▀   ▒█▀▀▀█ █░░█ █▀▀█ █▀▀█ 
░▒█░░ █▀▀█ █▀▀   ▒█░░░ █░░█ █▀▀ █▀▀ █▀▀ █▀▀   ░▀▀▀▄▄ █▀▀█ █░░█ █░░█ 
░▒█░░ ▀░░▀ ▀▀▀   ▒█▄▄█ ▀▀▀▀ ▀░░ ▀░░ ▀▀▀ ▀▀▀   ▒█▄▄▄█ ▀░░▀ ▀▀▀▀ █▀▀▀ 
                                                       Version : 1.0");
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
            if (loginstaff != null)
            {
                if (loginstaff.RoleID == 1)
                {
                    int isConfirm = 0;
                    int ordID;
                    bool active = true;
                    bool activeSub = true;
                    while (active)
                    {
                        List<Order> pendingOrders = new OrderBL().GetOrdersPending();
                        List<Order> confirmedOrders = new OrderBL().GetOrdersConfirmed();
                        List<int> confirmedOrdersID = new List<int>();
                        List<int> pendingOrdersID = new List<int>();
                        int bartenderChoice = UI.MenuHandle(@"
▒█▀▀█ █▀▀█ █▀▀█ ▀▀█▀▀ █▀▀ █▀▀▄ █▀▀▄ █▀▀ █▀▀█ 
▒█▀▀▄ █▄▄█ █▄▄▀ ░░█░░ █▀▀ █░░█ █░░█ █▀▀ █▄▄▀ 
▒█▄▄█ ▀░░▀ ▀░▀▀ ░░▀░░ ▀▀▀ ▀░░▀ ▀▀▀░ ▀▀▀ ▀░▀▀", bartenderMenu, loginstaff);
                        switch (bartenderChoice)
                        {
                            case 1:

                                UI.Title(@"
█▀▀█ █▀▀█ █▀▀▄ █▀▀ █▀▀█ 　 ░▀░ █▀▀ 　 █▀▀█ █░░ █▀▀█ █▀▀ █▀▀ █▀▀▄ 
█░░█ █▄▄▀ █░░█ █▀▀ █▄▄▀ 　 ▀█▀ ▀▀█ 　 █░░█ █░░ █▄▄█ █░░ █▀▀ █░░█ 
▀▀▀▀ ▀░▀▀ ▀▀▀░ ▀▀▀ ▀░▀▀ 　 ▀▀▀ ▀▀▀ 　 █▀▀▀ ▀▀▀ ▀░░▀ ▀▀▀ ▀▀▀ ▀▀▀░", loginstaff);
                                if (pendingOrders.Count() == 0 || pendingOrders == null)
                                {
                                    UI.ShowError("Don't Have Any Order To Confirm");
                                    UI.PressAnyKeyToContinue();
                                    break;
                                }
                                foreach (var item in pendingOrders)
                                {
                                    pendingOrdersID.Add(item.OrderID);
                                }
                                do
                                {
                                    UI.PrintListOrder(pendingOrders);
                                    Console.Write("Enter Order ID: ");
                                    int.TryParse(Console.ReadLine(), out orderID);
                                    Console.Clear();
                                } while (pendingOrdersID.IndexOf(orderID) == -1);
                                UI.PrintOrder(new OrderBL().GetOrderByID(orderID), loginstaff);
                                do
                                {
                                    Console.Write("Enter '1' To Confirm Product Or '2' To Back : ");
                                    int.TryParse(Console.ReadLine(), out isConfirm);
                                    if (isConfirm <= 0 || isConfirm > 2)
                                    {
                                        Console.WriteLine("Invalid Choice");
                                        UI.PressAnyKeyToContinue();
                                    }
                                } while (isConfirm <= 0 || isConfirm > 2);
                                if (isConfirm == 1)
                                {
                                    oBL.ConfirmOrder(orderID);
                                    UI.ShowSuccess("The order has been fulfilled, please receive the goods");
                                    UI.PressAnyKeyToContinue();
                                    break;
                                }
                                else
                                {
                                    // ShowSuccess("Cancel Order Completed");
                                    // UI.PressAnyKeyToContinue();
                                    break;
                                }
                            case 2:
                                UI.Title(@"

▒█▀▀▀█ █▀▀█ █▀▀▄ █▀▀ █▀▀█ 　 █░░█ █▀▀█ █▀▀▄ █▀▀▄ ░▀░ █▀▀▄ █▀▀▀ 　 █▀▀█ ▀█░█▀ █▀▀ █▀▀█ 
▒█░░▒█ █▄▄▀ █░░█ █▀▀ █▄▄▀ 　 █▀▀█ █▄▄█ █░░█ █░░█ ▀█▀ █░░█ █░▀█ 　 █░░█ ░█▄█░ █▀▀ █▄▄▀ 
▒█▄▄▄█ ▀░▀▀ ▀▀▀░ ▀▀▀ ▀░▀▀ 　 ▀░░▀ ▀░░▀ ▀░░▀ ▀▀▀░ ▀▀▀ ▀░░▀ ▀▀▀▀ 　 ▀▀▀▀ ░░▀░░ ▀▀▀ ▀░▀▀", loginstaff);

                                if (confirmedOrders.Count() == 0 || confirmedOrders == null)
                                {
                                    UI.ShowError("Don't Have Any Order To Confirm");
                                    UI.PressAnyKeyToContinue();
                                    break;
                                }
                                foreach (var item in confirmedOrders)
                                {
                                    confirmedOrdersID.Add(item.OrderID);
                                }
                                do
                                {
                                    UI.PrintListOrder(confirmedOrders);
                                    Console.Write("Enter Order ID: ");
                                    int.TryParse(Console.ReadLine(), out orderID);
                                    Console.Clear();
                                } while (confirmedOrdersID.IndexOf(orderID) == -1);
                                UI.PrintOrder(new OrderBL().GetOrderByID(orderID), loginstaff);
                                do
                                {
                                    Console.Write("Enter '1' To Handle Order Or '2' To Back: ");
                                    int.TryParse(Console.ReadLine(), out isConfirm);
                                    if (isConfirm <= 0 || isConfirm > 2)
                                    {
                                        UI.ShowError("Invalid Choice");
                                        UI.PressAnyKeyToContinue();
                                    }
                                } while (isConfirm <= 0 || isConfirm > 2);
                                if (isConfirm == 1)
                                {
                                    oBL.HandleOrder(orderID);
                                    UI.ShowSuccess("Handle Order Completed");
                                    UI.PressAnyKeyToContinue();
                                    break;
                                }
                                else
                                {
                                    UI.ShowSuccess("Cancel Order Completed");
                                    UI.PressAnyKeyToContinue();
                                    break;
                                }

                            case 3:
                                active = false;
                                break;
                        }
                    }
                }
                else if (loginstaff.RoleID == 2)
                {
                    bool active = true;
                    int productID = 0, quantity = 0, isValidSize = 0, addMoreChoice = 0;
                    string size = "";
                    Order order = new Order();
                    List<Product> products = new List<Product>();
                    List<ProductSize> productSizesInOrder = new List<ProductSize>();
                    List<ProductSize> productSizeClone = new List<ProductSize>();
                    ProductSize productSizeInOrder = new ProductSize();
                    string answer;

                    while (active)
                    {
                        List<Order> completedOrders = new OrderBL().GetOrdersCompleted();
                        List<int> completedOrdersID = new List<int>();
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
                                    UI.PrintTimeLine(new string[] { "Choose Product For Order", "Choose Payment Method", "Create Order" }, 1);
                                    do
                                    {
                                        Console.Write("Choose Product By ID: ");
                                        int.TryParse(Console.ReadLine(), out productID);
                                        if (pBL.GetProductByID(productID).Status == 1 || productID <= 0 || productID > pBL.GetAllProduct().Count())
                                        {
                                            if (pBL.GetProductByID(productID).Status == 1)
                                                UI.ShowError("This Product Not For Sale!");
                                            if (productID <= 0 || productID > pBL.GetAllProduct().Count())
                                                UI.ShowError("Invalid Choice!");
                                            UI.PressAnyKeyToContinue();
                                            UI.CreateOrderTitle();
                                            UI.ShowListProduct(pBL.GetAllProduct());
                                            UI.PrintTimeLine(new string[] { "Choose Product For Order", "Choose Payment Method", "Create Order" }, 1);
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
                                                UI.ShowError("This Product Is Out Of Stock!");
                                            if (!(size == "s" || size == "m" || size == "l"))
                                                UI.ShowError("Invalid Choice!");
                                            UI.PressAnyKeyToContinue();
                                            UI.CreateOrderTitle();
                                            UI.ShowListProduct(pBL.GetAllProduct());
                                            UI.PrintTimeLine(new string[] { "Choose Product For Order", "Choose Payment Method", "Create Order" }, 1);
                                            Console.WriteLine("Product ID: " + productID);
                                        }
                                        else
                                        {
                                            productSizeInOrder = pBL.GetProductSizeByProductIDAndSize(productID, size);
                                        }
                                    } while (isValidSize == 0 || !(size == "s" || size == "m" || size == "l"));
                                    bool isContinue;
                                    int isSure;
                                    do
                                    {
                                        isContinue = true;
                                        Console.Write("Enter Quantity: ");
                                        int.TryParse(Console.ReadLine(), out quantity);
                                        if (quantity > 20)
                                        {
                                            do
                                            {
                                                Console.ForegroundColor = ConsoleColor.Red;
                                                Console.Write($"The quantity you choice: {quantity}, Are you sure? \n(Input 1 to continue, Input 2 to re-enter the quantity): ");
                                                int.TryParse(Console.ReadLine(), out isSure);
                                                Console.ForegroundColor = ConsoleColor.White;
                                            } while (isSure <= 0 || isSure > 2);
                                            if (isSure == 1) isContinue = true;
                                            else if (isSure == 2) isContinue = false;
                                        }
                                        if (quantity <= 0)
                                        {
                                            UI.ShowError("Invalid Quantity!");
                                            UI.CreateOrderTitle();
                                            UI.ShowListProduct(pBL.GetAllProduct());
                                            UI.PrintTimeLine(new string[] { "Choose Product For Order", "Choose Payment Method", "Create Order" }, 1);
                                            Console.WriteLine("Product ID: " + productID);
                                            Console.WriteLine("Product Size: {0}", size);
                                        }
                                        else
                                        {
                                            productSizeInOrder.Quantity = quantity;
                                        }
                                    } while (quantity <= 0 || isContinue == false);
                                    // PrintTimeLine(new string[] {"Choose Product For Order", "Choose Payment Method", "Confirm Order"}, 1);
                                    List<int> ProductIDInOrder = new List<int>();
                                    foreach (var item in productSizesInOrder)
                                    {
                                        ProductIDInOrder.Add(item.ProductSizeID);
                                    }

                                    if (ProductIDInOrder.IndexOf(productSizeInOrder.ProductSizeID) != -1)
                                    {
                                        foreach (var item in productSizesInOrder)
                                        {
                                            if (item.ProductSizeID == productSizeInOrder.ProductSizeID)
                                            {
                                                item.Quantity += productSizeInOrder.Quantity;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        productSizesInOrder.Add(productSizeInOrder);
                                    }
                                    do
                                    {

                                        if (addMoreChoice == 3)
                                        {
                                            UI.CreateOrderTitle();
                                            UI.ShowListProduct(pBL.GetAllProduct());
                                        }
                                        do
                                        {
                                            Console.ForegroundColor = ConsoleColor.DarkCyan;
                                            Console.Write("Input '1' To Add More, '2' To Continue Creating Order, '3' To Show Selected Product, '4' To Back Main Menu: ");
                                            Console.ForegroundColor = ConsoleColor.White;
                                            int.TryParse(Console.ReadLine(), out addMoreChoice);

                                            if (addMoreChoice <= 0 || addMoreChoice > 4)
                                            {
                                                UI.ShowError("Invalid Choice!");
                                                UI.CreateOrderTitle();
                                                UI.ShowListProduct(pBL.GetAllProduct());
                                            }
                                        } while (addMoreChoice <= 0 || addMoreChoice > 4);

                                        if (addMoreChoice == 2) break;
                                        if (addMoreChoice == 3)
                                        {
                                            Console.Clear();
                                            UI.Title(@"
█▀▀ █▀▀ █░░ █▀▀ █▀▀ ▀▀█▀▀ █▀▀ █▀▀▄ 　 █▀▀█ █▀▀█ █▀▀█ █▀▀▄ █░░█ █▀▀ ▀▀█▀▀ 
▀▀█ █▀▀ █░░ █▀▀ █░░ ░░█░░ █▀▀ █░░█ 　 █░░█ █▄▄▀ █░░█ █░░█ █░░█ █░░ ░░█░░ 
▀▀▀ ▀▀▀ ▀▀▀ ▀▀▀ ▀▀▀ ░░▀░░ ▀▀▀ ▀▀▀░ 　 █▀▀▀ ▀░▀▀ ▀▀▀▀ ▀▀▀░ ░▀▀▀ ▀▀▀ ░░▀░░", loginstaff);



                                            UI.Line();
                                            Console.WriteLine("| {0,19} | {1, 10} | {2, 8} | {3, 10} |", "Product", "Quantity", "Size", "Price");
                                            UI.Line();
                                            foreach (var item in productSizesInOrder)
                                            {
                                                Console.WriteLine("| {0,19} | {1, 10} | {2, 8} | {3, 10} |", item.Product.ProductName, item.Quantity, item.Size.size, UI.FormatPrice(item.Price));
                                            }
                                            UI.Line();
                                            UI.PressAnyKeyToContinue();
                                        }
                                    } while (addMoreChoice == 3);
                                    if (addMoreChoice == 4) break;
                                } while (addMoreChoice == 1);
                                if (addMoreChoice == 4)
                                {
                                    productSizesInOrder = new List<ProductSize>();
                                    break;
                                }
                                order.CreateBy = loginstaff;
                                order.ProductsSize = productSizesInOrder;
                                int pm = 0;
                                do
                                {
                                    UI.PrintTimeLine(new string[] { "Choose Product For Order", "Choose Payment Method", "Create Order" }, 2);
                                    Console.WriteLine(" Choose Payment Method");
                                    Console.WriteLine(" 1. Banking");
                                    Console.WriteLine(" 2. Cash");
                                    Console.Write(" Your Choice: ");
                                    int.TryParse(Console.ReadLine(), out pm);
                                    if (pm <= 0 || pm > 2)
                                    {
                                        Console.WriteLine("Invalid Choice!");
                                        UI.PressAnyKeyToContinue();
                                    }
                                } while (pm <= 0 || pm > 2);
                                if (pm == 1)
                                {
                                    order.PaymentMethod = "Banking";
                                }
                                else if (pm == 2)
                                {
                                    order.PaymentMethod = "Cash";
                                }
                                productSizesInOrder = new List<ProductSize>();

                                int isCreateOrder;
                                do
                                {
                                    order.StaffID = loginstaff.StaffID;
                                    UI.PrintTimeLine(new string[] { "Choose Product For Order", "Choose Payment Method", "Create Order" }, 3);
                                    UI.PrintOrder(order, loginstaff);
                                    Console.Write("Enter '1' To Create Order Or '2' To Cancel Order: ");
                                    int.TryParse(Console.ReadLine(), out isCreateOrder);
                                    if (isCreateOrder <= 0 || isCreateOrder > 2)
                                    {
                                        Console.WriteLine("Invalid Choice!");
                                        UI.PressAnyKeyToContinue();
                                    }
                                } while (isCreateOrder <= 0 || isCreateOrder > 2);
                                if (isCreateOrder == 1)
                                {
                                    if (oBL.CreateOrder(order))
                                    {
                                        UI.ShowSuccess("Receipt being printed!\nOrder Has Been Sent To The Bartender");
                                    }
                                    else
                                    {
                                        UI.ShowError("Create Order Failed");
                                    }
                                }
                                else
                                {
                                    UI.ShowSuccess("Cancel Order Completed");
                                }
                                UI.PressAnyKeyToContinue();
                                productSizesInOrder = new List<ProductSize>();
                                break;

                            case 2:
                                UI.Title(@"
▒█▀▀▀█ █▀▀█ █▀▀▄ █▀▀ █▀▀█ 　 ▒█░░▒█ █▀▀█ ░▀░ ▀▀█▀▀ ░▀░ █▀▀▄ █▀▀▀ 
▒█░░▒█ █▄▄▀ █░░█ █▀▀ █▄▄▀ 　 ▒█▒█▒█ █▄▄█ ▀█▀ ░░█░░ ▀█▀ █░░█ █░▀█ 
▒█▄▄▄█ ▀░▀▀ ▀▀▀░ ▀▀▀ ▀░▀▀ 　 ▒█▄▀▄█ ▀░░▀ ▀▀▀ ░░▀░░ ▀▀▀ ▀░░▀ ▀▀▀▀", loginstaff);
                                int isConfirm = 0;
                                int ordID;

                                while (active)
                                {
                                    List<Order> pendingOrders = new OrderBL().GetOrdersPending();

                                    List<int> pendingOrdersID = new List<int>();
                                    if (pendingOrders.Count() == 0 || pendingOrders == null)
                                    {
                                        UI.ShowError("Don't Have Any Order To Confirm");
                                        UI.PressAnyKeyToContinue();
                                        break;
                                    }
                                    foreach (var item in pendingOrders)
                                    {
                                        pendingOrdersID.Add(item.OrderID);
                                    }
                                    do
                                    {
                                        UI.PrintListOrder(pendingOrders);
                                        Console.Write("Enter Order ID: ");
                                        int.TryParse(Console.ReadLine(), out orderID);
                                        if (pendingOrdersID.IndexOf(orderID) == -1)
                                            UI.ShowError("Invalid Order ID");
                                        Console.Clear();
                                    } while (pendingOrdersID.IndexOf(orderID) == -1);
                                    UI.PrintOrder(new OrderBL().GetOrderByID(orderID), loginstaff);
                                    UI.PressAnyKeyToContinue();
                                    break;
                                }
                                break;
                            case 3:
                                active = false;
                                break;

                        }
                    }
                }
            }
            else
            {
                UI.ShowError("\nInvalid User Name Or Password!");
                UI.PressAnyKeyToContinue();
                Main();
            }
        }
    }
}
