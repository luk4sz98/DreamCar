namespace DreamCar.Services.Services
{
    public class AppSettingsService
    {
        public const string SectionName  = "EmailCredentials";
        public string ApiKey { get; set; }
        public string Username { get; set; }
    }
}
