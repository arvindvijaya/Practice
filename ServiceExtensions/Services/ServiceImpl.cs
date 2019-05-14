using Interfaces.Models;
using SampleApplication.DbContext;
using SampleApplication.Domain;
using SampleApplication.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SampleApplication.Services
{
    public class PNotesServiceImpl : IPNotesService
    {
        private readonly Func<IPNoteServicesDbContext> _seiServicesDbContext;

        public PNotesServiceImpl(Func<IPNoteServicesDbContext> contextFactory)
        {
            _seiServicesDbContext = contextFactory;
        }

        public async Task<UserInfo> GetUserById(string LoginId)
        {
            var userInfo = new UserInfo();

            using (var context = _seiServicesDbContext())
            {
                var appUser = context.GetAsQueryable<ApplicationUser>().FirstOrDefault(x => x.LoginId == LoginId);

                if (appUser != null)
                {
                    userInfo.FirstName = appUser.FirstName;
                    userInfo.LastName = appUser.LastName;
                    userInfo.UserLoginId = appUser.LoginId;
                    userInfo.UserEmail = appUser.EmailId;
                }
                else return null;
            }

            return userInfo;
        }

        public async Task<bool> UpdateUser()
        { 
            using (var context = _seiServicesDbContext())
            {
                var appUser = context.GetAsQueryable<ApplicationUser>().FirstOrDefault(x => x.User_Id == 1);

                appUser.LastName = "test 2343";

                context.Update(appUser);

                await context.SaveChangesAsync();
            }

            return true;
        }
    }
}
