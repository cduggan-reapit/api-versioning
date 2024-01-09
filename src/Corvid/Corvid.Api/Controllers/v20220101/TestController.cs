using Corvid.Api.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Corvid.Api.Controllers.v20220101;

[IntroducedInVersion("2022-12-31")]
public class TestController : CorvidControllerBase
{
    [HttpGet("original")]
    public IActionResult GetOriginal()
        => Ok("Original 2023");
    
    [HttpGet]
    [IntroducedInVersion(2022, 1)]
    [RemovedInVersion(2024, 1)]
    public IActionResult Get()
        => Ok(2022);
    
    
    [HttpGet("more")]
    [IntroducedInVersion(2022, 1)]
    [RemovedInVersion(2023, 1)]
    public IActionResult GetMost()
        => Ok("most 2022");

    private void DummyMethod()
    {
        
    }
}