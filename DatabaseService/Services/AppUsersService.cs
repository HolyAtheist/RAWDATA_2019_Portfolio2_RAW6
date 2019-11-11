using System.Linq;
using DatabaseService.Modules;
using NLog.LayoutRenderers;

namespace DatabaseService.Services
{
    public class AppUsersService : IAppUsersService
    {
        AppContext database;

        public AppUsersService()
        {
            database = new AppContext();
        }

        public string GetAppUserName(int id)
        {
            var result = database.AppUser.Find(id);
            return result.Username;
        }

        public int GetAppUserId(string username)
        {
            var user = GetAppUser(username);
            if (user == null)
            {
                return -1;
            }
            else
            {
                return user.Id;
            }
        }

        public AppUser GetAppUser(string username)
        {
            var appUsers = database.AppUser.Where(user => user.Username == username).ToList();
            if (appUsers.Count > 0)
            {
                return appUsers.First();
            }

            return null;
        }

        public bool CreateAppUser(string username, string password, string salt)
        {
            if (!AppUserExist(username))
            {
                database.AppUser.Add(
                    new AppUser()
                    {
                        Username = username,
                        Password = password,
                        Salt = salt
                    });

                var result = database.SaveChanges();
                return result > 0;
            }

            return false;
        }

        public AppUser CreateUser(string name, string password, string salt)
        {
            if (CreateAppUser(name, password, salt))
            {
                return GetAppUser(name);
            }

            return null;
        }

        public bool UpdateAppUserName(string oldName, string newName)
        {
            if (AppUserExist(oldName))
            {
                int appUserId = GetAppUserId(oldName);
                var appUser = database.AppUser.Find(appUserId);
                database.AppUser.Update(appUser);
                appUser.Username = newName;
                var result = database.SaveChanges();
                return result > 0;
            }

            return false;
        }

        public bool DeleteAppUser(int id)
        {
            if (AppUserExist(id))
            {
                var appUser = database.AppUser.Find(id);
                database.AppUser.Remove(appUser);

                var result = database.SaveChanges();
                return result > 0;
            }

            return false;
        }

        public bool DeleteAppUser(string username)
        {
            return DeleteAppUser(GetAppUserId(username));
        }

        public bool AppUserExist(int id)
        {
            var result = database.AppUser.Find(id);
            return result != null;
        }

        public bool AppUserExist(string username)
        {
            return AppUserExist(GetAppUserId(username));
        }
        
    }
}