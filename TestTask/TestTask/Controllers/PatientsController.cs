using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TestTask.DTO.PatientsDTO;
using TestTask.Models;
using TestTask.Services;

namespace TestTask.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientsController : ControllerBase
    {
        private readonly IPatientService _patientService;

        public PatientsController(IPatientService patientService)
        {
            _patientService = patientService;
        }



        [HttpGet]
        public async Task<IActionResult> GetPatients([FromQuery] string sortBy = "name", int page = 1, int pageSize = 10)
        {
            var patients = await _patientService.GetPatientsAsync(sortBy, page, pageSize);
            return Ok(patients);
        }



        [HttpGet("{id}")]
        public async Task<IActionResult> GetPatientById(int id)
        {
            var patient = await _patientService.GetPatientByIdAsync(id);
            if (patient == null) return NotFound();
            return Ok(patient);
        }



        [HttpPost]
        public async Task<IActionResult> AddPatient([FromBody] PatientEditDto patientDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (!await _patientService.RegionExists(patientDto.PatientRegionId))
            {
                return BadRequest($"Участка с ID {patientDto.PatientRegionId} не существует.");
            }

            var newPatient = await _patientService.AddPatientAsync(patientDto);
            return CreatedAtAction(nameof(GetPatientById), new { id = newPatient.PatientId }, newPatient);
        }



        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePatient(int id, [FromBody] PatientEditDto patientDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (!await _patientService.RegionExists(patientDto.PatientRegionId))
            {
                return BadRequest($"Участка с ID {patientDto.PatientRegionId} не существует.");
            }

            var result = await _patientService.UpdatePatientAsync(id, patientDto);
            if (!result) return NotFound();

            return NoContent();
        }



        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePatient(int id)
        {
            var result = await _patientService.DeletePatientAsync(id);
            if (!result) return NotFound();

            return NoContent();
        }
    }
}
