using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace Corvid.Api.Controllers.v1_1;

[ApiVersion("2024-01-01")]
public class TestController : CorvidControllerBase
{
    [HttpGet]
    public IActionResult Get()
        => Ok(2024);
}