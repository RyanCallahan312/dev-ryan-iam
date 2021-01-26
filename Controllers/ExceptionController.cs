using dev_ryan_iam.DTOs;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace dev_ryan_iam.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ExceptionController : ControllerBase
    {
        [Route("exception")]
        public IActionResult Error()
        {
            IExceptionHandlerFeature context = HttpContext.Features.Get<IExceptionHandlerFeature>();
            Exception exception = context.Error;

            ExceptionDTO exceptionDTO = new()
            {
                Exception = exception,
                Message = exception.Message
            };

            int statusCode = (int)HttpStatusCode.InternalServerError;

            return StatusCode(statusCode, exceptionDTO);
        }
    }
}
