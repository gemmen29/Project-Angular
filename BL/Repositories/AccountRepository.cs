using BL.Bases;
using DAL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BL.Repositories
{
    public class AccountRepository: BaseRepository<ApplicationUserIdentity>
    {
        private readonly UserManager<ApplicationUserIdentity> manager;
       // ApplicationUserManager manager;
      

        public AccountRepository(DbContext db, UserManager<ApplicationUserIdentity> manager) : base(db)
        {
            //manager = new ApplicationUserManager(db);
            this.manager = manager;
           
        }
    
        public ApplicationUserIdentity GetAccountById(string id)
        {
            return GetFirstOrDefault(l => l.Id == id);
        }
        public List<ApplicationUserIdentity> GetAllAccounts()
        {
            return GetAll().ToList();
        }
        public async Task<ApplicationUserIdentity> FindById(string id)
        {

            ApplicationUserIdentity result = await manager.FindByIdAsync(id);

            return result;

        }
        public async Task <ApplicationUserIdentity> Find(string username, string password)
        {

            //ApplicationUserIdentity result = manager.find(username, password);

            //return result;
            var user = await manager.FindByNameAsync(username);
            if (user != null && await manager.CheckPasswordAsync(user, password))
            {
                return user;
            }
                
            return null;
            //return new ApplicationUserIdentity();

        }
        public async Task <IdentityResult> Register(ApplicationUserIdentity user)
        {
            user.Id = Guid.NewGuid().ToString();
            IdentityResult result;

                result = await manager.CreateAsync(user, user.PasswordHash);
            
            //catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            //{
            //    Exception raise = dbEx;
            //    foreach (var validationErrors in dbEx.EntityValidationErrors)
            //    {
            //        foreach (var validationError in validationErrors.ValidationErrors)
            //        {
            //            string message = string.Format("{0}:{1}",
            //                validationErrors.Entry.Entity.ToString(),
            //                validationError.ErrorMessage);
            //            // raise a new exception nesting  
            //            // the current instance as InnerException  
            //            raise = new InvalidOperationException(message, raise);
            //        }
            //    }
            //    throw raise;
            //}

          
           // manager.AddToRole(user.Id,"Admin");
            return result;
        }
        public async Task <IdentityResult> AssignToRole(string userid, string rolename)
        {
            List<string> roles = new List<string>();
            roles.Add(rolename);
            var user = await manager.FindByIdAsync(userid);
            if(user != null)
            {
                IdentityResult result = await manager.AddToRolesAsync(user,roles);
                return result;
            }
            return null;

        }
        public async Task <bool> updatePassword(ApplicationUserIdentity user)
        {
             manager.PasswordHasher.HashPassword(user, user.PasswordHash);
            //user.PasswordHash = manager.PasswordHasher.HashPassword(user.PasswordHash);
            IdentityResult result = await manager.UpdateAsync(user);
            return true;
        }
       
        public async Task<bool> UpdateAccount(ApplicationUserIdentity user)
        {

            //user.PasswordHash = manager.PasswordHasher.HashPassword(user.PasswordHash);
            //try
            //{

            //    IdentityResult result = manager.Update(user);
            //}
            //catch (Exception e)
            //{


            //}
            IdentityResult result = await manager.UpdateAsync(user);



            return true;
        }
    }
}
