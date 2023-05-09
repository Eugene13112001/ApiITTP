using ApiITTP.Data;
using ApiITTP.Features.AddUserFeature;
using ApiITTP.Models;
using FluentValidation;
using MediatR;
using System.Security.Claims;

namespace ApiITTP.Features.ChangeLoginFeature
{
    public class ChangeLoginCommand : IRequest<bool>
    {
        public string NewLogin { get; set; }

        public string Login { get; set; }

        public class ChangeLoginCommandHandler : IRequestHandler<ChangeLoginCommand, bool>
        {
            private IUserData data;
            IHttpContextAccessor context;

            public ChangeLoginCommandHandler(IUserData data, IHttpContextAccessor context)
            {
                this.data = data ?? throw new ArgumentNullException(nameof(data));
                this.context = context;
            }

            public async Task<bool> Handle(ChangeLoginCommand command, CancellationToken cancellationToken)
            {
                User? user = await this.data.GetByLogin(command.NewLogin);
                if (user != null) throw new Exception("Пользователь с таким логином уже существует");                
                return await this.data.ChangeLogin(command.Login, command.NewLogin, this.context.HttpContext.User.FindFirst(ClaimsIdentity.DefaultNameClaimType).Value);
            }


        }
        public class ChangeLoginCommandValidator : AbstractValidator<ChangeLoginCommand>
        {
            public ChangeLoginCommandValidator()
            {

                RuleFor(c => c.Login).NotEmpty().WithMessage("Login: Старый логин не существет");                              
                RuleFor(c => ((c.Login.Length > 0))).Equal(true).WithMessage("Login: Длина старого логина должна быть болбше 0");
                RuleFor(c => c.NewLogin).NotEmpty().WithMessage("NewLogin: Новый логин не существет");
                RuleFor(c => ((c.NewLogin.Length > 0))).Equal(true).WithMessage("NewLogin: Длина нового логина должна быть болбше 0");
               
            }




        }
    }
}
