using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp4
{
    public class GenerateMobileOTPRequestDTO
    {
        public GenerateMobileOTPRequestDTO()
        {
            secret = "U2FsdGVkX19583qicJSf45NTlSO55PyYIVIRcyhKuOZja/nwJBGagQYf7s0nZbBz1A3kg1AM2XTH6rPXlCnb6A==";
        }

        public string mobile { get; set; }
        public string secret { get; set; }
    }
}
