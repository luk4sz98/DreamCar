namespace DreamCar.Services.Services
{
    public class AppSettings
    {
        public const string SectionName  = "EmailCredentials";
        public string ApiKey { get; set; }
        public string Username { get; set; }
    }
}
