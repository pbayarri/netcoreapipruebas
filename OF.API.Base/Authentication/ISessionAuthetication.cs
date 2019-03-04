using System;
using System.Collections.Generic;
using System.Text;

namespace OF.API.Base.Authentication
{
    public interface ISessionAuthetication
    {
        string GetToken();
        DateTime GetLastAccess();
        int GetId();
    }
}
