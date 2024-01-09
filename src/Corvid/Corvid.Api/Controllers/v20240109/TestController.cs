using Corvid.Api.Attributes;
using Corvid.Api.Controllers.v20240109.RequestModels;
using Corvid.Api.Controllers.v20240109.ResponseModels;
using Microsoft.AspNetCore.Mvc;

namespace Corvid.Api.Controllers.v20240109;

[IntroducedInVersion("2024-01-09")]
public class TestController : ApiControllerBase
{
    [HttpPut]
    [IntroducedInVersion(2024, 1, 9)]
    public IActionResult PutSomething([FromBody] UpdateTestRequestModel requestModel)
        => Ok(new CommandResultResponseModel($"Updated ({requestModel.Count})", Guid.NewGuid()));
}
