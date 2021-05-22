using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoWINVaccineFinder.Application.DTOs
{
    public class FetchStatesResponse
    {
        [JsonProperty("states")]
        public List<State> States { get; set; }
      
        [JsonProperty("ttl")]
        public int Total { get; set; }
    }
}
