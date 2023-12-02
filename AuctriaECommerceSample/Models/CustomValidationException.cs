namespace AuctriaECommerceSample.Models
{
    public class CustomValidationException : Exception
    {
        public CustomValidationException(string errorMessage) : this(new List<string> { errorMessage })
        {
        }

        public CustomValidationException(List<string> errorMessages)
        {
            ErrorMessages = errorMessages ?? new List<string>();
        }

        public List<string> ErrorMessages { get; }
        public override string Message => string.Join("\r\n", ErrorMessages);

    }
}
