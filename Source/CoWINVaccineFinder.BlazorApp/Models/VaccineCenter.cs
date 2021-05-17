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
        public string MinimumAgeLimit { get; set; }
        public string Availability { get; set; }
    }
}
