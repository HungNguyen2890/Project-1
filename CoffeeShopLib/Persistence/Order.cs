namespace Persistence;

public class Order
{
    public int OrderID { get; set; }
    public int StaffID { get; set; }
    public List<ProductSize> ProductsSize { get; set; }
    public string? PaymentMethod { get; set; }
    public DateTime CreateAt { get; set; }
    public Staff CreateBy { get; set; }
    public DateTime UpdateAt { get; set; }
    public string Status { get; set; } = "Unprocessed";
}