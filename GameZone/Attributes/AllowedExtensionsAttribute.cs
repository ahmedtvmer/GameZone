namespace GameZone.Attributes
{
    public class AllowedExtensionsAttribute : ValidationAttribute
    {
        private readonly string _allowedExtensions;
        public AllowedExtensionsAttribute(string allowedExtensions)
        {
            _allowedExtensions = allowedExtensions;
        }

        protected override ValidationResult? IsValid
            (object? value, ValidationContext validationContext)
        {
            if (value is not IFormFile file || file == null || file.Length == 0)
                return ValidationResult.Success;

            if (file is not null)
            {
                var extension = Path.GetExtension(file.FileName);
                if (!_allowedExtensions.Split(',').Contains(extension.ToLower()))
                {
                    return new ValidationResult($"Only {_allowedExtensions} are allowed!");
                }
            return ValidationResult.Success;
            }
            return new ValidationResult($"File is null");

        }
    }
}
