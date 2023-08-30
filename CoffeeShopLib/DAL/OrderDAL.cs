using MySqlConnector;
using Persistence;

namespace DAL
{
    static class OrderFilter
    {
        public const int CHANGE_STATUS_TO_CONFIRMED = 1;
        public const int CHANGE_STATUS_TO_COMPLETED = 2;
        public const int GET_ORDERS_PENDING = 3;
        public const int GET_ORDERS_CONFIRMED = 4;
        public const int GET_ORDERS_COMPLETED = 5;
    }
    public class OrderDAL
    {
        public Order GetOrder(MySqlDataReader reader)
        {
            Order order = new Order();
            order.OrderID = reader.GetInt32("order_id");
            order.PaymentMethod = reader.GetString("paymentmethod");
            order.Status = (reader.GetInt32("status") == 0) ? "Pending" : (reader.GetInt32("status") == 1) ? "Confirm" : "Completed";
            order.ProductsSize = new List<ProductSize>();
            order.CreateAt = reader.GetDateTime("create_at");
            order.StaffID = reader.GetInt32("create_by");
            return order;
        }
        public Order GetOrderByID(int orderID)
        {
            Order order = new Order();
            try
            {
                MySqlCommand command = new MySqlCommand("", connection);
                command.CommandText = $@"SELECT * FROM Orders WHERE order_id = '{orderID}';";
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    order = GetOrder(reader);
                }
                reader.Close();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            order.ProductsSize = GetProductSizesByOrderID(order.OrderID);
            return order;
        }
        public List<Order> GetOrders(int orderFilter)
        {
            string query = "";
            List<Order> orders = new List<Order>();
            try
            {
                switch (orderFilter)
                {
                    case OrderFilter.GET_ORDERS_PENDING:
                        query = @"SELECT * FROM Orders WHERE status = 0;";
                        break;
                    case OrderFilter.GET_ORDERS_CONFIRMED:
                        query = @"SELECT * FROM Orders WHERE status = 1;";
                        break;
                    case OrderFilter.GET_ORDERS_COMPLETED:
                        query = @"SELECT * FROM Orders WHERE status = 2;";
                        break;
                }
                MySqlCommand command = new MySqlCommand(query, connection);
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    orders.Add(GetOrder(reader));
                }
                reader.Close();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            return orders;
        }
        public List<ProductSize> GetProductSizesByOrderID(int orderID)
        {
            ProductDAL pDAL = new ProductDAL();
            List<ProductSize> productSizes = new List<ProductSize>();
            try
            {
                MySqlCommand command = new MySqlCommand("", connection);
                command.CommandText = $@"SELECT * FROM Orders O
                INNER JOIN OrderDetails OD ON O.order_id = OD.order_id
                INNER JOIN productsizes PS ON PS.product_size_id = OD.product_size_id
                INNER JOIN sizes S ON S.size_id = PS.size_id
                INNER JOIN products P ON P.product_id = PS.product_id
                WHERE O.order_id = @orderid;";
                command.Parameters.AddWithValue("@orderid", orderID);
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    productSizes.Add(pDAL.GetProductSizeHaveQuantity(reader));
                }
                reader.Close();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            foreach (var item in productSizes)
            {
                item.Size = pDAL.GetSizeByID(item.SizeID);
                item.Product = pDAL.GetProductByID(item.ProductID);
            }
            return productSizes;
        }
        public bool UpdateOrder(int orderID, int orderFilter)
        {
            ProductSize product = new ProductSize();
            try
            {
                MySqlCommand command = new MySqlCommand("", connection);
                switch (orderFilter)
                {
                    case OrderFilter.CHANGE_STATUS_TO_CONFIRMED:
                        command.CommandText = @"UPDATE Orders SET status = 1 WHERE Order_id = @orderid;";
                        break;
                    case OrderFilter.CHANGE_STATUS_TO_COMPLETED:
                        command.CommandText = @"UPDATE Orders SET status = 2 WHERE Order_id = @orderid;";
                        break;
                }
                command.Parameters.AddWithValue("@orderid", orderID);
                MySqlDataReader reader = command.ExecuteReader();
                reader.Close();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            return true;
        }
        private MySqlConnection connection = DbConfig.GetConnection();
       
        public bool CreateOrder(Order order)
        {
            if (order == null || order.ProductsSize == null || order.ProductsSize.Count() == 0)
            {
                return false;
            }
            bool result = false;
            try
            {
                using (MySqlTransaction trans = connection.BeginTransaction())
                using (MySqlCommand cmd = connection.CreateCommand())
                {
                    cmd.Connection = connection;
                    cmd.Transaction = trans;
                    //Lock update all tables
                    cmd.CommandText = "lock tables Orders write, Products write, OrderDetails write;";
                    cmd.ExecuteNonQuery();

                    MySqlDataReader? reader = null;

                    try
                    {
                        //Insert Order
                        cmd.CommandText = "insert into Orders(create_by, paymentmethod) values (@createby, @paymentmethod);";
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@createby", order.CreateBy.StaffID);
                        cmd.Parameters.AddWithValue("@paymentmethod", order.PaymentMethod);
                        cmd.ExecuteNonQuery();
                        //get new Order_ID
                        cmd.CommandText = "select LAST_INSERT_ID() as order_id";
                        reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            order.OrderID = reader.GetInt32("order_id");
                        }
                        reader.Close();

                        foreach (ProductSize item in order.ProductsSize)
                        {
                            if (item.ProductSizeID == 0 || item.ProductSizeStatus == 1)
                            {
                                throw new Exception("Not Exists Item");
                            }

                            cmd.CommandText = @"insert into OrderDetails(order_id, product_size_id, quantity) values 
                            (" + order.OrderID + ", " + item.ProductSizeID + ", " + item.Quantity + ");";
                            cmd.ExecuteNonQuery();
                        }
                        //commit transaction
                        trans.Commit();
                        result = true;
                    }
                    catch
                    {
                        try
                        {
                            trans.Rollback();
                        }
                        catch { }
                    }
                    finally
                    {
                        //unlock all tables;
                        cmd.CommandText = "unlock tables;";
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: {ex.Message}");
            }


            return result;
        }

    }
}