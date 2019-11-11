using DatabaseService.Modules;
using System;
using System.Collections.Generic;
using System.Text;

namespace DatabaseService.Services
{
    public interface IAuthUsersService
    {
        AppUser GetUserByUserName(string username);
        AppUser CreateUser(string username, string password, string salt);
    }
}
