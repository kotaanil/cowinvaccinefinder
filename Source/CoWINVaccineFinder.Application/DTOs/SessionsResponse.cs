using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CoWINVaccineFinder.Application.DTOs
{
    public partial class SessionsResponse
    {
        [JsonProperty("centers")]
        public List<Center> Centers { get; set; }
    }
}
