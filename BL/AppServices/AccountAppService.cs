using BL.Bases;
using BL.Interfaces;
using BL.ViewModels;
using DAL;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BL.AppServices
{
    public class AccountAppService : AppServiceBase
    {
        IConfiguration _configuration;

        public AccountAppService(IUnitOfWork theUnitOfWork,IConfiguration configuration) : base(theUnitOfWork)
        {
            this._configuration = configuration;
        }
        //Login
        public List<RegisterViewodel> GetAllAccounts()
        {
            return Mapper.Map<List<RegisterViewodel>>(TheUnitOfWork.Account.GetAllAccounts().Where(ac => ac.isDeleted == false));
        }
        public RegisterViewodel GetAccountById(string id)
        {
            if (id == null)
                throw new ArgumentNullException();
            return Mapper.Map<RegisterViewodel>(TheUnitOfWork.Account.GetAccountById(id));

        }

        public bool DeleteAccount(string id)
        {
            if (id == null)
                throw new ArgumentNullException();
            bool result = false;
            ApplicationUserIdentity user = TheUnitOfWork.Account.GetAccountById(id);
            user.isDeleted = true;
            TheUnitOfWork.Account.Update(user);
            result = TheUnitOfWork.Commit() > new int();

            return result;
        }
        public async Task<ApplicationUserIdentity> Find(string name, string password)
        {
            ApplicationUserIdentity user = await TheUnitOfWork.Account.Find(name, password);

            if (user != null && user.isDeleted == false)
                return user;
            return null;
        }
        public async Task<IdentityResult> Register(RegisterViewodel user)
        {

            ApplicationUserIdentity identityUser = Mapper.Map<RegisterViewodel, ApplicationUserIdentity>(user);
            return await TheUnitOfWork.Account.Register(identityUser);

        }
        public async Task<IdentityResult> AssignToRole(string userid, string rolename)
        {
            if (userid == null || rolename == null)
                throw new ArgumentNullException();
            return await TheUnitOfWork.Account.AssignToRole(userid, rolename);


        }
        public async Task<bool> UpdatePassword(string userID, string newPassword)
        {
            //    ApplicationUserIdentity identityUser = TheUnitOfWork.Account.FindById(user.Id);

            //    Mapper.Map(user, identityUser);

            //    return TheUnitOfWork.Account.UpdateAccount(identityUser);


            ApplicationUserIdentity identityUser = await TheUnitOfWork.Account.FindById(userID);
            identityUser.PasswordHash = newPassword;
            return await TheUnitOfWork.Account.updatePassword(identityUser);

        }

        public async Task<bool> Update(RegisterViewodel user)
        {
            //    ApplicationUserIdentity identityUser = TheUnitOfWork.Account.FindById(user.Id);

            //    Mapper.Map(user, identityUser);

            //    return TheUnitOfWork.Account.UpdateAccount(identityUser);


            ApplicationUserIdentity identityUser = await TheUnitOfWork.Account.FindById(user.Id);
            var oldPassword = identityUser.PasswordHash;
            Mapper.Map(user, identityUser);
            identityUser.PasswordHash = oldPassword;
            return await TheUnitOfWork.Account.UpdateAccount(identityUser);

        }
        public async Task<bool> checkUserNameExist(string userName)
        {
            var user = await TheUnitOfWork.Account.FindByName(userName);
            return user == null ? false : true;
        }
        public async Task<IEnumerable<string>> GetUserRoles (ApplicationUserIdentity user)
        {
            return await TheUnitOfWork.Account.GetUserRoles(user);
        }
       public async Task<dynamic> CreateToken(ApplicationUserIdentity user)
        {
            var userRoles = await GetUserRoles(user);

            var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                   new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = token.ValidTo
            } ;

           
        }

    }
}
