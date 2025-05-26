using apbdcw11.Models;
using Microsoft.EntityFrameworkCore;

namespace apbdcw11.DAL;

public class DatabaseContext : DbContext
{
    public DbSet<Patient> Patients { get; set; }
    public DbSet<Doctor> Doctors { get; set; }
    public DbSet<Medicament> Medicaments { get; set; }
    public DbSet<Prescription> Prescriptions { get; set; }
    public DbSet<PrescriptionMedicament> PrescriptionMedicaments { get; set; }

    protected DatabaseContext()
    {
    }

    public DatabaseContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // Seed
        // Seed Doctors
        modelBuilder.Entity<Doctor>().HasData(
            new Doctor
            {
                IdDoctor = 1,
                FirstName = "Daniel",
                LastName = "Davis",
                Email = "davis@gmail.com",
            },
            new Doctor
            {
                IdDoctor = 2,
                FirstName = "James",
                LastName = "Bond",
                Email = "james@gmail.com",
            }
        );

        // Seed Patients
        modelBuilder.Entity<Patient>().HasData(
            new Patient
            {
                IdPatient = 1,
                FirstName = "Rina",
                LastName = "Bond",
                Birthdate = new DateTime(2000, 1, 1)
            },
            new Patient
            {
                IdPatient = 2,
                FirstName = "Jon",
                LastName = "Snow",
                Birthdate = new DateTime(1999, 4, 13)
            }
        );

        // Seed Medicaments
        modelBuilder.Entity<Medicament>().HasData(
            new Medicament
            {
                IdMedicament = 1,
                Description = "bla bla bla",
                Name = "Apap pro forte",
                Type = "Analgesics"
            },
            new Medicament
            {
                IdMedicament = 2,
                Description = "desfdslfjcksdlehk",
                Name = "Health Potion",
                Type = "Antibiotics"
            }
        );
    }
}