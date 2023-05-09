using ApiITTP.Data;
using ApiITTP.Features.ChangeUserFeature;
using FluentValidation;
using MediatR;
using ApiITTP.Models;
using ApiITTP.ViewModels;
using System.Reflection;
using System.Xml.Linq;
using System;

namespace ApiITTP.Features.GetUserByLoginAndPasword
{
    public class GetUserByLoginAndPasswordCommand : IRequest<UserView?>
    {
        public string Login { get; set; }
        public string Password { get; set; }

       

        public class GetUserByLoginAndPasswordCommandHandler : IRequestHandler<GetUserByLoginAndPasswordCommand, UserView?>
        {
            private IUserData data;
           

            public GetUserByLoginAndPasswordCommandHandler(IUserData data)
            {
                this.data = data ?? throw new ArgumentNullException(nameof(data));
               
            }

            public async Task<UserView?> Handle(GetUserByLoginAndPasswordCommand command, CancellationToken cancellationToken)
            {
                User? user = await this.data.Authentificate(command.Login, command.Password);
                if (user is null) throw new Exception("Такого пользователя нет");
                return new UserView
                {
                    Name = user.Name,
                    Gender = user.Gender,
                    Birthday = user.Birthday,
                    Active = (user.RevokedBy == null) ? true : false
                };
                


            }


        }
        public class GetUserByLoginAndPasswordCommandValidator : AbstractValidator<GetUserByLoginAndPasswordCommand>
        {
            public GetUserByLoginAndPasswordCommandValidator()
            {
                RuleFor(c => c.Password).NotEmpty().WithMessage("Password: Пароль не существет");

                RuleFor(c => ((c.Password.Length > 0))).Equal(true).WithMessage("Password: Длина пароля должна быть болбше 0");

                RuleFor(c => c.Login).NotEmpty().WithMessage("Login: Логин не существет");
               
                RuleFor(c => ((c.Login.Length > 0))).Equal(true).WithMessage("Login: Длина логина должна быть болбше 0");

            }




        }
    }
}
