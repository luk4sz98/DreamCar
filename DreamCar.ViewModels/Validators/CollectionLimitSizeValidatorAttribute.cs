using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DreamCar.ViewModels.Validators
{
    public class CollectionLimitSizeValidatorAttribute : ValidationAttribute, IClientModelValidator
    {
        private readonly string _errorMessage = "Możesz przesłać maksymalnie do {1} zdjęć";

        /// <summary>
        /// Maximum collection size
        /// </summary>
        public int MaximumCollectionSize { get; private set; }

        public CollectionLimitSizeValidatorAttribute(int collectionSize) : base()
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
                ErrorMessage = string.Format("{0} {1}", _errorMessage, MaximumCollectionSize);
                return false;
            }

            return true;
        }

        public void AddValidation(ClientModelValidationContext context)
        {        
            MergeAttribute(context.Attributes, "data-val", "true");
            MergeAttribute(context.Attributes, "data-val-maximumcollectionsize", _errorMessage);
            MergeAttribute(context.Attributes, "data-val-maximumcollectionsize-limit", MaximumCollectionSize.ToString());
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

