using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoWINVaccineFinder.Application.DTOs
{
    public partial class FetchIdTypesResponse
    {
        [JsonProperty("types")]
        public List<IdType> Types { get; set; }

        [JsonProperty("ttl")]
        public long total { get; set; }
    }
}
