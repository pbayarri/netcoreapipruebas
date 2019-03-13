using System;
using System.Collections.Generic;
using System.Text;

namespace OF.API.Base.Authentication
{
    public interface IApiKeyServiceBasic<K> where K : IApiKeyAuthentication
    {
        IEnumerable<K> GetAuthorizedApiKeys();
    }
}
