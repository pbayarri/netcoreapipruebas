using System;
using System.Collections.Generic;
using System.Text;

namespace OF.API.Base.Authorization
{
    public interface IRoleServiceBasic
    {
        IEnumerable<String> GetUserFunctionalities(int userId);
    }
}
