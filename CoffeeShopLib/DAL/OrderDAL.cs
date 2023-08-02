using MySqlConnector;
using Persistence;

namespace DAL
{
    public class OrderDAL
    {
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
                        cmd.CommandText = "insert into Orders(create_by) values (@createby);";
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@createby", order.CreateBy.StaffID);
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