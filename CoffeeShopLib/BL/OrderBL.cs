using Persistence;
using DAL;

namespace BL
{
    public class OrderBL
    {
        OrderDAL oDAL = new OrderDAL();
        public bool CreateOrder(Order order) {
            return oDAL.CreateOrder(order);
        }
    }
}
