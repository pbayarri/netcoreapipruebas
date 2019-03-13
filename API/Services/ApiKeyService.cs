using API.Entities;
using API.Helpers;
using OF.API.Base.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Services
{
    public interface IApiKeyService : IApiKeyServiceBasic<ApiKey>
    {
    }

    public class ApiKeyService : IApiKeyService
    {
        private DataContext _context;

        public ApiKeyService(DataContext context)
        {
            _context = context;
        }

        public IEnumerable<ApiKey> GetAuthorizedApiKeys()
        {
            return _context.ApiKeys;
        }
    }
}
