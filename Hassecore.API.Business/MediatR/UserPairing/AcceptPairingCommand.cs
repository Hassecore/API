using MediatR;

namespace Hassecore.API.Business.MediatR.UserPairing
{
    public class AcceptPairingCommand : IRequest<bool>
    {
        public required Guid AcceptingUserId { get; set; }
    }
}
