using apbdcw11.DTOs;
using apbdcw11.Exceptions;
using apbdcw11.Models;
using apbdcw11.Services;
using Microsoft.AspNetCore.Mvc;

namespace apbdcw11.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PrescriptionsController : ControllerBase
{
    private readonly IPrescriptionsService _prescriptionsService;
    public PrescriptionsController(IPrescriptionsService prescriptionsService)
    {
        _prescriptionsService = prescriptionsService;
    }
    
    // Końcówka 1: Dodanie nowej recepty
    [HttpPost()]
    public async Task<IActionResult> AddPrescription([FromBody] CreatePrescriptionDto createPrescriptionDto)
    {
        try
        {
            if (await _prescriptionsService.CreatePrescription(createPrescriptionDto))
            {
                return Ok("Added new Prescription");
            }
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (ConflictException e)
        {
            return Conflict(e.Message);
        }
        return BadRequest("Failed to add prescription");
    }
    
    // Końcówka 2: Wyświetlenie wszystkich danych na temat konkretnego pacjenta:
    // - z listą recept (sortowanie po DueDate) i leków, które pobrał
    // - wszystkie info na temat leków i lekarzy
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetPatientData(int id)
    {
        var data = await _prescriptionsService.GetPatientData(id);
        if (data == null)
        {
            return NotFound("Patient not found");
        }
        return Ok(data);
    }
}