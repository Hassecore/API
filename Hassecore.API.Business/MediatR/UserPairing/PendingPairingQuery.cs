using Hassecore.API.Business.DTOs.UserPairing;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hassecore.API.Business.MediatR.UserPairing
{
    public class PendingPairingQuery : IRequest<PendingPairingResponseDto?>
    {
        public Guid QueryingUserId { get; set; }
    }
}
