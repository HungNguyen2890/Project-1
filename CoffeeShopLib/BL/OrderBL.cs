using Persistence;
using DAL;

namespace BL
{
    public class OrderBL
    {
        OrderDAL oDAL = new OrderDAL();
        public bool CreateOrder(Order order)
        {


            return oDAL.CreateOrder(order);
        }
        public decimal CalculateTotalPriceInOrder(List<ProductSize> productSizesInOrder)
        {
            decimal sum = 0;
            foreach (ProductSize item in productSizesInOrder)
            {
                sum += item.Quantity * item.Price;
            }
            return sum;
        }
    }
}
