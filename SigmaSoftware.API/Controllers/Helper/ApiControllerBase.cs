using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace SigmaSoftware.API.Controllers.Helper;

/// <summary>
/// This Controller Base For Endpoints
/// </summary>
[ApiController]
[EnableCors("AllowedOrigins")]
[Route("api/[controller]")]
public class ApiControllerBase :  ControllerBase
{
    private ISender? _mediator;

    /// <summary>
    /// Mediator Service
    /// </summary>
    protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();
}