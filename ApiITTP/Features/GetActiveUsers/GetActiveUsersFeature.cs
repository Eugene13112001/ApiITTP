using ApiITTP.Data;
using ApiITTP.Features.ChangePasswordFeature;
using FluentValidation;
using ApiITTP.Models;
using MediatR;

namespace ApiITTP.Features.GetActiveUsers
{
    public class GetActiveUsersCommand : IRequest<List<User>>
    {
       

        public class GetActiveUsersCommandHandler : IRequestHandler<GetActiveUsersCommand, List<User>>
        {
            private IUserData data;


            public GetActiveUsersCommandHandler(IUserData data)
            {
                this.data = data ?? throw new ArgumentNullException(nameof(data));

            }

            public async Task<List<User>> Handle(GetActiveUsersCommand command, CancellationToken cancellationToken)
            {

                return await this.data.GetActive();
            }


        }
        
    }
}
