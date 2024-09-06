using System;
using System.Collections.Generic;

namespace TestTask.Models;

public partial class Patient
{
    public int PatientId { get; set; }

    public string PatientSurname { get; set; } = null!;

    public string PatientName { get; set; } = null!;

    public string? PatientPatronymic { get; set; }

    public string PatientAddress { get; set; } = null!;

    public bool PatientGender { get; set; }

    public int? PatientRegion { get; set; }

    public virtual Region? Region { get; set; }
}
