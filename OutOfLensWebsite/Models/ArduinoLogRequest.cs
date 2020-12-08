using System;
using System.Text.Json.Serialization;

namespace OutOfLensWebsite.Models
{
    public class ArduinoLogRequest
    {
        private const string CredentialPassword = "5eeb219ebc72cd90a4020538b28593fbfac63d2e0a8d6ccf6c28c21c97f00ea6";


        [JsonPropertyName("password")] public string Password { get; set; }

        [JsonPropertyName("data")] public string Data { get; set; }

        // TODO: Decrypt here
        public bool Valid => Password == CredentialPassword && Data.Length > 0;


        public string RfidData()
        {
            if (Data.Length < 3 || !Data.StartsWith("r "))
            {
                throw new InvalidOperationException("ArduinoLogRequest is not rfid");
            }

            return Data.Substring(2);

        }
    }
}