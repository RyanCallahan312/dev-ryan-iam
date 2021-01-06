using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace auto_highlighter_iam.Controllers
{
    [ApiController]
    [Route("/api-v1/[controller]")]
    public class AuthorizationController : ControllerBase
    {
        [HttpGet]
        public IActionResult Index()
        {
            throw new NotImplementedException();
        }
    }
}
