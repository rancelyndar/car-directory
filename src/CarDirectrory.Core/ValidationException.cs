namespace CarDirectrory.Core;

public class ValidationException : System.Exception
{
    public ValidationException(string message) : base(message) {}
}