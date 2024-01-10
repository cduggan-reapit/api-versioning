using Corvid.Api.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Corvid.Api.Controllers.Four;

public class TestController : ApiControllerBase
{
    /// <summary>
    /// Endpoint that should only be available from "2023-01-01" (inclusive) to "2023-02-01" (exclusive)
    /// </summary>
    /// <returns></returns>
    [HttpGet("four_only")]
    [ProducesResponseType<string>(200)]
    [IntroducedInVersion("2023-01-01")]
    [RemovedInVersion("2024-01-11")]
    public IActionResult GetOneOnly()
        => Ok("2020-01-01 (inclusive) to 2021-01-01 (exclusive)");
}