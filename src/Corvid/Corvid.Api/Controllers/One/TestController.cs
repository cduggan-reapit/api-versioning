using Corvid.Api.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Corvid.Api.Controllers.One;

[IntroducedInVersion(2020, 1, 1)]
public class TestController : ApiControllerBase
{
    /// <summary>
    /// Endpoint that should only be available from "2020-01-01" (inclusive) to "2021-01-01" (exclusive)
    /// </summary>
    /// <returns></returns>
    [HttpGet("one_only")]
    [ProducesResponseType<string>(200)]
    [RemovedInVersion("2021-01-01")]
    public IActionResult GetOneOnly()
        => Ok("2020-01-01 (inclusive) to 2021-01-01 (exclusive)");
    
    [HttpGet("one_two_three")]
    [ProducesResponseType<string>(200)]
    [RemovedInVersion("2023-01-01")]
    public IActionResult GetOneToThree()
        => Ok("2020-01-01 (inclusive) to 2023-01-01 (exclusive)");
    
    /// <summary>
    /// Endpoint that should only be available from "2020-01-01" (inclusive)
    /// </summary>
    /// <returns></returns>
    [HttpGet("all_versions_from_one")]
    [ProducesResponseType<string>(200)]
    public IActionResult GetAllVersions()
        => Ok("2020-01-01 (inclusive)");
}