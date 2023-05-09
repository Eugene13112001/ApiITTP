using ApiITTP.Models;
using ApiITTP.Data;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace ApiITTP.Features.AddUserFeature
{
    public class AddUserCommand : IRequest<User>
    {


        public string Name { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }

        public int Gender { get; set; }

        public bool Admin { get; set; }

        public DateTime? DateBirth { get; set; }

        public class AddUserCommandHandler : IRequestHandler<AddUserCommand, User>
        {
            private IUserData data;
            IHttpContextAccessor context;

            public AddUserCommandHandler(IUserData data , IHttpContextAccessor context)
            {
                this.data = data ?? throw new ArgumentNullException(nameof(data));
                this.context = context;
            }

            public async Task<User> Handle(AddUserCommand command, CancellationToken cancellationToken)
            {
                User? user = await this.data.GetByLogin(command.Login);
                if (user != null) throw new Exception("Пользователь с таким догином уже есть");
                user =  new User {Name = command.Name, Login = command.Login, Password = command.Password,
                Admin = command.Admin, CreatedOn = DateTime.Today, CreatedBy = this.context.HttpContext.User.FindFirst(ClaimsIdentity.DefaultNameClaimType).Value,
                  Gender = command.Gender,   Birthday = command.DateBirth};
                await this.data.AddUser(user);
                return user;


            }


        }
        public class AddUserCommandValidator : AbstractValidator<AddUserCommand>
        {
            private bool DateCheck(DateTime? date)
            {
                if (date is null) return true;
                return ((date < DateTime.Today));
            }
            public AddUserCommandValidator()
            {

                RuleFor(c => c.Name).NotEmpty().WithMessage("Name: Имя не существет");
                RuleFor(c => c.Login).NotEmpty().WithMessage("Login: Логин не существет");
                RuleFor(c => c.Password).NotEmpty().WithMessage("Password: Пароль не существет");
                
                RuleFor(c => c.Admin).NotEmpty().WithMessage("Admin: Роль не существет");
                RuleFor(c => ((c.Gender==1)||(c.Gender==0) || (c.Gender ==2))).Equal(true).WithMessage("Gender: Такого гендера нет");
                RuleFor(c => DateCheck(c.DateBirth)).Equal(true).WithMessage("DateBirth: Дата рождения не может быть больше сегодняшней даты ");
                RuleFor(c => ((c.Name.Length > 0))).Equal(true).WithMessage("Name: Длина имени должна быть болбше 0");
                RuleFor(c => ((c.Login.Length > 0))).Equal(true).WithMessage("Login: Длина логина должна быть болбше 0");
                RuleFor(c => ((c.Password.Length > 0))).Equal(true).WithMessage("Password: Длина пароля должна быть болбше 0");
            }




        }

    }
}
