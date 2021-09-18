using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DreamCar.ViewModels.Validators
{
    public class MaximumFileSizeValidator : ValidationAttribute, IClientModelValidator
    {
        private readonly string _errorMessage = "Jedno z wybranych zdjęć jest większe niż {1} MB";

        /// <summary>
        /// Maximum file size in MB
        /// </summary>
        public double MaximumFileSize { get; private set; }

        /// <param name="maximumFileSize">Maximum file size in MB</param>
        public MaximumFileSizeValidator(
            double maximumFileSize)
        {
            MaximumFileSize = maximumFileSize;
        }

        public void AddValidation(ClientModelValidationContext context)
        {
            var errorMessage = FormatErrorMessage(context.ModelMetadata.GetDisplayName());
            MergeAttribute(context.Attributes, "data-val", "true");
            MergeAttribute(context.Attributes, "data-val-maximumfilesize", errorMessage);
            MergeAttribute(context.Attributes, "data-val-maximumfilesize-maximumSize", MaximumFileSize.ToString());
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
                if (!IsValidMaximumFileSize(file.Length))
                {
                    ErrorMessage = string.Format(_errorMessage, "{0}", MaximumFileSize);
                    return false;
                }
            }

            return true;
        }

        public override string FormatErrorMessage(
            string name)
        {
            return string.Format(_errorMessage, name, MaximumFileSize);
        }

        private bool IsValidMaximumFileSize(
            long fileSize)
        {
            return ConvertBytesToMegabytes(fileSize) <= MaximumFileSize;
        }

        private static double ConvertBytesToMegabytes(
            long bytes)
        {
            return bytes / 1024f / 1024f;
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