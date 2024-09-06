namespace TestTask.DTO.DoctorsDTO
{
    public class DoctorListDto
    {
        public int DoctorId { get; set; }
        public string DoctorName { get; set; }
        public string DoctorSurname { get; set; }
        public string? DoctorPatronymic { get; set; }
        public int RoomNumber { get; set; }
        public string SpecializationName { get; set; }
        public int RegionNumber { get; set; }
    }
}
