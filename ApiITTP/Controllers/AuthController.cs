using ApiITTP.Data;
using ApiITTP.Filtrs;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiITTP.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IUserData userdata;
       

        public AuthController(IUserData userdata)
        {
            this.userdata = userdata;
          

        }
        /// <summary>
        /// Авторизация пользователя
        /// </summary>
        ///  <remarks>
        /// Параметры:
        ///  
        /// Login - логин пользователя (Eugene)
        ///  
        /// Password - пароль пользователя (12345)
        ///       
        /// 
        /// Возвращает токен
        ///</remarks>
        [HttpGet]
        [TypeFilter(typeof(SampleExceptionFilter))]
        [Route("api/Authenificate/{username}&{password}")]
        public async Task<IActionResult> GetToken(string username, string password)
        {
            var identity = await this.userdata.GetIdentity(username, password);
            if (identity == null) return BadRequest(new { errorText = "Invalid username or password." });


            var jwt = await this.userdata.GetToken(identity);
            Response.Headers.Add("x-access-token", jwt);

            return new JsonResult(jwt);
        }
    }
}
