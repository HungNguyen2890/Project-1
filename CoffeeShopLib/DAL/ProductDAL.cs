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
            product.ProductSizes = new List<ProductSize>();
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
            product.ProductSizes = GetProductSizesByProductID(product.ProductID);
            foreach (ProductSize ps in product.ProductSizes)
            {
                ps.Size = GetSizeByID(ps.SizeID);
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
            foreach (Product p in products)
            {
                p.ProductSizes = GetProductSizesByProductID(p.ProductID);
                foreach (ProductSize ps in p.ProductSizes)
                {
                    ps.Size = GetSizeByID(ps.SizeID);
                }
            }
            return products;
        }
        public ProductSize GetProductSizeByProductIDAndSizeID(int productID, int sizeID)
        {
            ProductSize product = new ProductSize();
            try
            {
                MySqlCommand command = new MySqlCommand("", connection);
                query = @"SELECT * FROM productsizes WHERE Product_ID = @productid AND size_id = @sizeid;";
                command.CommandText = query;
                command.Parameters.AddWithValue("@productid", productID);
                command.Parameters.AddWithValue("@sizeid", sizeID);
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