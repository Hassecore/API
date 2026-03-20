using Hassecore.API.Business.DTOs.UserPairing;
using Hassecore.API.Business.Managers.UserPairing;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hassecore.API.Business.MediatR.UserPairing
{
    public class PendingPairingQueryHandler : IRequestHandler<PendingPairingQuery, PendingPairingResponseDto?>
    {
        private readonly IUserPairingService _userPairingService;
        public PendingPairingQueryHandler(IUserPairingService userPairingService)
        {
            _userPairingService = userPairingService;
        }

        public async Task<PendingPairingResponseDto?> Handle(PendingPairingQuery request, CancellationToken cancellationToken)
        {
            var userPairingRequest = await _userPairingService.GetUserPairingRequestAsync(request.QueryingUserId);

            if (userPairingRequest == null)
            {
                return null;
            }

            return new PendingPairingResponseDto
            {
                RequestingUserEmail = userPairingRequest.Sender!.Email,
                AuthenticatedUserIsReceiver = request.QueryingUserId == userPairingRequest.ReceiverId,
            };
        }
    }
}
