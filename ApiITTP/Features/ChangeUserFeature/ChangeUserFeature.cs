using ApiITTP.Data;
using ApiITTP.Features.AddUserFeature;
using ApiITTP.Models;
using FluentValidation;
using MediatR;
using System.Security.Claims;

namespace ApiITTP.Features.ChangeUserFeature
{
    public class ChangeUserCommand : IRequest<bool>
    {
       
        public string Login { get; set; }
        public string NewName { get; set; }

        public int NewGender { get; set; }

        public DateTime? NewDateBirth { get; set; }

        public class ChangeUserCommandHandler : IRequestHandler<ChangeUserCommand, bool>
        {
            private IUserData data;
            IHttpContextAccessor context;

            public ChangeUserCommandHandler(IUserData data, IHttpContextAccessor context)
            {
                this.data = data ?? throw new ArgumentNullException(nameof(data));
                this.context = context;
            }

            public async Task<bool> Handle(ChangeUserCommand command, CancellationToken cancellationToken)
            {
                return await this.data.ChangeUser(command.Login, command.NewGender, command.NewName, command.NewDateBirth, this.context.HttpContext.User.FindFirst(ClaimsIdentity.DefaultNameClaimType).Value);


            }


        }
        public class ChangeUserCommandValidator : AbstractValidator<ChangeUserCommand>
        {
            private bool DateCheck(DateTime? date)
            {
                if (date is null) return true;
                return ((date < DateTime.Today));
            }
            public ChangeUserCommandValidator()
            {

                
                RuleFor(c => c.Login).NotEmpty().WithMessage("Login: Логин не существет");
                RuleFor(c => c.NewName).NotEmpty().WithMessage("NewName: Имя не существет");
                RuleFor(c => c.NewGender).NotEmpty().WithMessage("NewGender: Такого гендера не существует не существет");
              
                RuleFor(c => ((c.NewGender == 1) || (c.NewGender == 0) || (c.NewGender == 2))).Equal(true).WithMessage("Gender: Такого гендера нет");
                RuleFor(c => DateCheck(c.NewDateBirth)).Equal(true).WithMessage("DateBirth: Дата рождения не может быть больше сегодняшней даты ");
                RuleFor(c => ((c.NewName.Length > 0))).Equal(true).WithMessage("Name: Длина имени должна быть болбше 0");
                RuleFor(c => ((c.Login.Length > 0))).Equal(true).WithMessage("Login: Длина логина должна быть болбше 0");
                
            }




        }
    }
}
