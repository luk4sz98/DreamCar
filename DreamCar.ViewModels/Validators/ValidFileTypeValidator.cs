using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DreamCar.ViewModels.Validators
{
    public class ValidFileTypeValidator : ValidationAttribute, IClientModelValidator
    {
        private readonly string _errorMessage = "Nieprawidłowy format pliku";

        /// <summary>
        /// Valid file extentions
        /// </summary>
        public IEnumerable<string> ValidFileTypes { get; private set; }

        /// <param name="validFileTypes">Valid file extentions(without the dot)</param>
        public ValidFileTypeValidator(
            params string[] validFileTypes)
        {
            ValidFileTypes = validFileTypes;
        }

        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return true;
            }

            var files = value as IFormFileCollection;

            foreach (var file in files)
            {
                if (String.IsNullOrEmpty(file.FileName))
                {
                    return true;
                }
            } 

            if (ValidFileTypes != null)
            {
                var validFileTypeFound = false;

                foreach (var file in files)
                {
                    var fileNameParts = file.FileName.Split('.');
                    foreach (var validFileType in ValidFileTypes)
                    {
                        if (fileNameParts[^1] == validFileType)
                        {
                            validFileTypeFound = true;
                            break;
                        }
                        else
                        {
                            validFileTypeFound = false;
                        }
                    }
                    if (!validFileTypeFound)
                        break;
                }

                if (!validFileTypeFound)
                {
                    ErrorMessage = string.Format(_errorMessage, "{0}", string.Join(",", ValidFileTypes));
                    return false;
                }
            }
            return true;
        }

        public override string FormatErrorMessage(string name)
        {
            return string.Format(_errorMessage, name, string.Join(",", ValidFileTypes));
        }

        public void AddValidation(ClientModelValidationContext context)
        {
            var errorMessage = FormatErrorMessage(context.ModelMetadata.GetDisplayName());
            MergeAttribute(context.Attributes, "data-val-validfiletype", errorMessage);
            MergeAttribute(context.Attributes, "data-val-validfiletype-validTypes", string.Join(",", ValidFileTypes));
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

