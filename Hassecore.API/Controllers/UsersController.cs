using Hassecore.API.Business.DTOs.UserPairing;
using Hassecore.API.Data.Context.CurrentUserContext;
using Hassecore.API.Data.Entities.UserPairing;
using Hassecore.API.Data.Repositories;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hassecore.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IBaseRepository _baseRepository;

        private readonly CurrentUserContext _currentUserContext;

        private readonly ILogger<UsersController> _logger;

        public UsersController(
                               IBaseRepository repository,
                               CurrentUserContext currentUserContext,
                               ILogger<UsersController> logger)
        {
            _baseRepository = repository;
            _currentUserContext = currentUserContext;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsersAsync()
        {
            var users =  await _baseRepository.GetQueryable<User>(x => true).ToListAsync();
            
            _logger.LogInformation("Users retrieved successfully.");
            return Ok(users);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetUserAsync(Guid id)
        {
            var user = await _baseRepository.GetAsync<User>(id);

            if (user == null)
            {
                _logger.LogWarning("User with id {UserId} not found.", id);
                return NotFound();
            }
            else
            {
                _logger.LogInformation("User with id {UserId} retrieved successfully.", id);
                return Ok(user);
            }
        }

        [HttpGet]
        [Route("UserInfo")]
        public IActionResult GetUserInfoAsync(Guid id)
        {
            var userInfo = new UserPairDto
            {
                CurrentUserId = _currentUserContext.UserId,
                PairedUserid = _currentUserContext.PairedUserId
            };

            _logger.LogInformation("User info for user {UserId} retrieved successfully.", userInfo.CurrentUserId);
            return Ok(userInfo);
        }
    }
}
