using ApiITTP.Data;
using ApiITTP.Features.DeleteUserFeature;
using ApiITTP.Models;
using FluentValidation;
using MediatR;

namespace ApiITTP.Features.RecoverUserFeature
{
    public class RecoverUserCommand : IRequest<bool>
    {
        public string Login { get; set; }

        public class RecoverUserCommandHandler : IRequestHandler<RecoverUserCommand, bool>
        {
            private IUserData data;


            public RecoverUserCommandHandler(IUserData data)
            {
                this.data = data ?? throw new ArgumentNullException(nameof(data));

            }

            public async Task<bool> Handle(RecoverUserCommand command, CancellationToken cancellationToken)
            {

                return await this.data.Recover(command.Login);
            }

            public class RecoverUserCommandValidator : AbstractValidator<RecoverUserCommand>
            {
                public RecoverUserCommandValidator()
                {


                    RuleFor(c => c.Login).NotEmpty().WithMessage("Login: Логин не существет");
                   
                    RuleFor(c => ((c.Login.Length > 0))).Equal(true).WithMessage("Login: Длина логина должна быть болбше 0");

                }




            }
        }

    }
}
