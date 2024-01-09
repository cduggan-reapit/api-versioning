using Corvid.Api.Attributes;
using Corvid.Api.Controllers.v20230228.RequestModels;
using Corvid.Api.Controllers.v20230228.ResponseModels;
using Microsoft.AspNetCore.Mvc;

namespace Corvid.Api.Controllers.v20230228;

[IntroducedInVersion("2023-02-28")]
public class TestController : ApiControllerBase
{
    [HttpPost]
    public IActionResult PostSomething([FromBody] CreateTestRequestModel requestModel)
        => Ok(new CommandResultResponseModel("Created", Guid.NewGuid()));
    
    [HttpPut]
    [RemovedInVersion(2024, 1, 9)]
    public IActionResult PutSomething([FromBody] CreateTestRequestModel requestModel)
        => Ok(new CommandResultResponseModel("Updated", Guid.NewGuid()));
}