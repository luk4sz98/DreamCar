using DreamCar.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace DreamCar.Services.Services
{
    public class ReCaptcha: IReCaptcha
    { 
        private readonly HttpClient _reCaptchaClient;
        private readonly ILogger _logger;

        public ReCaptcha(HttpClient reCaptchaClient, ILogger logger)
        {
            _reCaptchaClient = reCaptchaClient;
            _logger = logger;
        }

        public async Task<bool> IsValid(string captcha)
        {
            try
            {
                var postTask = await _reCaptchaClient
                    .PostAsync($"?secret={AesOperation.GetReCaptchaSecretKey()}&response={captcha}", new StringContent(""));
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
    }
}
