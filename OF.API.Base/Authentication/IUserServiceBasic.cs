using System;
using System.Collections.Generic;
using System.Text;

namespace OF.API.Base.Authentication
{
    public interface IUserServiceBasic<U> where U : IUserAuthentication
    {
        U GetUserById(int userId);
        U GetUserByAccessToken(string userAccessToken);
        U GetDefaultUserForApiKey();
        U GetUserByUserName(string userName);
    }
}
