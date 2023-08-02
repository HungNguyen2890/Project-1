namespace Persistence;

public class Product
{
    public int ProductID { get; set; }
    public string? ProductName { get; set; }
    public string? Description { get; set; }
    public int Status { get; set; }
    public List<ProductSize> ProductSizes { get; set; }
}