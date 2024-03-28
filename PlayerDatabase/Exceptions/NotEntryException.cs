namespace Data_base.Exceptions;

public class NotEntryException : Exception
{
    public NotEntryException(string message = "The entry is not in the database") : base(message)
    {
    }
}