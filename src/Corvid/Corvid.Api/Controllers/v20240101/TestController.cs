using Corvid.Api.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Corvid.Api.Controllers.v20240101;

[IntroducedInVersion(2024, 1)]
public class TestController : CorvidControllerBase
{
    [HttpGet]
    public IActionResult Get()
        => Ok(2024);
}