using Hassecore.API.Business.DTOs.UserPairing;
using Hassecore.API.Business.MediatR.UserPairing.Queries;
using Hassecore.API.Data.Context.CurrentUserContext;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hassecore.API.Business.MediatR.UserPairing.QueryHandlers
{
    public class UserPairQueryHandler : IRequestHandler<UserPairQuery, UserPairDto>
    {
        private readonly CurrentUserContext _currentUserContext;
        public UserPairQueryHandler()
        {
            
        }
        public async Task<UserPairDto> Handle(UserPairQuery request, CancellationToken cancellationToken)
        {



            throw new NotImplementedException();
        }
    }
}
