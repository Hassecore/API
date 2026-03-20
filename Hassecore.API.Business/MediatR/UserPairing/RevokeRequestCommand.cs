using MediatR;

namespace Hassecore.API.Business.MediatR.UserPairing
{
    public  class RevokeRequestCommand : IRequest<bool>
    {
        public Guid RevokingUserId { get; set; }
    }
}
