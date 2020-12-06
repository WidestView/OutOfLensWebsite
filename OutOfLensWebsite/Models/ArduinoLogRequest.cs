using System;
using System.Text.Json.Serialization;
using OutOfLens_ASP.Controllers;

namespace OutOfLens_ASP.Models
{
    public class ArduinoLogRequest
    {
        public const char RfidLogChar = 'r';

        private const string EncryptionKey = "527be398166707a4f4edbffd02f3d335";
        private const string CredentialPassword = "5eeb219ebc72cd90a4020538b28593fbfac63d2e0a8d6ccf6c28c21c97f00ea6";


        [JsonPropertyName("password")] public string Password { get; set; }

        [JsonPropertyName("data")] public string Data { get; set; }

        // TODO: Decrypt here
        public bool Valid => Password == CredentialPassword && Data.Length > 0;


        public string RfidData()
        {
            if (Data.Length < 2 || Data[0] != RfidLogChar)
            {
                throw new InvalidOperationException("ArduinoLogRequest is not rfid");
            }

            return Data.Substring(1);

        }
    }
}