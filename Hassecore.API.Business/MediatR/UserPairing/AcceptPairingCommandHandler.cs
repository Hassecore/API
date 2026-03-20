using Hassecore.API.Business.Managers.UserPairing;
using MediatR;

namespace Hassecore.API.Business.MediatR.UserPairing
{
    internal class AcceptPairingCommandHandler : IRequestHandler<AcceptPairingCommand, bool>
    {
        private readonly IUserPairingService _userPairingService;
        public AcceptPairingCommandHandler(IUserPairingService userPairingService)
        {
            _userPairingService = userPairingService;
        }

        public async Task<bool> Handle(AcceptPairingCommand request, CancellationToken cancellationToken)
        {
            return await _userPairingService.AcceptUserPairingRequestAsync(request.AcceptingUserId);
        }
    }
}
