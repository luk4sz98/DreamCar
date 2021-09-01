﻿using DreamCar.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace DreamCar.Services.Services
{
    public class ReCaptchaService: HttpClient, IReCaptcha
    { 
        private readonly ILogger _logger;

        public ReCaptchaService(ILogger logger)
        {
            BaseAddress = new Uri("https://www.google.com/recaptcha/api/siteverify");
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
    }
}