namespace Persistence;

public class ProductSize
{
    public int ProductSizeID { get; set; }
    public int ProductID { get; set; }
    public Product Product { get; set; }
    public int Quantity { get; set; }
    public Size Size { get; set; }
    public int SizeID { get; set; }
    public int ProductSizeStatus { get; set; }
    public decimal Price { get; set; }
}