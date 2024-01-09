using Corvid.Api.Attributes;
using Corvid.Api.Controllers.v20221231.RequestModels;
using Corvid.Api.Controllers.v20221231.ResponseModels;
using Microsoft.AspNetCore.Mvc;

namespace Corvid.Api.Controllers.v20221231;

[IntroducedInVersion("2022-12-31")]
[RemovedInVersion("2023-02-28")]
public class TestController : ApiControllerBase
{
    [HttpGet("farewell")]
    [ProducesResponseType<ReadFarewellResponseModel>(200)]
    public IActionResult GetGreeting([FromQuery] ReadFarewellRequestModel requestModel)
        => Ok(new ReadFarewellResponseModel($"Bye {requestModel.Name}!"));
}