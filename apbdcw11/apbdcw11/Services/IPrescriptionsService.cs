using apbdcw11.DTOs;

namespace apbdcw11.Services;

public interface IPrescriptionsService
{
    Task<bool> CreatePrescription(CreatePrescriptionDto prescription);
    Task<PatientDto?> GetPatientData(int id);
}