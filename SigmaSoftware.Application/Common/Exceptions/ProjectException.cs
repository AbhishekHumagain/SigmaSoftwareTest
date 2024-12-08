using System.Runtime.Serialization;

namespace SigmaSoftware.Application.Common.Exceptions
{
    [Serializable]
    public class ProjectException : Exception
    {
        //
        // Summary:
        //     Creates a new ProjectException object.
        public ProjectException()
        {
        }

        //
        // Summary:
        //     Creates a new ProjectException object.
        public ProjectException(SerializationInfo serializationInfo, StreamingContext context)
            : base(serializationInfo, context)
        {
        }

        //
        // Summary:
        //     Creates a new ProjectException object.
        //
        // Parameters:
        //   message:
        //     Exception message
        public ProjectException(string message)
            : base(message)
        {
        }

        //
        // Summary:
        //     Creates a new ProjectException object.
        //
        // Parameters:
        //   message:
        //     Exception message
        //
        //   innerException:
        //     Inner exception
        public ProjectException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
