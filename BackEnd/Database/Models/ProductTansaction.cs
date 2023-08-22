namespace BackEnd.Database.Models;

public class ProductTansaction
{
    public int ProductId { get; set; }
    public long TransactionId { get; set; }
    public int Kuantitas { get; set; }
    public int HargaSatuanTrx { get; set; }
    public Product? Product { get; set; }
    public Transaction? Transaction { get; set; }
}