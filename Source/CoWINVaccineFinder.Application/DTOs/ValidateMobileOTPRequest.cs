using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoWINVaccineFinder.Application.DTOs
{
    public class ValidateMobileOTPRequest
    {
        public string otp { get; set; }
        public string txnId { get; set; }
    }
}
