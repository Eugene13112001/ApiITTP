using ApiITTP.Data;
using ApiITTP.Features.GetUserByLoginAndPasword;
using ApiITTP.Models;
using FluentValidation;
using MediatR;

namespace ApiITTP.Features.GetOldUsers
{
    public class GetOldUsersCommand : IRequest<List<User>?>
    {
        public int Age { get; set; }

        public class GetOldUsersCommandHandler : IRequestHandler<GetOldUsersCommand, List<User>?>
        {
            private IUserData data;


            public GetOldUsersCommandHandler(IUserData data)
            {
                this.data = data ?? throw new ArgumentNullException(nameof(data));

            }

            public async Task<List<User>?> Handle(GetOldUsersCommand command, CancellationToken cancellationToken)
            {
                return await this.data.GetUsersByAge(command.Age);


            }


        }
        public class GetOldUsersCommandCommandValidator : AbstractValidator<GetOldUsersCommand>
        {
            public GetOldUsersCommandCommandValidator()
            {
                RuleFor(c => c.Age).NotEmpty().WithMessage("Age: Возраст не существет");

                RuleFor(c => ((c.Age > 0))).Equal(true).WithMessage("Age: Возраст должен быть болбше 0");


            }




        }
    }
}
