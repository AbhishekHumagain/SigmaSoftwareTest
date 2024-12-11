using System.Net;
using System.Text.Json.Serialization;

namespace SigmaSoftware.Domain.Common.Errors
{

    public record Error(HttpStatusCode Status, string ErrorMessage, [property: JsonIgnore] bool CommitTransaction = false)
    {
        public static readonly Error None = new(HttpStatusCode.OK, string.Empty);
    }

}