using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DreamCar.ViewModels.Validators
{
    public class CollectionLimitSizeValidator : ValidationAttribute, IClientModelValidator
    {
        private readonly string _errorMessage = "Możesz przesłać maksymalnie do {1} zdjęć";

        /// <summary>
        /// Maximum collection size
        /// </summary>
        public int MaximumCollectionSize { get; private set; }

        public CollectionLimitSizeValidator(int collectionSize) : base()
        {
            MaximumCollectionSize = collectionSize;
        }

        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return true;
            }

            var collection = value as IFormFileCollection;
            
            if (collection.Count > MaximumCollectionSize)
            {
                ErrorMessage = string.Format(_errorMessage, "{0}", MaximumCollectionSize);
                return false;
            }

            return true;
        }

        public override string FormatErrorMessage(string name)
        {
            return string.Format(_errorMessage, name, MaximumCollectionSize);
        }

        public void AddValidation(ClientModelValidationContext context)
        {
            var errorMessage = FormatErrorMessage(context.ModelMetadata.GetDisplayName());
            MergeAttribute(context.Attributes, "data-val", "true");
            MergeAttribute(context.Attributes, "data-val-maximumcollectionsize", errorMessage);
            MergeAttribute(context.Attributes, "data-val-maximumcollectionsize-limit", MaximumCollectionSize.ToString());
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

