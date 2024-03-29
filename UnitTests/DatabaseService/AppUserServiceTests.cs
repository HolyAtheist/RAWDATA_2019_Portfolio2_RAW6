using DatabaseService.Modules;
using DatabaseService.Services;
using Xunit;

namespace UnitTests.DatabaseService
{
    public class AppUserServiceTests
    {
        private string Password = "55";
        private string Salt = "salty";

        [Fact]
        public void AppUserExistByIdFalse()
        {
            IAppUserService service = new AppUserService();
            const int userId = -1; //Hardcoded user in DB //todo replace with a mock

            Assert.False(service.AppUserExist(userId));
        }

        [Fact]
        public void AppUserExistByIdTrue()
        {
            IAppUserService service = new AppUserService();
            const int userId = 0; //Hardcoded user in DB //todo replace with a mock

            Assert.True(service.AppUserExist(userId));
        }

        [Fact]
        public void AppUserExistByNameFalse()
        {
            IAppUserService service = new AppUserService();
            const string userName = "£@£@£@€$£$£{£$£@$€$£€€£$€"; //Hardcoded user in DB //todo replace with a mock

            Assert.False(service.AppUserExist(userName));
        }

        [Fact]
        public void AppUserExistByNameTrue()
        {
            IAppUserService service = new AppUserService();
            const string userName = "in"; //Hardcoded user in DB //todo replace with a mock

            Assert.True(service.AppUserExist(userName));
        }

        [Fact]
        public void GetAppUserById()
        {
            IAppUserService service = new AppUserService();
            const int userId = 0;
            const string userName = "in"; //Hardcoded user in DB //todo replace with a mock

            Assert.Equal(userName, service.GetAppUserName(userId));
        }

        [Fact]
        public void GetAppUserByName()
        {
            IAppUserService service = new AppUserService();
            const int userId = 0;
            const string userName = "in"; //Hardcoded user in DB //todo replace with a mock

            Assert.Equal(userId, service.GetAppUserId(userName));
        }

        [Fact]
        public void CreateAppUser()
        {
            IAppUserService service = new AppUserService();
            const string userName = "Mr. Tester von testons";

            bool creationBool = service.CreateAppUser(userName, Password, Salt);
            int userId = service.GetAppUserId(userName);

            Assert.True(creationBool);
            Assert.Equal(userName, service.GetAppUserName(userId));

            //clean up todo delete when mock is working
            service.DeleteAppUser(userId);
        }

        [Fact]
        public void CreateAppUserTwice()
        {
            IAppUserService service = new AppUserService();
            const string userName = "Mr. Tester von testons";

            bool creationBoolOne = service.CreateAppUser(userName, Password, Salt);
            bool creationBoolTwo = service.CreateAppUser(userName, Password, Salt);
            int userId = service.GetAppUserId(userName);

            Assert.True(creationBoolOne);
            Assert.False(creationBoolTwo);
            Assert.Equal(userName, service.GetAppUserName(userId));

            //clean up todo delete when mock is working
            service.DeleteAppUser(userId);
        }

        [Fact]
        public void CreateUserGetObject()
        {
            IAppUserService service = new AppUserService();
            const string userName = "Mr. Tester von testonsen";

            AppUser user = service.CreateUser(userName, Password, Salt);

            Assert.Equal(userName, user.Username);

            //clean up todo delete when mock is working
            service.DeleteAppUser(user.Id);
        }

        [Fact]
        public void CreateUserGetObjectNull()
        {
            IAppUserService service = new AppUserService();
            const string userName = "in";

            AppUser user = service.CreateUser(userName, Password, Salt);

            Assert.Null(user);
        }

        [Fact]
        public void UpdateAppUserNameValidUser()
        {
            IAppUserService service = new AppUserService();
            const string userNameOne = "Ms. donald docker";
            const string userNameTwo = "Ms. donald ducker";

            bool creationBool = service.CreateAppUser(userNameOne, Password, Salt);
            int userIdOne = service.GetAppUserId(userNameOne);
            bool updateBool = service.UpdateAppUserName(userNameOne, userNameTwo);
            int userIdTwo = service.GetAppUserId(userNameTwo);

            Assert.True(creationBool);
            Assert.True(updateBool);

            Assert.Equal(userIdOne, userIdTwo);

            Assert.NotEqual(userNameOne, service.GetAppUserName(userIdOne));
            Assert.Equal(userNameTwo, service.GetAppUserName(userIdOne));

            //clean up todo delete when mock is working
            service.DeleteAppUser(userIdOne);
            service.DeleteAppUser(userIdTwo);
        }

        [Fact]
        public void UpdateAppUserNameInvalidUser()
        {
            IAppUserService service = new AppUserService();
            const string userNameOne = "Ms. ronaldo docker";
            const string userNameTwo = "Ms. ronaldo ducker";

            bool updateBool = service.UpdateAppUserName(userNameOne, userNameTwo);

            Assert.False(updateBool);
        }

        [Fact]
        public void DeleteAppUserByNameTrue()
        {
            IAppUserService service = new AppUserService();
            const string userName = "dock";

            bool creationBool = service.CreateAppUser(userName, Password, Salt);
            bool existBeforeDeletion = service.AppUserExist(userName);
            bool deletionBool = service.DeleteAppUser(userName);

            Assert.True(creationBool);
            Assert.True(existBeforeDeletion);
            Assert.True(deletionBool);
            Assert.False(service.AppUserExist(userName));
        }

        [Fact]
        public void DeleteAppUserByNameFalse()
        {
            IAppUserService service = new AppUserService();
            const string userName = "docker";
            const string falseName = "not docker";

            bool creationBool = service.CreateAppUser(userName, Password, Salt);
            bool existBeforeDeletion = service.AppUserExist(userName);
            bool deletionBool = service.DeleteAppUser(falseName);

            Assert.True(creationBool);
            Assert.True(existBeforeDeletion);
            Assert.False(deletionBool);
            Assert.True(service.AppUserExist(userName));

            //clean up todo delete when mock is working
            service.DeleteAppUser(userName);
        }

        [Fact]
        public void DeleteAppUserByIdTrue()
        {
            IAppUserService service = new AppUserService();
            const string userName = "donald";

            bool creationBool = service.CreateAppUser(userName, Password, Salt);
            int userId = service.GetAppUserId(userName);
            bool existBeforeDeletion = service.AppUserExist(userId);
            bool deletionBool = service.DeleteAppUser(userId);

            Assert.True(creationBool);
            Assert.True(existBeforeDeletion);
            Assert.True(deletionBool);
            Assert.False(service.AppUserExist(userName));
            Assert.False(service.AppUserExist(userId));
        }

        [Fact]
        public void DeleteAppUserByIdFalse()
        {
            IAppUserService service = new AppUserService();
            const string userName = "niels";
            const int falseId = -2;

            bool creationBool = service.CreateAppUser(userName, Password, Salt);
            int userId = service.GetAppUserId(userName);
            bool existBeforeDeletion = service.AppUserExist(userId);
            bool deletionBool = service.DeleteAppUser(falseId);

            Assert.True(creationBool);
            Assert.True(existBeforeDeletion);
            Assert.False(deletionBool);
            Assert.True(service.AppUserExist(userName));
            Assert.True(service.AppUserExist(userId));

            //clean up todo delete when mock is working
            service.DeleteAppUser(userName);
        }
    }
}