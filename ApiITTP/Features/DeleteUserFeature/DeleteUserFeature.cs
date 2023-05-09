using ApiITTP.Data;
using ApiITTP.Features.ChangeUserFeature;
using ApiITTP.Models;
using FluentValidation;
using MediatR;
using System.Security.Claims;

namespace ApiITTP.Features.DeleteUserFeature
{
    public class DeleteUserCommand : IRequest<bool>
    {
        public string Login { get; set; }
        public string Type { get; set; }
        

        public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, bool>
        {
            private IUserData data;
            IHttpContextAccessor context;

            public DeleteUserCommandHandler(IUserData data, IHttpContextAccessor context)
            {
                this.data = data ?? throw new ArgumentNullException(nameof(data));
                this.context = context;
            }

            public async Task<bool> Handle(DeleteUserCommand command, CancellationToken cancellationToken)
            {
                return await this.data.DeleteUser(command.Login, command.Type, this.context.HttpContext.User.FindFirst(ClaimsIdentity.DefaultNameClaimType).Value);


            }


        }
        public class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommand>
        {
            public DeleteUserCommandValidator()
            {


                RuleFor(c => c.Login).NotEmpty().WithMessage("Login: Логин не существет");
                RuleFor(c => c.Type).NotEmpty().WithMessage("Type: Типа не существет");
                
                RuleFor(c => ((c.Type.Length > 0))).Equal(true).WithMessage("Type: Длина типа  должна быть болбше 0");
                RuleFor(c => ((c.Login.Length > 0))).Equal(true).WithMessage("Login: Длина логина должна быть болбше 0");

            }




        }
    }
}
