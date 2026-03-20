using Hassecore.API.Data.Context.CurrentUserContext;
using Hassecore.API.Data.Entities.UserPairing;
using Hassecore.API.Data.Repositories;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Hassecore.API.Middleware
{
    public class UserHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public UserHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IBaseRepository repository, CurrentUserContext currentUserContext)
        {
            // Add user handling logic here
            //var test = context.User.Identity.Name;
            //var test2 = repository.GetAsync<User>(Guid.NewGuid());
            var userUniqueId = context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userUniqueId != null)
            {
                var existingUser = (await repository.GetSingleOrDefaultAsync<User>(x => x.ExternalId == userUniqueId));
                if (existingUser != null)
                {
                    PopulateCurrentUserContext(repository, currentUserContext, existingUser);
                }
                else
                {
                    var createdUser = await CreateUser(context, repository);
                    if (createdUser == null)
                    {
                        return;
                    }

                    PopulateCurrentUserContext(repository, currentUserContext, createdUser);
                }

            }
            await _next(context);
        }

        private void PopulateCurrentUserContext(IBaseRepository repository, CurrentUserContext currentUserContext, User currentUser)
        {
            //currentUserContext.Id = currentUser.Id;
            //currentUserContext.ExternalId = currentUser.ExternalId;
            //currentUserContext.Username = currentUser.Username;
            //currentUserContext.Email = currentUser.Email;

            Guid? pairedUserId = null;

            var userPair = repository.GetSingleOrDefaultAsync<UserPair>(x => x.User1Id == currentUser.Id || x.User2Id == currentUser.Id).Result;
            if (userPair != null)
            {
                pairedUserId = currentUser.Id == userPair.User1Id ? userPair.User2Id : userPair.User1Id;
            }

            currentUserContext.Initialize(currentUser.Id, currentUser.ExternalId, currentUser.Username, currentUser.Email, pairedUserId);
        }

        private async Task<User?> CreateUser(HttpContext context, IBaseRepository repository)
        {
            var timeNow = DateTime.UtcNow;
            var externalId = context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var username = context.User?.FindFirst(JwtRegisteredClaimNames.Name)?.Value;
            var email = context.User?.FindFirst(ClaimTypes.Email)?.Value;

            if (!UserDataIsValid(externalId, username, email))
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                return null;
            }

            var user = new User
            {
                Id = Guid.NewGuid(),
                ExternalId = externalId,
                Username = username,
                Email = email,
                CreatedAt = timeNow,
                UpdatedAt = timeNow,
                LastOnline = DateOnly.FromDateTime(timeNow)
            };

            await repository.CreateAsync(user);

            return user;
        }

        private bool UserDataIsValid(string? externalId, string? userName, string? email)
        {
            if (string.IsNullOrEmpty(externalId) ||
                string.IsNullOrEmpty(userName) ||
                string.IsNullOrEmpty(email))
            {
                return false;
            }

            return true;
        }
    }
}
