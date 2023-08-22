using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackEnd.Database.Models;

public class Product : IAuditable
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string? Nama { get; set; }
    public string? Deskripsi { get; set; }
    public DateTime? TanggalKadaluarsa { get; set; }
    public int Jumlah { get; set; }
    public int HargaSatuan { get; set; }
    [DefaultValue(true)]
    public bool IsActive { get; set; }

    public ICollection<Transaction> Transactions { get; } = new List<Transaction>();
    public ICollection<ProductTansaction> ProductTansactions { get; } = new List<ProductTansaction>();
}