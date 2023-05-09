using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Newtonsoft.Json.Linq;
using System.Security.Cryptography;

namespace ApiITTP.Filtrs
{
    public interface IStrategy
    {
        public BadRequestObjectResult? Algorithm(HttpContext context);
    }

    public class AdminOnly : IStrategy
    {
        public BadRequestObjectResult? Algorithm(HttpContext context)
        {
           var name = context.User.FindFirst(ClaimsIdentity.DefaultNameClaimType);
           if (name == null) return new BadRequestObjectResult("Пользователь не авторизован");
           var val =  context.User.FindFirst(ClaimsIdentity.DefaultRoleClaimType);
           if (val == null) return new BadRequestObjectResult("Пользователь не авторизован");
           if (val.Value == "Admin") return null;
           return new BadRequestObjectResult("Доступно только администраторам");
        }
    }

    public class UsersOnlyGet : IStrategy
    {
        public BadRequestObjectResult? Algorithm(HttpContext context)
        {
            var name = context.User.FindFirst(ClaimsIdentity.DefaultNameClaimType);
            if (name == null) return new BadRequestObjectResult("Пользователь не авторизован");
            var val = context.User.FindFirst(ClaimsIdentity.DefaultRoleClaimType);
            if (val == null) return new BadRequestObjectResult("Пользователь не авторизован");
            if (!context.Request.RouteValues.ContainsKey("login")) return new BadRequestObjectResult("Запрос не содержит логина");
            string login = context.Request.RouteValues["login"].ToString();
            if (login == name.Value) return null;
            return new BadRequestObjectResult("Не доступно этому пользователю");
        }
    }
    public class ConcreteStrategy : IStrategy
    {
        public BadRequestObjectResult? Algorithm(HttpContext context)
        {
            var name = context.User.FindFirst(ClaimsIdentity.DefaultNameClaimType);
            if (name == null) return new BadRequestObjectResult("Пользователь не авторизован");
            var val = context.User.FindFirst(ClaimsIdentity.DefaultRoleClaimType);
            if (val == null) return new BadRequestObjectResult("Пользователь не авторизован");
            if (val.Value == "Admin") return null;
            if (!context.Request.Form.ContainsKey("Login")) return new BadRequestObjectResult("Запрос не содержит логина");
            if (context.Request.Form["Login"] == name.Value) return null;
            return new BadRequestObjectResult("Пользователь не имеет прав на этот запрос");
        }
    }

    public class MyAttribute: Attribute, IAuthorizationFilter
    {
        IStrategy strategy;
        public MyAttribute(string type)
        {
            if (type == "OnlyAdmin") strategy = new AdminOnly();
            if (type == "UsersOnly") strategy = new UsersOnlyGet();
            if (type == "ConcreteStrategy") strategy = new ConcreteStrategy();
            
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var result = this.strategy.Algorithm(context.HttpContext);
            if (result != null) context.Result = result;
            
            
        }
    }
}
