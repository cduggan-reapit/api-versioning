using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace Corvid.Api.Controllers.v1_0;

[ApiVersion("2023-01-01")]
public class TestController : CorvidControllerBase
{
    [HttpGet]
    public IActionResult Get()
        => Ok(2023);
}