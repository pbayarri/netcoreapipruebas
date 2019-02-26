using System;
using System.Collections.Generic;
using System.Text;

namespace OF.API.Base.Authentication
{
    public interface IUserServiceBasic<T>
    {
        T GetById(int id);
    }
}
