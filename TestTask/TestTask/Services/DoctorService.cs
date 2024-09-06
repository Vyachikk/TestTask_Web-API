using Microsoft.EntityFrameworkCore;
using TestTask.DTO.DoctorsDTO;
using TestTask.Models;

namespace TestTask.Services
{
    public interface IDoctorService
    {
        Task<IEnumerable<DoctorListDto>> GetDoctorsAsync(string sortBy, int page, int pageSize);
        Task<DoctorEditDto> GetDoctorByIdAsync(int id);
        Task<Doctor> AddDoctorAsync(DoctorEditDto doctor);
        Task<bool> UpdateDoctorAsync(int id, DoctorEditDto doctor);
        Task<bool> DeleteDoctorAsync(int id);
        Task<bool> RoomExists(int roomId);
        Task<bool> SpecializationExists(int specializationId);
        Task<bool> RegionExists(int regionId);
    }



    public class DoctorService : IDoctorService
    {
        private readonly ApplicationContext _context;

        public DoctorService(ApplicationContext context)
        {
            _context = context;
        }



        // Существует ли кабинет
        public async Task<bool> RoomExists(int roomId)
        {
            return await _context.Rooms.AnyAsync(r => r.RoomId == roomId);
        }

        // Существует ли специализация
        public async Task<bool> SpecializationExists(int specializationId)
        {
            return await _context.Specializations.AnyAsync(s => s.SpecializationId == specializationId);
        }

        // Существует ли участок
        public async Task<bool> RegionExists(int regionId)
        {
            return await _context.Regions.AnyAsync(s => s.RegionId == regionId);
        }



        public async Task<Doctor> AddDoctorAsync(DoctorEditDto newDoctor)
        {
            var doctor = new Doctor
            {
                DoctorName = newDoctor.DoctorName,
                DoctorSurname = newDoctor.DoctorSurname,
                DoctorPatronymic = newDoctor.DoctorPatronymic,
                RoomId = newDoctor.RoomId,
                SpecializationId = newDoctor.SpecializationId,
                RegionId = newDoctor.RegionId
            };

            _context.Doctors.Add(doctor);
            await _context.SaveChangesAsync();

            return doctor;
        }



        public async Task<bool> DeleteDoctorAsync(int id)
        {
            var doctor = await _context.Doctors.FindAsync(id);
            if (doctor == null) return false;

            _context.Doctors.Remove(doctor);
            await _context.SaveChangesAsync();
            return true;
        }



        public async Task<DoctorEditDto> GetDoctorByIdAsync(int id)
        {
            var doctor = await _context.Doctors.FindAsync(id);
            if (doctor == null) return null;

            return new DoctorEditDto
            {
                DoctorId = doctor.DoctorId,
                DoctorName = doctor.DoctorName,
                DoctorSurname = doctor.DoctorSurname,
                DoctorPatronymic = doctor.DoctorPatronymic,
                RoomId = doctor.RoomId,
                SpecializationId = doctor.SpecializationId,
                RegionId = (int)doctor.RegionId
            };
        }



        public async Task<IEnumerable<DoctorListDto>> GetDoctorsAsync(string sortBy, int page, int pageSize)
        {
            var query = _context.Doctors
                .Include(d => d.Room)
                .Include(d => d.Specialization)
                .Include(d => d.Region)
                .AsQueryable();

            switch (sortBy.ToLower())
            {
                case "name":
                    query = query.OrderBy(d => d.DoctorName);
                    break;
                case "specialization":
                    query = query.OrderBy(d => d.Specialization.SpecializationName);
                    break;
                case "region":
                    query = query.OrderBy(d => d.Region.RegionNumber);
                    break;
                default:
                    query = query.OrderBy(d => d.DoctorSurname);
                    break;
            }

            var doctors = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            return doctors.Select(d => new DoctorListDto
            {
                DoctorId = d.DoctorId,
                DoctorName = d.DoctorName,
                DoctorSurname = d.DoctorSurname,
                DoctorPatronymic = d.DoctorPatronymic,
                RoomNumber = d.Room.RoomNumber,
                SpecializationName = d.Specialization.SpecializationName,
                RegionNumber = d.Region.RegionNumber
            });
        }



        public async Task<bool> UpdateDoctorAsync(int id, DoctorEditDto doctorDto)
        {
            var doctor = await _context.Doctors.FindAsync(id);
            if (doctor == null) return false;

            doctor.DoctorName = doctorDto.DoctorName;
            doctor.DoctorSurname = doctorDto.DoctorSurname;
            doctor.DoctorPatronymic = doctorDto.DoctorPatronymic;
            doctor.RoomId = doctorDto.RoomId;
            doctor.SpecializationId = doctorDto.SpecializationId;
            doctor.RegionId = doctorDto.RegionId;

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
