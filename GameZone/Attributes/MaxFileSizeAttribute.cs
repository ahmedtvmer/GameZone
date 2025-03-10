namespace GameZone.Attributes
{
    public class MaxFileSizeAttribute : ValidationAttribute
    {
        private readonly int _maxFileSizeInMB;

        public MaxFileSizeAttribute(int maxFileSizeInMB)
        {
            this._maxFileSizeInMB = maxFileSizeInMB;
        }
        protected override ValidationResult? IsValid
            (object? value, ValidationContext validationContext)
        {
            if (value is not IFormFile file || file == null || file.Length == 0)
                return ValidationResult.Success;
            if (file is not null)
            {
                if (file.Length > _maxFileSizeInMB * 1024 * 1024)
                {
                    return new ValidationResult($"File size should not exceed {_maxFileSizeInMB}MB.");
                }
                return ValidationResult.Success;
            }
            return new ValidationResult($"File is null");
        }
    }
}
