namespace Persistence;

public class ProductUpdates
{
    public int staff_id { get; set; }
    public int ProductID { get; set; }
    public int ProductUpdatesID { get; set; }
    public int UpdateBy { get; set; }
    public int CreateBy { get; set; }
    public DateTime CreateAt { get; set; }
    public string ?Description {get;set;}
    public DateTime UpdateAt{get;set;}


}