using Corvid.Api.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Corvid.Api.Controllers.Three;

[IntroducedInVersion(2021, 1, 1)]
[RemovedInVersion("2022-01-01")]
public class TestController : ApiControllerBase
{
    /// <summary>
    /// Endpoint that should only be available from "2022-01-01" (inclusive) to "2023-01-01" (exclusive)
    /// </summary>
    /// <returns></returns>
    [HttpGet("three_only")]
    [ProducesResponseType<string>(200)]
    public IActionResult GetOneOnly()
        => Ok("2021-01-01 (inclusive) to 2022-01-01 (exclusive)");
}