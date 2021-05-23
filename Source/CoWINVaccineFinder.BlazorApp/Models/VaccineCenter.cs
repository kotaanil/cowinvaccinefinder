using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoWINVaccineFinder.BlazorApp.Models
{
    public class VaccineCenter
    {
        public string CenterName { get; set; }
        public string VaccineType { get; set; }
        public long MinimumAgeLimit { get; set; }
        public long Availability { get; set; }
        public long AvailabilityDose1 { get; set; }
        public long AvailabilityDose2 { get; set; }
    }
}
