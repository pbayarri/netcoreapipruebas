using System;
using System.Collections.Generic;
using System.Text;

namespace OF.API.Base.Authentication
{
    public interface IUserAuthentication
    {
        int GetUserId();
        string GetUserName();
        string GetUserPasword();
        string GetGeneratedToken();
    }
}
