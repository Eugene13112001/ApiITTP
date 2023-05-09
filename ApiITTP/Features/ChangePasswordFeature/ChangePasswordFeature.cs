using ApiITTP.Data;
using ApiITTP.Features.ChangeLoginFeature;
using ApiITTP.Models;
using FluentValidation;
using MediatR;
using System.Security.Claims;

namespace ApiITTP.Features.ChangePasswordFeature
{
    public class ChangePasswordCommand : IRequest<bool>
    {
        public string Login { get; set; }

        public string NewPassword { get; set; }

        public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, bool>
        {
            private IUserData data;
            IHttpContextAccessor context;

            public ChangePasswordCommandHandler(IUserData data, IHttpContextAccessor context)
            {
                this.data = data ?? throw new ArgumentNullException(nameof(data));
                this.context = context;
            }

            public async Task<bool> Handle(ChangePasswordCommand command, CancellationToken cancellationToken)
            {
                
                return await this.data.ChangePassword(command.Login, command.NewPassword, this.context.HttpContext.User.FindFirst(ClaimsIdentity.DefaultNameClaimType).Value);
            }


        }
        public class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand>
        {
            public ChangePasswordCommandValidator()
            {

                RuleFor(c => c.Login).NotEmpty().WithMessage("Login: Логин не существет");
                RuleFor(c => ((c.Login.Length > 0))).Equal(true).WithMessage("Login: Длина  логина должна быть болбше 0");
                RuleFor(c => c.NewPassword).NotEmpty().WithMessage("NewPassword: Новый пароль не существет");
                RuleFor(c => ((c.NewPassword.Length > 0))).Equal(true).WithMessage("NewPassword: Длина нового пароля должна быть болбше 0");

            }




        }
    }

}
