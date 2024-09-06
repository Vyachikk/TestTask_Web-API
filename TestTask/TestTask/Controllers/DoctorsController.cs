using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TestTask.DTO.DoctorsDTO;
using TestTask.Services;

namespace TestTask.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorsController : ControllerBase
    {
        private readonly IDoctorService _doctorService;

        public DoctorsController(IDoctorService doctorService)
        {
            _doctorService = doctorService;
        }



        [HttpGet]
        public async Task<IActionResult> GetDoctors([FromQuery] string sortBy = "surname", int page = 1, int pageSize = 10)
        {
            var doctors = await _doctorService.GetDoctorsAsync(sortBy, page, pageSize);
            return Ok(doctors);
        }



        [HttpGet("{id}")]
        public async Task<IActionResult> GetDoctorById(int id)
        {
            var doctor = await _doctorService.GetDoctorByIdAsync(id);
            if (doctor == null) return NotFound();
            return Ok(doctor);
        }



        [HttpPost]
        public async Task<IActionResult> AddDoctor([FromBody] DoctorEditDto doctorDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!await _doctorService.RoomExists(doctorDto.RoomId))
            {
                return BadRequest($"Кабинет с ID {doctorDto.RoomId} не существует.");
            }

            if (!await _doctorService.SpecializationExists(doctorDto.SpecializationId))
            {
                return BadRequest($"Специализация с ID {doctorDto.SpecializationId} не существует.");
            }

            if (!await _doctorService.RegionExists(doctorDto.RegionId))
            {
                return BadRequest($"Участка с ID {doctorDto.RegionId} не существует.");
            }

            var newDoctor = await _doctorService.AddDoctorAsync(doctorDto);
            return CreatedAtAction(nameof(GetDoctorById), new { id = newDoctor.DoctorId }, newDoctor);
        }




        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDoctor(int id, [FromBody] DoctorEditDto doctorDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (!await _doctorService.RoomExists(doctorDto.RoomId))
            {
                return BadRequest($"Кабинет с ID {doctorDto.RoomId} не существует.");
            }

            if (!await _doctorService.SpecializationExists(doctorDto.SpecializationId))
            {
                return BadRequest($"Специализация с ID {doctorDto.SpecializationId} не существует.");
            }

            if (!await _doctorService.RegionExists(doctorDto.RegionId))
            {
                return BadRequest($"Участка с ID {doctorDto.RegionId} не существует.");
            }

            var result = await _doctorService.UpdateDoctorAsync(id, doctorDto);
            if (!result) return NotFound();

            return NoContent();
        }



        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDoctor(int id)
        {
            var result = await _doctorService.DeleteDoctorAsync(id);
            if (!result) return NotFound();

            return NoContent();
        }
    }
}
