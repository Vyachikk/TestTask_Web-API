using System.ComponentModel.DataAnnotations;

namespace TestTask.DTO.PatientsDTO
{
    public class PatientEditDto
    {
        public int PatientId { get; set; }

        [Required(ErrorMessage = "Фамилия пациента обязательна")]
        public string PatientSurname { get; set; }

        [Required(ErrorMessage = "Имя пациента обязательно")]
        public string PatientName { get; set; }

        public string? PatientPatronymic { get; set; }

        [Required(ErrorMessage = "Адрес пациента обязателен")]
        public string PatientAddress { get; set; }

        [Required(ErrorMessage = "Пол пациента обязателен (1 - муж. 0 - жен.)")]
        public bool PatientGender { get; set; }

        [Required(ErrorMessage = "Участок пациента обязателен")]
        [Range(1, int.MaxValue, ErrorMessage = "ID участка должен быть положительным")]
        public int PatientRegionId { get; set; }
    }
}
