using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DreamCar.ViewModels.Validators
{
    public class MinimumFileSizeValidator : ValidationAttribute, IClientModelValidator
    {
        private readonly string _errorMessage = "Zdjęcie nie może być mniejsze niż {1} MB";

        /// <summary>
        /// Minimum file size in MB
        /// </summary>
        public double MinimumFileSize { get; private set; }

        /// <param name="minimumFileSize">MinimumFileSize file size in MB</param>
        public MinimumFileSizeValidator(
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
                    ErrorMessage = string.Format(_errorMessage, "{0}", MinimumFileSize);
                    return false;
                }
            }

            return true;
        }

        public override string FormatErrorMessage(string name)
        {
            return string.Format(_errorMessage, name, MinimumFileSize);
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
            var errorMessage = FormatErrorMessage(context.ModelMetadata.GetDisplayName());
            MergeAttribute(context.Attributes, "data-val-minimumfilesize", errorMessage);
        }

        private static bool MergeAttribute(
            IDictionary<string, string> attributes,
            string key,
            string value)
        {
            if (attributes.ContainsKey(key))
            {
                return false;
            }
            attributes.Add(key, value);
            return true;
        }
    }
}