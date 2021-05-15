using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoWINVaccineFinder.Application.DTOs
{
    public class GenerateMobileOTPRequest
    {
        public string mobile { get; set; }
        public string secret { get; set; }
    }
}
