namespace RestaurantAPI.Exceptions
{
    public class ValidationException(string errorMessage, string errorCode = "ValidationError") : Exception(errorMessage)
    {
        public string ErrorCode { get; set; } = errorCode;
        public string ErrorMessage { get; set; } = errorMessage;
    }
}
