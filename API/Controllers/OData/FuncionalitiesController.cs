using API.Helpers;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers.OData
{
    [Authorize]
    public class FuncionalitiesController : ODataController
    {
        private DataContext _dataContext;

        public FuncionalitiesController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [EnableQuery]
        public IActionResult Get()
        {
            return Ok(_dataContext.RoleFunctionalities);
        }

        [EnableQuery]
        public IActionResult Get(int key)
        {
            return Ok(_dataContext.RoleFunctionalities.FirstOrDefault(c => c.Id == key));
        }
    }
}
