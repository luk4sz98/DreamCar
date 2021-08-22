using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DreamCar.Services.Services
{
    public class AppSettings
    {
        public const string SectionName  = "EmailCredentials";
        public string Key { get; set; }
        public string ApiKey { get; set; }
        public string Username { get; set; }
    }
}
