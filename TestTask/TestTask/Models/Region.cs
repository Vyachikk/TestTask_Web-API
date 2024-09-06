using System;
using System.Collections.Generic;

namespace TestTask.Models;

public partial class Region
{
    public int RegionId { get; set; }

    public int RegionNumber { get; set; }

    public virtual ICollection<Doctor> Doctors { get; set; } = new List<Doctor>();
    public virtual ICollection<Patient> Patients { get; set; } = new List<Patient>();
}
