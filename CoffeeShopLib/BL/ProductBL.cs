using Persistence;
using DAL;

namespace BL
{
    public class ProductBL
    {
        ProductDAL pDAL = new ProductDAL();
        public List<Product> GetAllProduct()
        {
            return pDAL.GetAllProduct();
        }
        public List<ProductSize> GetProductSizeByProductID(int productID) {
            return pDAL.GetProductSizesByProductID(productID);
        }
        public Product GetProductByID(int productID) {
            return pDAL.GetProductByID(productID);
        }
        public ProductSize GetProductSizeByProductIDAndSize(int productID, string size) {
            return pDAL.GetProductSizeByProductIDAndSize(productID, size);
        }
    }
}
