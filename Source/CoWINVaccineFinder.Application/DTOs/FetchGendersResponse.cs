using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoWINVaccineFinder.Application.DTOs
{
    public partial class FetchGendersResponse
    {
        [JsonProperty("genders")]
        public List<Gender> Genders { get; set; }

        [JsonProperty("ttl")]
        public long Total { get; set; }
    }
}
