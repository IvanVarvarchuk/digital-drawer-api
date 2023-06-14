using DigitalDrawer.Application.Common.Exeptions;
using DigitalDrawer.Application.Features.Conversion.Commands.CreateConversionCommand;
using DigitalDrawer.Application.Features.Conversion.Queries.GetConversionFileQuery;
using DigitalDrawer.Application.Features.Conversion.Queries.GetConversionsQuery;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace DigitalDrawer.WebAPI.Controllers
{
    [ApiController]
    public class ConversionController : ApiControllerBase
    {
        const string CONTENT_TYPE= "application/octet-stream";
        [HttpPost]
        [Authorize]
        public async Task<ActionResult> CreateConvertion([FromBody] CreateConversionCommand command)
        {
            var response = await Mediator.Send(command);
            return File(response.ConvertedFileContent, CONTENT_TYPE, response.FileName);

            //return Ok(await Mediator.Send(command));
        }

        [HttpGet("[action]/{id}")]
        public async Task<ActionResult> GetConvertion([FromRoute] Guid id)
        {
            try
            {
                var response = await Mediator.Send(new GetConversionFileQuery() { Id = id });
                return File(response.ConvertedFileContent, CONTENT_TYPE, response.ConvertedFileName);
            }
            catch (NotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<ActionResult> GetConvertions([FromQuery] GetConversionsQuery query)
        {
            return Ok(await Mediator.Send(query));
        }
    }
}
