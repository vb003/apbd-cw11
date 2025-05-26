using apbdcw11.Controllers;
using apbdcw11.DTOs;
using apbdcw11.Exceptions;
using apbdcw11.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace apbdcw11.Tests;

public class EndpointTests
{
    [Fact]
    public async Task CreatePerscriptionShouldReturnOkIfCorrectData()
    {
        var mockService = new Mock<IPrescriptionsService>();
        var createPrescriptionRequestDto = new CreatePrescriptionDto
        {
            Date = DateTime.Parse("2021-01-01"),
            DueDate = DateTime.Parse("2021-01-02"),
            IdDoctor = 1,
            Patient = new CreatePatientDto
            {
                FirstName = "Rina",
                LastName = "Bond",
                Birthdate = new DateTime(2000, 1, 1),
            },
            Medicaments = new List<CreateMedicamentDto>()
        };
        mockService.Setup(s => s.CreatePrescription(createPrescriptionRequestDto)).ReturnsAsync(true);
        
        var prescriptionsController = new PrescriptionsController(mockService.Object);
        var result = await prescriptionsController.AddPrescription(createPrescriptionRequestDto);
        Assert.IsType<OkObjectResult>(result);
    }
    
    [Fact]
    public async Task CreatePerscriptionShouldReturnNotFoundIfPatientDoesNotExist()
    {
        var mockService = new Mock<IPrescriptionsService>();
        var createPrescriptionRequestDto = new CreatePrescriptionDto
        {
            Date = DateTime.Parse("2021-01-01"),
            DueDate = DateTime.Parse("2021-01-02"),
            IdDoctor = 1,
            Patient = new CreatePatientDto
            {
                FirstName = "Johjjn",
                LastName = "Dokke",
                Birthdate = DateTime.Parse("2000-01-01"),
            },
            Medicaments = new List<CreateMedicamentDto>()
        };
        mockService.Setup(s => s.CreatePrescription(It.IsAny<CreatePrescriptionDto>()))
            .ThrowsAsync(new NotFoundException("Patient not found"));
        
        var prescriptionsController = new PrescriptionsController(mockService.Object);
        var result = await prescriptionsController.AddPrescription(createPrescriptionRequestDto);
        Assert.IsType<NotFoundObjectResult>(result);
    }
    
    [Fact]
    public async Task CreatePrescriptionShouldReturnConflictIfConflict()
    {
        var mockService = new Mock<IPrescriptionsService>();
        var createPrescriptionRequestDto = new CreatePrescriptionDto
        {
            Date = DateTime.Parse("2021-01-01"),
            DueDate = DateTime.Parse("2020-01-02"), // DueDate < Date
            IdDoctor = 1,
            Patient = new CreatePatientDto
            {
                FirstName = "Johjjn",
                LastName = "Dokke",
                Birthdate = DateTime.Parse("2000-01-01"),
            },
            Medicaments = new List<CreateMedicamentDto>()
        };
        mockService.Setup(s => s.CreatePrescription(It.IsAny<CreatePrescriptionDto>()))
            .ThrowsAsync(new ConflictException("Due date should be greater than or equal to the prescription date"));
        
        var prescriptionsController = new PrescriptionsController(mockService.Object);
        var result = await prescriptionsController.AddPrescription(createPrescriptionRequestDto);
        Assert.IsType<ConflictObjectResult>(result);
    }
    
}