using Newtonsoft.Json;

namespace Models
{
    public class User
    {
        [JsonProperty("Login")]
        public string Login { get; set; }

        [JsonProperty("Password")]
        public string Password { get; set; }
    }
}
