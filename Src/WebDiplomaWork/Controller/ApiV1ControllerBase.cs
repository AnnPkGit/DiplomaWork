using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebDiplomaWork.Filters;

namespace WebDiplomaWork.Controller;

[ApiController]
[ApiExceptionFilter]
[Route("api/v1/[controller]")]
public abstract class ApiV1ControllerBase : ControllerBase
{
    private ISender? _mediator;

    protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();
}