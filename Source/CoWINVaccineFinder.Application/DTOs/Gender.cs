using Newtonsoft.Json;

namespace CoWINVaccineFinder.Application.DTOs
{
    public partial class Gender
    {
        [JsonProperty("gender")]
        public string GenderGender { get; set; }

        [JsonProperty("gender_l")]
        public string GenderL { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }
    }
}
