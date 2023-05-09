using ApiITTP.Data;
using ApiITTP.Features.GetUserByLoginAndPasword;
using ApiITTP.Models;
using FluentValidation;
using MediatR;
using ApiITTP.ViewModels;

namespace ApiITTP.Features.GetUserByLoginFeature
{
    public class GetUserByLoginCommand : IRequest<UserView?>
    {
        public string Login { get; set; }
     


        public class GetUserByLoginCommandHandler : IRequestHandler<GetUserByLoginCommand, UserView?>
        {
            private IUserData data;


            public GetUserByLoginCommandHandler(IUserData data)
            {
                this.data = data ?? throw new ArgumentNullException(nameof(data));

            }

            public async Task<UserView?> Handle(GetUserByLoginCommand command, CancellationToken cancellationToken)
            {
                User? user = await this.data.GetByLogin(command.Login);
                if (user == null) throw new Exception("Такого пользователя нет");
                return new UserView {Name= user.Name,  Gender = user.Gender, Birthday = user.Birthday,
                Active = ( user.RevokedBy == null)?true:false
                };


            }


        }
        public class GetUserByLoginCommandValidator : AbstractValidator<GetUserByLoginCommand>
        {
            public GetUserByLoginCommandValidator()
            {
                

                RuleFor(c => c.Login).NotEmpty().WithMessage("Login: Логин не существет");

                RuleFor(c => ((c.Login.Length > 0))).Equal(true).WithMessage("Login: Длина логина должна быть болбше 0");

            }




        }
    }
}
