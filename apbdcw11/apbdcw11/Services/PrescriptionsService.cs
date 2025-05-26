using apbdcw11.DAL;
using apbdcw11.DTOs;
using apbdcw11.Exceptions;
using apbdcw11.Models;
using Microsoft.EntityFrameworkCore;

namespace apbdcw11.Services;

public class PrescriptionsService : IPrescriptionsService
{
    private readonly DatabaseContext _context;

    public PrescriptionsService(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<bool> CreatePrescription(CreatePrescriptionDto prescriptionDto)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            // Czy podany lek istnieje?
            var medicamentsFromRequest = prescriptionDto.Medicaments.Select(m => m.IdMedicament).ToList();
            var realMedicaments = await _context.Medicaments.Where(m => medicamentsFromRequest.Contains(m.IdMedicament))
                .ToListAsync();
            if (medicamentsFromRequest.Count != realMedicaments.Count)
                throw new NotFoundException("Some medicaments not exist");

            // Maks 10 leków
            if (medicamentsFromRequest.Count > 10)
                throw new ConflictException("Prescription should contain 10 medicaments or less");

            // Czy Duedate >= Date?
            if (prescriptionDto.DueDate < prescriptionDto.Date)
                throw new ConflictException("Due date should be greater than or equal to the prescription date");

            // Czy pacjent istnieje?
            var patient = await _context.Patients.FindAsync(prescriptionDto.Patient.IdPatient);
            int IdPatient;
            if (patient == null)
            {
                var newPatient = new Patient
                {
                    FirstName = prescriptionDto.Patient.FirstName,
                    LastName = prescriptionDto.Patient.LastName,
                    Birthdate = prescriptionDto.Patient.Birthdate
                };
                _context.Patients.Add(newPatient);
                await _context.SaveChangesAsync();
                IdPatient = newPatient.IdPatient;
            }
            else
                IdPatient = patient.IdPatient;

            // Tworzenie recepty:
            var prescription = new Prescription
            {
                Date = prescriptionDto.Date,
                DueDate = prescriptionDto.DueDate,
                IdPatient = IdPatient,
                IdDoctor = prescriptionDto.IdDoctor
            };
            _context.Prescriptions.Add(prescription);
            await _context.SaveChangesAsync();

            foreach (var medicamentDto in prescriptionDto.Medicaments)
            {
                var prescriptionMedicament = new PrescriptionMedicament
                {
                    IdPrescription = prescription.IdPrescription,
                    IdMedicament = medicamentDto.IdMedicament,
                    Dose = medicamentDto.Dose,
                    Details = medicamentDto.Details
                };
                _context.PrescriptionMedicaments.Add(prescriptionMedicament);
            }
            
            await _context.SaveChangesAsync();
            
            await transaction.CommitAsync();
            return true;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<PatientDto?> GetPatientData(int id)
    {
        var data = await _context.Patients
            .Where(p => p.IdPatient == id)
            .Select((p =>
                    new PatientDto
                    {
                        IdPatient = p.IdPatient,
                        FirstName = p.FirstName,
                        LastName = p.LastName,
                        Birthdate = p.Birthdate,
                        Prescriptions = p.Prescriptions
                            .OrderBy(pr => pr.DueDate)
                            .Select(pr =>
                                new PrescriptionDto
                                {
                                    IdPrescription = pr.IdPrescription,
                                    Date = pr.Date,
                                    DueDate = pr.DueDate,
                                    Doctor = new DoctorDto
                                    {
                                        IdDoctor = pr.Doctor.IdDoctor,
                                        FirstName = pr.Doctor.FirstName,
                                        LastName = pr.Doctor.LastName,
                                        Email = pr.Doctor.Email
                                    },
                                    Medicaments = pr.PrescriptionMedicaments.Select(pm =>
                                        new MedicamentDto
                                        {
                                            IdMedicament = pm.IdMedicament,
                                            Name = pm.Medicament.Name,
                                            Dose = pm.Dose,
                                            Description = pm.Medicament.Description,
                                            Type = pm.Medicament.Type
                                        }
                                    ).ToList()
                                }).ToList()
                    }
                )
            ).FirstOrDefaultAsync();
        
        return data;
    }
}