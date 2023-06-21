using DigitalDrawer.Application.Common.Exeptions;
using DigitalDrawer.Application.Features.ApiKey.Commands.CreateApiKeyCommand;
using DigitalDrawer.Application.Features.Conversion.Commands.ConversionCancelDeleteCommand;
using DigitalDrawer.Application.Features.Conversion.Commands.ConversionHardDeleteCommand;
using DigitalDrawer.Application.Features.Conversion.Commands.ConversionSoftDeleteCommand;
using DigitalDrawer.Application.Features.Conversion.Commands.CreateConversionCommand;
using DigitalDrawer.Application.Features.Conversion.Commands.CreateConversionTaskCommand;
using DigitalDrawer.Application.Features.Conversion.Queries.GetConversionFileQuery;
using DigitalDrawer.Application.Features.Conversion.Queries.GetConversionsQuery;
using DigitalDrawer.Application.Features.Conversion.Queries.GetConversionsResultsQuery;
using Hangfire;
using Hangfire.MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.IO;
using System.IO.Pipes;
using System.Net;

namespace DigitalDrawer.WebAPI.Controllers
{
    //[Authorize]
    [ApiController]
    public class ConversionController : ApiControllerBase
    {
        const string CONTENT_TYPE= "application/octet-stream";
        private Action<string> cleanup = (string jobId) => _ = BackgroundJob.Delete(jobId);
        
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(Guid))]
        public async Task<ActionResult> CreateConvertionTask([FromBody] IEnumerable<CreateConversionCommand> commands)
        {
            try
            {
                Guid taskId = await Mediator.Send(new CreateConversionTaskCommand());
                foreach (var command in commands)
                {
                    command.ConvertionTaskId = taskId;
                    Mediator.Enqueue($"File convertion task {taskId}", command);
                }
                return CreatedAtAction(nameof(GetConvertionsByTaskId), new { id = taskId }, taskId);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("[action]/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerResponse(StatusCodes.Status200OK, "File", typeof(FileResult))]
        public async Task<ActionResult> Download([FromRoute] Guid id)
        {
            try
            {
                var response = await Mediator.Send(new GetConversionFileQuery() { Id = id });
                //return File(response.ConvertedFileContent, CONTENT_TYPE, response.ConvertedFileName);
                return new FileStreamResult(response.ConvertedFileContent, CONTENT_TYPE)
                {
                    FileDownloadName = response.ConvertedFileName
                };
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("ConvertionResults/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(IEnumerable<GetConversionQueryDto>))]
        public async Task<ActionResult> GetConvertionsByTaskId([FromRoute] Guid id)
        {
            return Ok(await Mediator.Send(new GetConversionResultsQuery() { ConvertionTaskId = id }));
        }
        
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(IEnumerable<GetConversionQueryDto>))]
        public async Task<ActionResult> GetConvertions([FromQuery] GetConversionsQuery query)
        {
            return Ok(await Mediator.Send(query));
        }

        [HttpDelete("[action]/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> SoftDelete([FromRoute] Guid id)
        {
            try
            {
                var deletionDate = DateTime.Now.AddDays(30);
                var sheduled = TimeSpan.FromDays(30);
                
                string jobId = Mediator.Schedule("FileDeletion", new ConversionHardDeleteCommand() { Id = id }, sheduled);

                await Mediator.Send(new ConversionSoftDeleteCommand() { Id = id, DeletionDateTime = deletionDate, DeletionJobId = jobId });
                
                return NoContent();
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
        
        [HttpDelete("[action]/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> HardDelete([FromRoute] Guid id)
        {
            try
            {
                await Mediator.Send(new ConversionHardDeleteCommand() { Id = id, CleanUp = cleanup });
                return NoContent();
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            
        }
        [HttpPatch("[action]/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> CancelDelete([FromRoute] Guid id)
        {
            try
            {
                await Mediator.Send(new ConversionCancelDeleteCommand() { Id = id, CleanUp = cleanup });
                return NoContent();
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

    }
}
