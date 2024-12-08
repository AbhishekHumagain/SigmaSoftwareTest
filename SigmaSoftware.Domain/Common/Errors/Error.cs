using System.Net;

namespace SigmaSoftware.Domain.Common.Errors
{
    public record Error(HttpStatusCode Status, string ErrorMessage)
    {
        public static Error None = new(HttpStatusCode.OK, string.Empty);
    }
}