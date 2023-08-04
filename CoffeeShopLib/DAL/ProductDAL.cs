using System.Security.Cryptography;
using System.Text;
using MySqlConnector;
using Persistence;

namespace DAL
{
    public class ProductDAL
    {
        private string query = "";
        private MySqlConnection connection = DbConfig.GetConnection();

        internal Product GetProduct(MySqlDataReader reader)
        {
            Product product = new Product();
            product.ProductID = reader.GetInt32("Product_ID");
            product.ProductName = reader.GetString("Product_Name");
            product.Status = reader.GetInt32("Product_Status");
            product.Description = reader.GetString("Description");
            return product;
        }
        internal Size GetSize(MySqlDataReader reader)
        {
            Size size = new Size();
            size.SizeId = reader.GetInt32("size_id");
            size.size = reader.GetString("size");
            return size;
        }
        internal ProductSize GetProductSize(MySqlDataReader reader)
        {
            ProductSize productSize = new ProductSize();
            productSize.ProductSizeID = reader.GetInt32("product_size_id");
            productSize.ProductID = reader.GetInt32("product_id");
            productSize.Product = new Product();
            productSize.Size = new Size();
            productSize.SizeID = reader.GetInt32("size_id");
            productSize.ProductSizeStatus = reader.GetInt32("Product_Size_Status");
            productSize.Price = reader.GetDecimal("Price");
            return productSize;
        }
        public Product GetProductByID(int productID)
        {
            Product product = new Product();
            try
            {
                MySqlCommand command = new MySqlCommand("", connection);
                query = @"SELECT * FROM products WHERE Product_ID = @productid;";
                command.CommandText = query;
                command.Parameters.AddWithValue("@productid", productID);
                MySqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    product = GetProduct(reader);
                }
                reader.Close();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            return product;
        }
        public List<Product> GetAllProduct()
        {
            List<Product> products = new List<Product>();
            try
            {
                MySqlCommand command = new MySqlCommand("", connection);
                query = @"SELECT * FROM products;";
                command.CommandText = query;
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    products.Add(GetProduct(reader));
                }
                reader.Close();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            return products;
        }
        public ProductSize GetProductSizeByProductIDAndSizeID(int productID, string size)
        {
            ProductSize product = new ProductSize();
            try
            {
                MySqlCommand command = new MySqlCommand("", connection);
                query = @"SELECT * FROM productsizes PS
                INNER JOIN sizes S ON S.size_id = PS.size_id
                WHERE Product_ID = @productid AND S.size = @size;";
                command.CommandText = query;
                command.Parameters.AddWithValue("@productid", productID);
                command.Parameters.AddWithValue("@size", size);
                MySqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    product = GetProductSize(reader);
                }
                reader.Close();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            product.Size = GetSizeByID(product.SizeID);
            product.Product = GetProductByID(product.ProductID);
            return product;
        }
        public List<ProductSize> GetProductSizesByProductID(int productID)
        {
            List<ProductSize> productsSize = new List<ProductSize>();
            try
            {
                MySqlCommand command = new MySqlCommand("", connection);
                query = @"SELECT * FROM productsizes WHERE product_id = @productid;";
                command.CommandText = query;
                command.Parameters.AddWithValue("@productid", productID);
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    productsSize.Add(GetProductSize(reader));
                }
                reader.Close();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
            }

            if (productsSize.Count() != 0)
            {
                foreach (ProductSize item in productsSize)
                {
                    item.Size = GetSizeByID(item.SizeID);
                    item.Product = GetProductByID(item.ProductID);
                }
            }
            return productsSize;
        }
        public Size GetSizeByID(int sizeID)
        {
            Size size = new Size();
            try
            {
                MySqlCommand command = new MySqlCommand("", connection);
                query = @"SELECT * FROM sizes WHERE size_id = @sizeid;";
                command.CommandText = query;
                command.Parameters.AddWithValue("@sizeid", sizeID);
                MySqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    size = GetSize(reader);
                }
                reader.Close();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            return size;
        }
    }
}