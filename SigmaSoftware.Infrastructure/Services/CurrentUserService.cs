using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using SigmaSoftware.Application.Common.Interfaces;

namespace SigmaSoftware.Infrastructure.Services;

public class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUserService
{
    public string? UserId => httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
    
    public string? UserRole =>  httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Role);
}