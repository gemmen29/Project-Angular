using BL.Bases;
using BL.Interfaces;
using BL.ViewModels;
using DAL;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.AppServices
{
    public class AccountAppService : AppServiceBase
    {
        
        public AccountAppService(IUnitOfWork theUnitOfWork):base(theUnitOfWork)
        {
                
        }
        //Login
        public List<RegisterViewodel> GetAllAccounts()
        {
            return Mapper.Map<List<RegisterViewodel>>(TheUnitOfWork.Account.GetAllAccounts().Where(ac=>ac.isDeleted==false));
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
           
            if( user != null && user.isDeleted == false)
                  return user;
            return null;
        }
        public async Task <IdentityResult> Register(RegisterViewodel user)
        {
          
            ApplicationUserIdentity identityUser = Mapper.Map<RegisterViewodel, ApplicationUserIdentity>(user);
            return await TheUnitOfWork.Account.Register(identityUser);

        }
        public async Task <IdentityResult> AssignToRole(string userid, string rolename)
        {
            if (userid == null || rolename == null)
                throw new ArgumentNullException();
            return await TheUnitOfWork.Account.AssignToRole(userid, rolename);


        }
        public async Task <bool> UpdatePassword(string userID,string newPassword)
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


            ApplicationUserIdentity identityUser =await TheUnitOfWork.Account.FindById(user.Id);
            var oldPassword = identityUser.PasswordHash;
            Mapper.Map(user, identityUser);
            identityUser.PasswordHash = oldPassword;
            return  await TheUnitOfWork.Account.UpdateAccount(identityUser);

        }

    }
}
