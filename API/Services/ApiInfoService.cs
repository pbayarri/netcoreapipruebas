using API.Entities;
using API.Helpers;
using OF.API.Front.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Services
{
    public interface IApiInfoService
    {
        IEnumerable<ApiInfo> GetAll();
    }

    public class ApiInfoService : IApiInfoService
    {
        private readonly DataContext _context;

        public ApiInfoService(DataContext context)
        {
            _context = context;
        }

        public IEnumerable<ApiInfo> GetAll()
        {
            return _context.InstalledApis;
        }
    }
}
