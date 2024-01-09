using Corvid.Api.Attributes;
using Corvid.Api.Controllers.v20200131.RequestModels;
using Corvid.Api.Controllers.v20200131.ResponseModels;
using Microsoft.AspNetCore.Mvc;

namespace Corvid.Api.Controllers.v20200131;

[IntroducedInVersion(2020, 1, 31)]
public class TestController : ApiControllerBase
{
    [HttpGet("greeting")]
    [ProducesResponseType<ReadGreetingResponseModel>(200)]
    [RemovedInVersion("2023-01-01")]
    public IActionResult GetGreeting([FromQuery] ReadGreetingRequestModel requestModel)
        => Ok(new ReadGreetingResponseModel($"Hello {requestModel.Name}!"));
}