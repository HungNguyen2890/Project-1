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
        public bool ConfirmOrder(int orderID) {
            return oDAL.UpdateOrder(orderID, OrderFilter.CHANGE_STATUS_TO_CONFIRMED);
        }
        public bool HandleOrder(int orderID) {
            return oDAL.UpdateOrder(orderID, OrderFilter.CHANGE_STATUS_TO_COMPLETED);
        }
        public List<Order> GetOrdersPending() {
            return oDAL.GetOrders(OrderFilter.GET_ORDERS_PENDING);
        }
        public List<Order> GetOrdersConfirmed() {
            return oDAL.GetOrders(OrderFilter.GET_ORDERS_CONFIRMED);
        }
        public List<Order> GetOrdersCompleted() {
            return oDAL.GetOrders(OrderFilter.GET_ORDERS_COMPLETED);
        }
        public Order GetOrderByID(int orderID) {
            return oDAL.GetOrderByID(orderID);
        }
    }
}
