using Hassecore.API.Business.DTOs.UserPairing;
using MediatR;

namespace Hassecore.API.Business.MediatR.UserPairing
{
    public class PairedUserQuery : IRequest<PairedUserResponseDto?>
    {
        public Guid QueryingUserId { get; set; }
    }
}
