using Corvid.Api.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Corvid.Api.Controllers.v20230101;

[IntroducedInVersion(2023, 11)]
public class ExtraController : CorvidControllerBase
{
    [HttpGet]
    public IActionResult Get()
        => Ok("extra, extra, read all about it");
}