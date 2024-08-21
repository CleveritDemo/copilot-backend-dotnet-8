using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Marena.API.Models;

public class Movie
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; }

    [Range(0, 10)]
    public double Score { get; set; }

    [Required]
    [MaxLength(200)]
    public string Genres { get; set; }

    [Range(1888, 2100)]
    public int Year { get; set; }
}
