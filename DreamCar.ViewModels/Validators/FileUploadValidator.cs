using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DreamCar.ViewModels.Validators
{
    public class FileUploadValidator
        : ValidationAttribute, IClientModelValidator
    {
        private readonly MinimumFileSizeValidator _minimumFileSizeValidator;
        private readonly MaximumFileSizeValidator _maximumFileSizeValidator;
        private readonly ValidFileTypeValidator _validFileTypeValidator;

        /// <param name="validFileTypes">Valid file extentions(without the dot)</param>
        public FileUploadValidator(
                params string[] validFileTypes)
            : base()
        {
            _validFileTypeValidator = new ValidFileTypeValidator(validFileTypes);
        }

        /// <param name="maximumFileSize">Maximum file size in MB</param>
        /// <param name="validFileTypes">Valid file extentions(without the dot)</param>
        public FileUploadValidator(
                double maximumFileSize
            , params string[] validFileTypes)
            : base()
        {
            _maximumFileSizeValidator = new MaximumFileSizeValidator(maximumFileSize);
            _validFileTypeValidator = new ValidFileTypeValidator(validFileTypes);
        }

        /// <param name="minimumFileSize">MinimumFileSize file size in MB</param>
        /// <param name="maximumFileSize">Maximum file size in MB</param>
        /// <param name="validFileTypes">Valid file extentions(without the dot)</param>
        public FileUploadValidator(
                double minimumFileSize
            , double maximumFileSize
            , params string[] validFileTypes)
            : base()
        {
            _minimumFileSizeValidator = new MinimumFileSizeValidator(minimumFileSize);
            _maximumFileSizeValidator = new MaximumFileSizeValidator(maximumFileSize);
            _validFileTypeValidator = new ValidFileTypeValidator(validFileTypes);
        }

        protected override ValidationResult IsValid(
                object value
            , ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }

            try
            {
                var errorMessage = new StringBuilder();
                var file = value as IFormFile;

                if (_minimumFileSizeValidator != null)
                {
                    if (!_minimumFileSizeValidator.IsValid(file))
                    {
                        errorMessage.Append(String.Format("{0}. ", _minimumFileSizeValidator.FormatErrorMessage(validationContext.DisplayName)));
                    }
                }

                if (_maximumFileSizeValidator != null)
                {
                    if (!_maximumFileSizeValidator.IsValid(file))
                    {
                        errorMessage.Append(String.Format("{0}. ", _maximumFileSizeValidator.FormatErrorMessage(validationContext.DisplayName)));
                    }
                }

                if (_validFileTypeValidator != null)
                {
                    if (!_validFileTypeValidator.IsValid(file))
                    {
                        errorMessage.Append(String.Format("{0}. ", _validFileTypeValidator.FormatErrorMessage(validationContext.DisplayName)));
                    }
                }

                if (String.IsNullOrEmpty(errorMessage.ToString()))
                {
                    return ValidationResult.Success;
                }
                else
                {
                    return new ValidationResult(errorMessage.ToString());
                }
            }
            catch (Exception excp)
            {
                return new ValidationResult(excp.Message);
            }
        }

        public void AddValidation(ClientModelValidationContext context)
        {
            var errorMessage = FormatErrorMessage(context.ModelMetadata.GetDisplayName());
            MergeAttribute(context.Attributes, "data-val-fileuploadvalidator", errorMessage);
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
