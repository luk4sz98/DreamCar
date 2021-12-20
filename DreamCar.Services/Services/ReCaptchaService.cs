using DreamCar.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using DreamCar.Services.Services.StaticClasses;

namespace DreamCar.Services.Services
{
    public class ReCaptchaService: HttpClient, IReCaptchaService
    { 
        private readonly ILogger _logger;
        private readonly IConfiguration _config;

        public ReCaptchaService(ILogger logger, IConfiguration configuration)
        {
            _config = configuration;
            BaseAddress = new Uri(GetUrl());
            _logger = logger;
        }

        public async Task<bool> IsValid(string captcha)
        {
            try
            {
                var postTask = await PostAsync($"?secret={AesOperationService.GetReCaptchaSecretKey()}&response={captcha}", new StringContent(""));
                var result = await postTask.Content.ReadAsStringAsync();
                var resultObject = JObject.Parse(result);
                dynamic success = resultObject["success"];
                return (bool)success;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return false;
            }
        }

        private string GetUrl() => _config.GetValue<string>("ReCaptchaUrl");
    }
}
