using MongoDB.Driver;
using ApiITTP.Settings;
using ApiITTP.Models;
using MongoDB.Bson;
using System.Xml.Linq;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System;
using DnsClient;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace ApiITTP.Data
{
    public interface IUserData
    {
        public  Task<string> GetToken(ClaimsIdentity identity);
        public  Task<User> AddUser(User user);
        public  Task<bool> Recover(string login);
        public  Task<bool> ChangePassword(string login, string newpassword, string modifiedby);
        public  Task<User?> Authentificate(string password, string name);
        public Task<bool> ChangeLogin(string oldlogin, string newlogin, string modifiedby);
        public Task<User?> GetByLogin(string login);
        public  Task<List<User>> GetUsersByAge(int year);
        public  Task<List<User>> GetActive();
        public Task<bool> DeleteUser(string login, string type, string admin);
        public Task<bool> ChangeUser(string login, int gender, string name, DateTime? birth, string modifiedby);
        public Task<ClaimsIdentity?> GetIdentity(string username, string password);
      
    }

    public class UserData : IUserData
    {
        public IMongoCollection<User> users;

        public UserData(IDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            

            var database = client.GetDatabase(settings.DatabaseName);

            users = database.GetCollection<User>(settings.CollectionName);
            if (users.CountDocuments(p => p.Admin) ==0 )
            users.InsertOne(new User { Admin = true, Name = "Eugene", Login = "Eugene", Password = "12345" , CreatedOn = DateTime.Today }) ;
        }
        public async Task<bool> ChangeUser(string login, int gender, string name , DateTime? birth , string modifiedby)
        {
            var filter = Builders<User>.Filter.Eq(p => p.Login, login);
            var update = Builders<User>.Update.Set(p => p.Gender, gender).Set(p => p.Name, name).Set(p => p.Birthday, birth).Set(p => p.ModifiedOn, DateTime.Now).Set(p => p.ModifiedBy, modifiedby); ;
            var resul = await this.users.UpdateOneAsync(filter, update);
            if (resul.MatchedCount == 0) throw new Exception("Такого пользователя не существует");
            return true;
        }
        public async Task<bool> CheckAdmin()
        {
            var count  = await this.users.Find<User>(p => p.Admin == true).ToListAsync<User>(); 
            if (count.Count() == 0) return false;
            return true;
        }
        public async Task<User?> GetByLogin(string login)
        {
            return await this.users.Find<User>(p => p.Login == login).FirstOrDefaultAsync<User>();
            
        }
      
        public async Task<List<User>> GetUsersByAge(int year)
        {
            
            var builder = Builders<User>.Filter;
            var filter = Builders<User>.Filter.Where(d => (d.Birthday != null)) &
                Builders<User>.Filter.Where(d => (d.Birthday.Value.AddYears(year+1) < DateTime.Today));
            return await this.users.Find(filter).ToListAsync();
        }
        public async Task<List<User>> GetActive()
        {
            var sortDefinition = Builders<User>.Sort.Descending(p => p.CreatedOn);
            return await this.users.Find<User>(p => p.RevokedOn == null).Sort(sortDefinition).ToListAsync();
        }
        public async Task<bool> ChangeLogin(string oldlogin , string newlogin, string modifiedby)
        {
            var filter = Builders<User>.Filter.Eq(p => p.Login, oldlogin);
            var update = Builders<User>.Update.Set(p => p.Login, newlogin).Set(p => p.ModifiedOn, DateTime.Now).Set(p => p.ModifiedBy, modifiedby);
            var resul = await this.users.UpdateOneAsync(filter, update);
            if (resul.MatchedCount == 0) throw new Exception("Такого пользователя не существует");
            return true;
        }
        public async Task<bool> ChangePassword(string login, string newpassword, string modifiedby)
        {
            
            var filter = Builders<User>.Filter.Eq(p => p.Login, login);
            var update = Builders<User>.Update.Set(p => p.Password, newpassword).Set(p => p.ModifiedOn, DateTime.Now).Set(p => p.ModifiedBy, modifiedby); ;
            var resul = await this.users.UpdateOneAsync(filter, update);
            if (resul.MatchedCount == 0) throw new Exception("Такого пользователя не существует");
            return true;
        }
        public async Task<bool> DeleteUser(string login, string type, string admin)
        {
            
            switch (type)
            {
                case "full":
                    var result = await this.users.DeleteOneAsync(p => p.Login == login);
                    if (result.DeletedCount == 0) throw new Exception("Такого пользователя не существует");
                    return true;
                case "soft":
                    var filter = Builders<User>.Filter.Eq(p => p.Login, login);
                    var update = Builders<User>.Update.Set(p => p.RevokedBy, admin).Set(p => p.RevokedOn, DateTime.Today);
                    var resul = await this.users.UpdateOneAsync(filter, update);
                    if (resul.MatchedCount == 0) throw new Exception("Такого пользователя не существует");
                    return true;

            }

            throw new Exception("Нет такого типа удаления");
        }
        public async Task<bool> Recover(string login )
        {

            var filter = Builders<User>.Filter.Eq(p => p.Login, login);
            var update = Builders<User>.Update.Set(p => p.RevokedBy, null).Set(p => p.RevokedOn, null);   
            var result = await this.users.UpdateManyAsync(filter, update);
            if (result.MatchedCount == 0) throw new Exception("Такого пользователя не существует");
            return true;
        }
        public async Task<User> AddUser(User user)
        {
            await this.users.InsertOneAsync(user);
            return user;
        }

        public async Task<User?> Authentificate ( string name, string password)
        {
            return await this.users.Find<User>(p => p.Login == name && p.Password == password).FirstOrDefaultAsync<User>();

        }
        public async Task<string> GetToken(ClaimsIdentity identity)
        {



            var key = Encoding.ASCII.GetBytes("This is a sample secret key - please don't use in production environment.");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = identity,
                Expires = DateTime.UtcNow.AddMinutes(10),

                SigningCredentials = new SigningCredentials
            (new SymmetricSecurityKey(key),
            SecurityAlgorithms.HmacSha512Signature)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var stringToken = tokenHandler.WriteToken(token);
            return stringToken;
        }
        public async Task<ClaimsIdentity?> GetIdentity(string username, string password)
        {
        
            User? person = await this.Authentificate(username, password);
            if (person == null) return null;
            if (person.RevokedOn != null) return null;
            var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, person.Login),
                
                     new Claim(ClaimsIdentity.DefaultRoleClaimType, (person.Admin ? "Admin": "User")),
                     
                };
            ClaimsIdentity claimsIdentity =
                new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);
              
            return claimsIdentity;
            

            
        }
    }
}