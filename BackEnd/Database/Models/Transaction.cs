using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackEnd.Database.Models;

public class Transaction : IAuditable
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }
    public string? AlamatTujuan { get; set; }
    public int TotalHarga { get; set; }
    [DefaultValue(true)]
    public bool IsActive { get; set; }

    public int? AppUserId { get; set; }
    public AppUser? AppUser { get; set; }
    public ICollection<Product> Product { get; } = new List<Product>();
    public ICollection<ProductTansaction> ProductTansactions { get; } = new List<ProductTansaction>();
}