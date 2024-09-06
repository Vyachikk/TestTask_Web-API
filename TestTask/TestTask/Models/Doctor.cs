using System;
using System.Collections.Generic;

namespace TestTask.Models;

public partial class Doctor
{
    public int DoctorId { get; set; }

    public string DoctorName { get; set; } = null!;

    public string DoctorSurname { get; set; } = null!;

    public string DoctorPatronymic { get; set; } = null!;

    public int RoomId { get; set; }

    public int SpecializationId { get; set; }

    public int? RegionId { get; set; }

    public virtual Region? Region { get; set; }

    public virtual Room Room { get; set; } = null!;

    public virtual Specialization Specialization { get; set; } = null!;
}
