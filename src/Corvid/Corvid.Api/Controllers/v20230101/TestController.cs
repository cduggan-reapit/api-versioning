using Corvid.Api.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Corvid.Api.Controllers.v20230101;

public class TestController : CorvidControllerBase
{
    [HttpGet("more")]
    [IntroducedInVersion(2023, 1)]
    public IActionResult GetMore()
        => Ok("more 2023");
}