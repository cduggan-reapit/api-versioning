using Corvid.Api.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Corvid.Api.Controllers.Two;

[IntroducedInVersion(2021, 1, 1)]
public class TestController : ApiControllerBase
{
    /// <summary>
    /// Endpoint that should only be available from "2021-01-01" (inclusive) to "2022-01-01" (exclusive)
    /// </summary>
    /// <returns></returns>
    [HttpGet("two_only")]
    [ProducesResponseType<string>(200)]
    [RemovedInVersion("2022-01-01")]
    public IActionResult GetOneOnly()
        => Ok("2021-01-01 (inclusive) to 2022-01-01 (exclusive)");
    
    /// <summary>
    /// Endpoint that should only be available from "2020-01-01" (inclusive)
    /// </summary>
    /// <returns></returns>
    [HttpGet("all_versions_from_two")]
    [ProducesResponseType<string>(200)]
    public IActionResult GetAllVersions()
        => Ok("2021-01-01 (inclusive)");
}