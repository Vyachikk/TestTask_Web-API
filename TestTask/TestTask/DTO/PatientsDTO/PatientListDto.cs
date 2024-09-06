namespace TestTask.DTO.PatientsDTO
{
    public class PatientListDto
    {
        public int PatientId { get; set; }
        public string PatientSurname { get; set; }
        public string PatientName { get; set; }
        public string? PatientPatronymic { get; set; }
        public string PatientAddress { get; set; }
        public bool PatientGender { get; set; }
        public int PatientRegionNumber { get; set; }
    }
}
