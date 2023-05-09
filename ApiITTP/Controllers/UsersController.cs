using Microsoft.AspNetCore.Mvc;
using ApiITTP.Data;
using ApiITTP.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using ApiITTP.Filtrs;
using ApiITTP.Features.ChangePasswordFeature;
using ApiITTP.Features.AddUserFeature;
using MediatR;
using ApiITTP.Features.ChangeUserFeature;
using ApiITTP.Features.ChangeLoginFeature;
using ApiITTP.Features.GetActiveUsers;
using ApiITTP.Features.DeleteUserFeature;
using ApiITTP.Features.RecoverUserFeature;
using ApiITTP.Features.GetOldUsers;
using ApiITTP.Features.GetUserByLoginAndPasword;
using ApiITTP.Features.GetUserByLoginFeature;
using ApiITTP.ViewModels;

namespace ApiITTP.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        

        
        IMediator _mediator;

        public UsersController( IMediator mediator)
        {
            
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
           
        }
        /// <summary>
        /// Добавление пользователя
        /// </summary>
        ///  <remarks>
        /// Параметры:
        ///  
        /// Name - имя пользователя
        ///  
        /// Login - логин пользователя (должен быть уникальным)
        ///  
        /// Password - пароль пользователя 
        /// 
        /// Gender - пол ( 0 - женщина, 1 - мужчина, 2 - неизвестно)
        /// 
        /// Admin - будет ли админом
        /// 
        /// DateBirth - дата рождения (может быть null) (например: 2023-05-09)
        /// 
        /// Возвращает добавленного пользователя
        ///</remarks>
        [MyAttribute("OnlyAdmin")]
        [TypeFilter(typeof(SampleExceptionFilter))]
        [Route("AddUser")]
        [HttpPost]
        public async Task<User?> AddUser([FromForm] AddUserCommand client,
           CancellationToken token)
        {
            
            return   await _mediator.Send(client, token);
        }
        /// <summary>
        /// Изменение пароля пользователя
        /// </summary>
        ///  <remarks>
        /// Параметры:
        ///  
        /// Login - логин пользователя 
        ///  
        /// NewPassword - новый пароль пользователя 
        ///
        /// Возвращает true , если операция была успешна
        ///</remarks>
        [MyAttribute("ConcreteStrategy")]
        [TypeFilter(typeof(SampleExceptionFilter))]
        [Route("ChangePassword")]
        [HttpPut]
        public async Task<bool> ChangePassword([FromForm] ChangePasswordCommand client,
          CancellationToken token)
        {

            return await _mediator.Send(client, token);
        }
        /// <summary>
        /// Изменение логина пользователя
        /// </summary>
        ///  <remarks>
        /// Параметры:
        ///  
        /// Login - логин пользователя 
        ///  
        /// NewLogin - новый логин пользователя (должен быть уникальным)
        ///
        /// Возвращает true , если операция была успешна
        ///</remarks>
        [MyAttribute("ConcreteStrategy")]
        [TypeFilter(typeof(SampleExceptionFilter))]
        [Route("ChangeLogin")]
        [HttpPut]
        public async Task<bool> ChangeLogin([FromForm] ChangeLoginCommand client,
          CancellationToken token)
        {

            return await _mediator.Send(client, token);
        }
        /// <summary>
        /// Изменение  имени, пола или даты рождения пользователя
        /// </summary>
        ///  <remarks>
        /// Параметры:
        ///  
        /// Login - логин пользователя 
        ///  
        /// NewName - новое имя пользователя 
        ///
        /// NewGender - новый пол ( 0 - женщина, 1 - мужчина, 2 - неизвестно)
        /// 
        /// NewDateBirth - новая дата рождения (может быть null) (например: 2023-05-09)
        /// 
        /// Возвращает true , если операция была успешна
        ///</remarks>
        [MyAttribute("ConcreteStrategy")]
        [TypeFilter(typeof(SampleExceptionFilter))]
        [Route("ChangeUser")]
        [HttpPut]
        public async Task<bool> ChangeUser([FromForm] ChangeUserCommand client,
         CancellationToken token)
        {

            return await _mediator.Send(client, token);
        }
        /// <summary>
        /// Получение списка активных пользователей
        /// </summary>
        ///  <remarks>
        ///  
        /// Возвращает список активных пользователей , список отсортирован по  CreatedOn
        ///</remarks>
        [MyAttribute("OnlyAdmin")]
        [TypeFilter(typeof(SampleExceptionFilter))]
        [Route("GetActiveUsers")]
        [HttpGet]
        public async Task<List<User>?> GetActiveUsers([FromRoute]
         CancellationToken token)
        {
            GetActiveUsersCommand client = new GetActiveUsersCommand { };
            return await _mediator.Send(client, token);
        }
        /// <summary>
        /// Получение пользователя по логину
        /// </summary>
        ///  <remarks>
        ///  Параметры:
        ///  
        /// Login - логин пользователя
        /// 
        /// Возвращает пользователя, если он существет
        ///</remarks>
        [MyAttribute("OnlyAdmin")]
        [TypeFilter(typeof(SampleExceptionFilter))]
        [Route("GetUserByLogin/{login}")]
        [HttpGet]
        public async Task<UserView?> GetUserByLogin([FromRoute] string login,
        CancellationToken token)
        {
            GetUserByLoginCommand client = new GetUserByLoginCommand { Login = login };
            return await _mediator.Send(client, token);
        }
        /// <summary>
        /// Возвращение списка пользователей старше определенного возраста
        /// </summary>
        ///  <remarks>
        ///  Параметры:
        ///  
        /// Age - возраст, старше которого должны быть возвращаемые пользователи
        /// 
        /// Возвращает список пользователей старше введенного возраста
        ///</remarks>
        [MyAttribute("OnlyAdmin")]
        [TypeFilter(typeof(SampleExceptionFilter))]
        [Route("GetOldUsers/{age:int}")]
        [HttpGet]
        public async Task<List<User>?> GetOldUsers([FromRoute] int age,
        CancellationToken token)
        {
            GetOldUsersCommand client = new GetOldUsersCommand { Age = age };
            return await _mediator.Send(client, token);
        }
        /// <summary>
        /// Получение пользователя по логину и паролю
        /// </summary>
        ///  <remarks>
        ///  Параметры:
        ///  
        ///  Login - логин пользователя
        ///
        ///  Password - пароль пользователя
        /// 
        /// Возвращает пользователя, если он существет
        ///</remarks>
        [MyAttribute("UsersOnly")]
        [TypeFilter(typeof(SampleExceptionFilter))]
        [Route("GetUserByLoginAndPassword/{login}&{password}")]
        [HttpGet]
        public async Task<UserView?> GetUserByLoginAndPasword([FromRoute]string login, string password,
        CancellationToken token)
        {
            GetUserByLoginAndPasswordCommand client = new GetUserByLoginAndPasswordCommand { Login = login, Password = password };
            return await _mediator.Send(client, token);
        }
        /// <summary>
        /// Удаление пользователя
        /// </summary>
        ///  <remarks>
        ///  Параметры:
        ///  
        ///  Login - логин пользователя
        ///
        ///  Type - тип удаления (full - полное    , soft - мягкое )
        /// 
        /// Возвращает true, если операция была удачной
        ///</remarks>
        [MyAttribute("OnlyAdmin")]
        [TypeFilter(typeof(SampleExceptionFilter))]
        [Route("DeleteUser")]
        [HttpDelete]
        public async Task<bool> DeleteUser([FromForm] DeleteUserCommand client,
        CancellationToken token)
        {

            return await _mediator.Send(client, token);
        }
        /// <summary>
        /// Восстановление пользователя
        /// </summary>
        ///  <remarks>
        ///  Параметры:
        ///  
        ///  Login - логин пользователя
        /// 
        /// Возвращает true, если операция была удачной
        ///</remarks>
        [MyAttribute("OnlyAdmin")]
        [TypeFilter(typeof(SampleExceptionFilter))]
        [Route("RecoverUser")]
        [HttpDelete]
        public async Task<bool> RecoverUser([FromForm] RecoverUserCommand client,
        CancellationToken token)
        {

            return await _mediator.Send(client, token);
        }
        
    }
}