﻿using UI;
using DAL;
using BL;
using Persistence;

public class Program
{
    static Staff? loginstaff = null;
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
        if (loginstaff != null)
        {

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
                List<ProductSize> productSizeClone = new List<ProductSize>();
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
                                            ShowError("This Product Not For Sale!");
                                        if (productID <= 0 || productID > pBL.GetAllProduct().Count())
                                            ShowError("Invalid Choice!");
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
                                            ShowError("This Product Is Out Of Stock!");
                                        if (!(size == "s" || size == "m" || size == "l"))
                                            ShowError("Invalid Choice!");
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
                                        ShowError("Invalid Quantity!");
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
                                productSizesInOrder.Add(productSizeInOrder);
                                do
                                {
                                    if (addMoreChoice == 3)
                                        UI.ShowListProduct(pBL.GetAllProduct());
                                    do
                                    {
                                        Console.ForegroundColor = ConsoleColor.DarkCyan;
                                        Console.WriteLine("Press '1' To Add More, '2' To Create Order, '3' To Show Select Product: ");
                                        Console.ForegroundColor = ConsoleColor.White;
                                        int.TryParse(Console.ReadLine(), out addMoreChoice);


                                        if (addMoreChoice <= 0 || addMoreChoice > 3)
                                        {
                                            ShowError("Invalid Choice!");
                                            UI.CreateOrderTitle();
                                            UI.ShowListProduct(pBL.GetAllProduct());
                                            Console.WriteLine("Product ID: " + productID);
                                            Console.WriteLine("Product Size: {0}", size);
                                            Console.WriteLine("Quantity: " + quantity);
                                        }
                                    } while (addMoreChoice <= 0 || addMoreChoice > 3);

                                    if (addMoreChoice == 2) break;
                                    if (addMoreChoice == 3)
                                    {
                                        Console.Clear();
                                       Console.WriteLine (@"
█▀▀ █▀▀ █░░ █▀▀ █▀▀ ▀▀█▀▀ █▀▀ █▀▀▄ 　 █▀▀█ █▀▀█ █▀▀█ █▀▀▄ █░░█ █▀▀ ▀▀█▀▀ 
▀▀█ █▀▀ █░░ █▀▀ █░░ ░░█░░ █▀▀ █░░█ 　 █░░█ █▄▄▀ █░░█ █░░█ █░░█ █░░ ░░█░░ 
▀▀▀ ▀▀▀ ▀▀▀ ▀▀▀ ▀▀▀ ░░▀░░ ▀▀▀ ▀▀▀░ 　 █▀▀▀ ▀░▀▀ ▀▀▀▀ ▀▀▀░ ░▀▀▀ ▀▀▀ ░░▀░░",loginstaff);
                            


                                        UI.Line();
                                        Console.WriteLine("| {0,19} | {1, 10} | {2, 10} | {3, 10} |", "Product", "Quantity", "Size", "Price");
                                        UI.Line();
                                        foreach (var item in productSizesInOrder)
                                        {
                                            Console.WriteLine("| {0,19} | {1, 10} | {2, 10} | {3, 10} |", item.Product.ProductName, item.Quantity, item.Size.size, item.Price);
                                        }
                                        UI.Line();
                                        UI.PressAnyKeyToContinue();
                                    }
                                } while (addMoreChoice == 3);
                            } while (addMoreChoice == 1);
                            order.CreateBy = loginstaff;
                            order.ProductsSize = productSizesInOrder;
                            
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
                            UI.Line();
                            foreach (var item in productSizesInOrder)
                            {
                                Console.WriteLine("| {0,19} | {1, 10} | {2, 10} | {3, 10} |", item.Product.ProductName, item.Quantity, item.Size.size, item.Price);
                            }
                            UI.Line();
                            Console.WriteLine("{0,64}", "Total Price: " + oBL.CalculateTotalPriceInOrder(productSizesInOrder) + " VND");
                            UI.Line();
                            Console.WriteLine("\n\t \t🌸 THANK YOU, SEE YOU AGAIN 🌸");
                            if (oBL.CreateOrder(order))
                            {
                                ShowSuccess("Create Order Completed");
                                ShowSuccess("Order has been sent to the bartender");
                            }
                            else
                            {
                                ShowError("Create Order Failed");
                            };
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
        }
        else
        {
            ShowError("\nInvalid User Name Or Password!");
            UI.PressAnyKeyToContinue();
            Main();
        }
        return;
    }
    static void ShowSuccess(string msg)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine(msg + "!");
        Console.ForegroundColor = ConsoleColor.White;
    }
    static void ShowError(string msg)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(msg + "!");
        Console.ForegroundColor = ConsoleColor.White;
    }

}