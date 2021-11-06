using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DreamCar.ViewModels.Validators
{
    public class MinimumFileSizeValidatorAttribute : ValidationAttribute, IClientModelValidator
    {
        private readonly string _errorMessage = "Jedno z wybranych zdjęć jest mniejsze niż {1} MB";

        /// <summary>
        /// Minimum file size in MB
        /// </summary>
        public double MinimumFileSize { get; private set; }

        /// <param name="minimumFileSize">MinimumFileSize file size in MB</param>
        public MinimumFileSizeValidatorAttribute(
            double minimumFileSize)
            : base()
        {
            MinimumFileSize = minimumFileSize;
        }

        public override bool IsValid(
                object value)
        {
            if (value == null)
            {
                return true;
            }

            foreach (var file in (value as IFormFileCollection))
            {
                if (!IsValidMinimumFileSize(file.Length))
                {
                    ErrorMessage = string.Format("{0} {1}", _errorMessage, MinimumFileSize);
                    return false;
                }
            }

            return true;
        }

        private bool IsValidMinimumFileSize(long fileSize)
        {
            return ConvertBytesToMegabytes(fileSize) >= MinimumFileSize;
        }

        private static double ConvertBytesToMegabytes(long bytes)
        {
            return bytes / 1024f / 1024f;
        }

        public void AddValidation(ClientModelValidationContext context)
        {
            MergeAttribute(context.Attributes, "data-val", "true");
            MergeAttribute(context.Attributes, "data-val-minimumfilesize", _errorMessage);
            MergeAttribute(context.Attributes, "data-val-minimumfilesize-minimumSize", MinimumFileSize.ToString());
        }

        private static void MergeAttribute(
            IDictionary<string, string> attributes,
            string key,
            string value)
        {
            if (attributes.ContainsKey(key))
            {
                return;
            }
            attributes.Add(key, value);
        }
    }
}