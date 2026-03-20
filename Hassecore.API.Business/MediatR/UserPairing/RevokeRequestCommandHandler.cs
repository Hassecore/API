using Hassecore.API.Business.Managers.UserPairing;
using MediatR;

namespace Hassecore.API.Business.MediatR.UserPairing
{
    public class RevokeRequestCommandHandler : IRequestHandler<RevokeRequestCommand, bool>
    {
        private readonly IUserPairingService _userPairingService;
        public RevokeRequestCommandHandler(IUserPairingService userPairingService)
        {
            _userPairingService = userPairingService;
        }

        public async Task<bool> Handle(RevokeRequestCommand request, CancellationToken cancellationToken)
        {
            return await _userPairingService.RevokeUserPairingRequestAsync(request.RevokingUserId);
        }
    }
}
