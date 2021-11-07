using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DreamCar.ViewModels.Validators
{
    public class MaximumFileSizeValidatorAttribute : ValidationAttribute, IClientModelValidator
    {
        private readonly string _errorMessage = "Jedno z wybranych zdjęć jest większe niż {1} MB";

        /// <summary>
        /// Maximum file size in MB
        /// </summary>
        public double MaximumFileSize { get; private set; }

        /// <param name="maximumFileSize">Maximum file size in MB</param>
        public MaximumFileSizeValidatorAttribute(
            double maximumFileSize)
        {
            MaximumFileSize = maximumFileSize;
        }

        public void AddValidation(ClientModelValidationContext context)
        {
            MergeAttribute(context.Attributes, "data-val", "true");
            MergeAttribute(context.Attributes, "data-val-maximumfilesize", _errorMessage);
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
                    ErrorMessage = string.Format("{0} {1}", _errorMessage, MaximumFileSize);
                    return false;
                }
            }

            return true;
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