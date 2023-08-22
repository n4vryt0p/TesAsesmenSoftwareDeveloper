using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackEnd.Database.Models;

public class Package
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    [MaxLength(1000)]
    public string? Name { get; set; }
    public string? Description { get; set; }
    //public int? AlternateId { get; set; }
    public ICollection<AppUser> AppUsers { get; } = new List<AppUser>();
}