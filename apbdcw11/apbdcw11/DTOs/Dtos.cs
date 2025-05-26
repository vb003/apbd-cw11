using System.Collections;
using System.ComponentModel.DataAnnotations;
using apbdcw11.Models;

namespace apbdcw11.DTOs;

public class CreatePrescriptionDto
{
    public CreatePatientDto Patient { get; set; }
    public ICollection<CreateMedicamentDto> Medicaments { get; set; }
    public DateTime Date { get; set; }
    public DateTime DueDate { get; set; }
    public int IdDoctor { get; set; }
}

public class CreateMedicamentDto
{
    public int IdMedicament { get; set; }
    public int? Dose { get; set; }
    public string Details { get; set; }
}

public class CreatePatientDto
{
    public int IdPatient { get; set; }
    [MaxLength(100)]
    public String FirstName { get; set; }
    [MaxLength(100)]
    public String LastName { get; set; }
    
    public DateTime Birthdate { get; set; }
}

public class PatientDto
{
    public int IdPatient { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime Birthdate { get; set; }
    public ICollection<PrescriptionDto> Prescriptions { get; set; }
}

public class PrescriptionDto
{
    public int IdPrescription { get; set; }
    public DateTime Date { get; set; }
    public DateTime DueDate { get; set; }
    public ICollection<MedicamentDto> Medicaments { get; set; }
    public DoctorDto Doctor { get; set; }
}

public class DoctorDto
{
    public int IdDoctor { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
}

public class MedicamentDto
{
    public int IdMedicament { get; set; }
    public string Name { get; set; }
    public int? Dose { get; set; }
    public string Description {get; set;}
    public string Type { get; set; }
}