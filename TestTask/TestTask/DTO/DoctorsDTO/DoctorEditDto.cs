using System.ComponentModel.DataAnnotations;

namespace TestTask.DTO.DoctorsDTO
{
    public class DoctorEditDto
    {
        public int DoctorId { get; set; }

        [Required(ErrorMessage = "Имя врача обязательно")]
        public string DoctorName { get; set; }

        [Required(ErrorMessage = "Фамилия врача обязательна")]
        public string DoctorSurname { get; set; }

        public string? DoctorPatronymic { get; set; }

        [Required(ErrorMessage = "ID кабинета обязателен")]
        [Range(1, int.MaxValue, ErrorMessage = "ID кабинета должен быть положительным")]
        public int RoomId { get; set; }

        [Required(ErrorMessage = "ID специализации обязателен")]
        [Range(1, int.MaxValue, ErrorMessage = "ID специализации должен быть положительным")]
        public int SpecializationId { get; set; }

        [Required(ErrorMessage = "ID участка обязателен")]
        [Range(1, int.MaxValue, ErrorMessage = "ID участка должен быть положительным")]
        public int RegionId { get; set; }
    }
}
