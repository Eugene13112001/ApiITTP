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
        /// ���������� ������������
        /// </summary>
        ///  <remarks>
        /// ���������:
        ///  
        /// Name - ��� ������������
        ///  
        /// Login - ����� ������������ (������ ���� ����������)
        ///  
        /// Password - ������ ������������ 
        /// 
        /// Gender - ��� ( 0 - �������, 1 - �������, 2 - ����������)
        /// 
        /// Admin - ����� �� �������
        /// 
        /// DateBirth - ���� �������� (����� ���� null) (��������: 2023-05-09)
        /// 
        /// ���������� ������������ ������������
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
        /// ��������� ������ ������������
        /// </summary>
        ///  <remarks>
        /// ���������:
        ///  
        /// Login - ����� ������������ 
        ///  
        /// NewPassword - ����� ������ ������������ 
        ///
        /// ���������� true , ���� �������� ���� �������
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
        /// ��������� ������ ������������
        /// </summary>
        ///  <remarks>
        /// ���������:
        ///  
        /// Login - ����� ������������ 
        ///  
        /// NewLogin - ����� ����� ������������ (������ ���� ����������)
        ///
        /// ���������� true , ���� �������� ���� �������
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
        /// ���������  �����, ���� ��� ���� �������� ������������
        /// </summary>
        ///  <remarks>
        /// ���������:
        ///  
        /// Login - ����� ������������ 
        ///  
        /// NewName - ����� ��� ������������ 
        ///
        /// NewGender - ����� ��� ( 0 - �������, 1 - �������, 2 - ����������)
        /// 
        /// NewDateBirth - ����� ���� �������� (����� ���� null) (��������: 2023-05-09)
        /// 
        /// ���������� true , ���� �������� ���� �������
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
        /// ��������� ������ �������� �������������
        /// </summary>
        ///  <remarks>
        ///  
        /// ���������� ������ �������� ������������� , ������ ������������ ��  CreatedOn
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
        /// ��������� ������������ �� ������
        /// </summary>
        ///  <remarks>
        ///  ���������:
        ///  
        /// Login - ����� ������������
        /// 
        /// ���������� ������������, ���� �� ���������
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
        /// ����������� ������ ������������� ������ ������������� ��������
        /// </summary>
        ///  <remarks>
        ///  ���������:
        ///  
        /// Age - �������, ������ �������� ������ ���� ������������ ������������
        /// 
        /// ���������� ������ ������������� ������ ���������� ��������
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
        /// ��������� ������������ �� ������ � ������
        /// </summary>
        ///  <remarks>
        ///  ���������:
        ///  
        ///  Login - ����� ������������
        ///
        ///  Password - ������ ������������
        /// 
        /// ���������� ������������, ���� �� ���������
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
        /// �������� ������������
        /// </summary>
        ///  <remarks>
        ///  ���������:
        ///  
        ///  Login - ����� ������������
        ///
        ///  Type - ��� �������� (full - ������    , soft - ������ )
        /// 
        /// ���������� true, ���� �������� ���� �������
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
        /// �������������� ������������
        /// </summary>
        ///  <remarks>
        ///  ���������:
        ///  
        ///  Login - ����� ������������
        /// 
        /// ���������� true, ���� �������� ���� �������
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