using API.Entities;
using API.Helpers;
using Microsoft.EntityFrameworkCore;
using OF.API.Base.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Services
{
    public interface IRoleService : IRoleServiceBasic
    {
    }

    public class RoleService : IRoleService
    {
        private DataContext _context;

        public RoleService(DataContext context)
        {
            _context = context;
        }

        public IEnumerable<string> GetUserFunctionalities(int userId)
        {
            // probando el uso de los SP
            return _context.RoleFunctionalities
                .FromSql($"GetUserFunctionalities {userId}")
                .Select(f => f.Name).ToList();
        }
    }
}
