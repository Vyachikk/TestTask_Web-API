using Microsoft.EntityFrameworkCore;
using TestTask.DTO.PatientsDTO;
using TestTask.Models;

namespace TestTask.Services
{
    public interface IPatientService
    {
        Task<IEnumerable<PatientListDto>> GetPatientsAsync(string sortBy, int page, int pageSize);
        Task<PatientEditDto> GetPatientByIdAsync(int id);
        Task<Patient> AddPatientAsync(PatientEditDto patient);
        Task<bool> UpdatePatientAsync(int id, PatientEditDto patient);
        Task<bool> DeletePatientAsync(int id);
        Task<bool> RegionExists(int regionId);
    }



    public class PatientService : IPatientService
    {
        private readonly ApplicationContext _context;

        public PatientService(ApplicationContext context)
        {
            _context = context;
        }



        // Существует ли участок
        public async Task<bool> RegionExists(int regionId)
        {
            return await _context.Regions.AnyAsync(s => s.RegionId == regionId);
        }



        public async Task<Patient> AddPatientAsync(PatientEditDto newPatient)
        {
            var patient = new Patient
            {
                PatientName = newPatient.PatientName,
                PatientSurname = newPatient.PatientSurname,
                PatientPatronymic = newPatient.PatientPatronymic,
                PatientAddress = newPatient.PatientAddress,
                PatientGender = newPatient.PatientGender,
                PatientRegion = newPatient.PatientRegionId
            };

            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();

            return patient;
        }



        public async Task<bool> DeletePatientAsync(int id)
        {
            var patient = await _context.Patients.FindAsync(id);
            if (patient == null) return false;

            _context.Patients.Remove(patient);
            await _context.SaveChangesAsync();
            return true;
        }



        public async Task<PatientEditDto> GetPatientByIdAsync(int id)
        {
            var patient = await _context.Patients.FindAsync(id);
            if (patient == null) return null;

            return new PatientEditDto
            {
                PatientId = patient.PatientId,
                PatientName = patient.PatientName,
                PatientSurname = patient.PatientSurname,
                PatientPatronymic = patient.PatientPatronymic,
                PatientAddress = patient.PatientAddress,
                PatientGender = patient.PatientGender,
                PatientRegionId = (int)patient.PatientRegion
            };
        }



        public async Task<IEnumerable<PatientListDto>> GetPatientsAsync(string sortBy, int page, int pageSize)
        {
            var query = _context.Patients
                .Include(p => p.Region)
                .AsQueryable();

            switch (sortBy.ToLower())
            {
                case "surname":
                    query = query.OrderBy(p => p.PatientSurname);
                    break;
                case "address":
                    query = query.OrderBy(p => p.PatientAddress);
                    break;
                case "region":
                    query = query.OrderBy(p => p.Region.RegionNumber);
                    break;
                default:
                    query = query.OrderBy(p => p.PatientName);
                    break;
            }

            var patients = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            return patients.Select(p => new PatientListDto
            {
                PatientId = p.PatientId,
                PatientName = p.PatientName,
                PatientSurname = p.PatientSurname,
                PatientPatronymic = p.PatientPatronymic,
                PatientAddress = p.PatientAddress,
                PatientGender = p.PatientGender,
                PatientRegionNumber = p.Region.RegionNumber
            });
        }



        public async Task<bool> UpdatePatientAsync(int id, PatientEditDto patient)
        {
            var updatedPatient = await _context.Patients.FindAsync(id);
            if (updatedPatient == null) return false;

            updatedPatient.PatientName = patient.PatientName;
            updatedPatient.PatientSurname = patient.PatientSurname;
            updatedPatient.PatientPatronymic = patient.PatientPatronymic;
            updatedPatient.PatientAddress = patient.PatientAddress;
            updatedPatient.PatientGender = patient.PatientGender;
            updatedPatient.PatientRegion = patient.PatientRegionId;

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
