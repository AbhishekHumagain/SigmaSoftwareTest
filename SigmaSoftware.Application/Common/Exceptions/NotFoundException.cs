using System.Net;

namespace SigmaSoftware.Application.Common.Exceptions;

[Serializable]
public class NotFoundException : ProjectException
{
    public NotFoundException()
        : base()
    {
    }

    public NotFoundException(string message)
        : base(message)
    {
    } 
    public NotFoundException(string message, HttpStatusCode code)
        : base(message)
    {
    }

    public NotFoundException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    public NotFoundException(string name, object key)
        : base($"Entity \"{name}\" ({key}) was not found.")
    {
    }

    public NotFoundException(object key, string value)
        : base($"{key}: {value}")
    {
    }
}
