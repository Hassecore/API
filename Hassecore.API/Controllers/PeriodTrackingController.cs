using Hassecore.API.Business.DTOs.PeriodTracking;
using Hassecore.API.Business.MediatR.PeriodTracking.Commands;
using Hassecore.API.Business.MediatR.PeriodTracking.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hassecore.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class PeriodTrackingController : ControllerBase
    {
        private readonly IMediator _mediator;

        private readonly ILogger<PeriodTrackingController> _logger;
    
        public PeriodTrackingController(IMediator mediator,
                                        ILogger<PeriodTrackingController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }
    
        [HttpPost]
        public async Task<IActionResult> CreatePeriodEventAsync([FromBody] CreatePeriodEventDto request)
        {
            //string userExternalId = HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
            //var userId = (await _baseRepository.GetSingleAsync<User>(x => x.ExternalId == userExternalId)).Id;

            var command = new CreatePeriodEventCommand
            {
                EventDate = request.EventDate,
                EventType = request.EventType,
            };
            var userPairingRequestWasCreated = await _mediator.Send(command);

            if (!userPairingRequestWasCreated)
            {
                return BadRequest();
            }

            return Created();
        }

        [HttpGet]
        public async Task<IActionResult> GetPeriodEvents()
        {
            var periodEvents = await _mediator.Send(new GetPeriodEventsQuery());

            return Ok(periodEvents);
        }


        [HttpPut]
        public async Task<IActionResult> UpdatePeriodEventDate([FromBody] UpdatePeriodEventDateDto request)
        {
            throw new NotImplementedException();
        }

        [HttpDelete]
        //[Route("request-pairing")]
        [Route("{id}")]
        public async Task<IActionResult> DeletePeriodEvent(Guid id)
        {
            await _mediator.Send(new DeletePeriodEventCommand { PeriodEventId = id });

            return NoContent();
        }


    }
}
