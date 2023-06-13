using DigitalDrawer.Application.Features.Conversion.Commands.CreateConversionCommand;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace DigitalDrawer.WebAPI.Controllers
{
    [ApiController]
    public class ConversionController : ApiControllerBase
    {
        [HttpPost]
        public async Task<ActionResult> CreateReservation([FromBody] CreateConversionCommand command)
        {
            var stream = (await Mediator.Send(command)).ConvertedFileContent;
            return new FileStreamResult(stream, "application/octet-stream");
            //return File((await Mediator.Send(command)).ConvertedFileContent, "image/png");
        }
    }
}
