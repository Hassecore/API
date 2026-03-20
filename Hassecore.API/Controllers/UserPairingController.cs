using Hassecore.API.Business.DTOs.UserPairing;
using Hassecore.API.Business.MediatR.UserPairing;
using Hassecore.API.Data.Entities.UserPairing;
using Hassecore.API.Data.Repositories;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Hassecore.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class UserPairingController : ControllerBase
    {
        private readonly IMediator _mediator;

        private readonly IBaseRepository _baseRepository;

        private readonly ILogger<UserPairingController> _logger;

        public UserPairingController(IMediator mediator,
                                     IBaseRepository repository,
                                     ILogger<UserPairingController> logger)
        {
            _mediator = mediator;
            _baseRepository = repository;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Ping()
        {
            return Ok();
        }

        [HttpPost]
        [Route("request-pairing")]
        public async Task<IActionResult> RequestPairingAsync([FromBody] RequestUserPairingDto request)
        {
            string userExternalId = HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
            var userId = (await _baseRepository.GetSingleAsync<User>(x => x.ExternalId == userExternalId)).Id;

            var command = new RequestPairingCommand
            {
                ReceiverEmail = request.ReceiverEmail,
                SenderId = userId
            };
            var userPairingRequestWasCreated = await _mediator.Send(command);

            if (!userPairingRequestWasCreated)
            {
                return NotFound("No such (unpaired) user was found.");
            }

            return Ok();
        }

        [HttpDelete]
        [Route("revoke-request")]
        public async Task<IActionResult> RevokeRequestAsync()
        {
            string userExternalId = HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
            var userId = (await _baseRepository.GetSingleAsync<User>(x => x.ExternalId == userExternalId)).Id;

            var command = new RevokeRequestCommand
            {
                RevokingUserId = userId
            };
            var userPairingRequestWasDeleted = await _mediator.Send(command);

            if (!userPairingRequestWasDeleted)
            {
                return NotFound();
            }

            return Ok();
        }

        [HttpPost]
        [Route("accept-pairing")]
        public async Task<IActionResult> AcceptPairingAsync()
        {
            string userExternalId = HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
            var userId = (await _baseRepository.GetSingleAsync<User>(x => x.ExternalId == userExternalId)).Id;
            var command = new AcceptPairingCommand
            {
                AcceptingUserId = userId
            };

            var result = await _mediator.Send(command);

            if (!result)
            {
                return BadRequest();
            }

            return Ok();
        }

        [HttpDelete]
        [Route("deny-pairing")]
        public async Task<IActionResult> DenyPairingAsync()
        {
            string userExternalId = HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
            var userId = (await _baseRepository.GetSingleAsync<User>(x => x.ExternalId == userExternalId)).Id;
            var command = new DenyPairingCommand
            {
                DenyingUserId = userId
            };

            var result = await _mediator.Send(command);

            if (!result)
            {
                return BadRequest();
            }

            return Ok();
        }

        [HttpGet]
        [Route("paired-user")]
        public async Task<IActionResult> GetUserPairAsync()
        {
            string userExternalId = HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
            var userId = (await _baseRepository.GetSingleAsync<User>(x => x.ExternalId == userExternalId)).Id;
            var pairedUserDto = await _mediator.Send(new PairedUserQuery
            {
                QueryingUserId = userId,
            });

            if (pairedUserDto == null)
            {
                return NotFound();
            }

            return Ok(pairedUserDto);
        }

        [HttpGet]
        [Route("pending-pairing")]
        public async Task<IActionResult> GetPendingPairingAsync()
        {
            string userExternalId = HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
            var userId = (await _baseRepository.GetSingleAsync<User>(x => x.ExternalId == userExternalId)).Id;
            var pairedUserDto = await _mediator.Send(new PendingPairingQuery
            {
                QueryingUserId = userId,
            });

            if (pairedUserDto == null)
            {
                return NotFound();
            }

            return Ok(pairedUserDto);
        }
    }
}
