namespace CarDirectory.Web.Controllers.Fines.Dto;

public class FineDto
{
    public string Id { get; set; }
    public string StateNumber { get; set; }
    public double Price { get; set; }
    public DateTime ReceiptDate { get; set; }
    public bool IsPayed { get; set; }
}