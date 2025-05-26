using System.ComponentModel.DataAnnotations;

namespace apbdcw11.Models;

public class Medicament
{
    [Key]
    public int IdMedicament { get; set; }
    [Required]
    [StringLength(100)]
    public string Name { get; set; }
    [Required]
    [StringLength(100)]
    public string Description { get; set; }
    [Required]
    [StringLength(100)]
    public string Type { get; set; }
    
    public ICollection<PrescriptionMedicament> PrescriptionMedicaments { get; set; }
}