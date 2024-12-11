using System.Net;
using SigmaSoftware.Domain.Common.Errors;

namespace SigmaSoftware.Domain.Errors;

public static class UserErrors
{
    public static readonly Error UserNotFound = new(HttpStatusCode.NotFound,"User not found!");
    public static readonly Error UserNotCreated = new(HttpStatusCode.NotFound,"User not created!");
    public static readonly Error EmailAlreadyTaken = new(HttpStatusCode.Ambiguous,"This email is already taken!");
    public static readonly Error InValidEmail = new(HttpStatusCode.Ambiguous,"Invalid Email Address!");
      
}