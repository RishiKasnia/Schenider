using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using Thesaurus.Services;

namespace Thesaurus.API.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]


    public class ErrorsController : ControllerBase
    {
        private readonly ILogger<ErrorsController> _logger;

        public ErrorsController(ILogger<ErrorsController> logger)
        {
            _logger = logger;
        }

        //public ThesaurusErrorResponse Error()
        //{
        //    var context = HttpContext.Features.Get<IExceptionHandlerFeature>();
        //    var exception = context.Error; // custom exception
        //    var code = 500; // Internal Server Error by default

        //    _logger.LogError(exception, exception.Message);


        //    if (exception is HttpStatusException httpException)
        //    {
        //        code = (int)httpException.Status;
        //    }

        //    Response.StatusCode = code;

        //    return new ThesaurusErrorResponse(exception);
        //}

        [HttpGet]
        [Route("/error-local-development")]
        public IActionResult ErrorLocalDevelopment(
        [FromServices] IWebHostEnvironment webHostEnvironment)
        {
            if (webHostEnvironment.EnvironmentName != "Development")
            {
                throw new InvalidOperationException(
                    "This shouldn't be invoked in non-development environments.");
            }

            var context = HttpContext.Features.Get<IExceptionHandlerFeature>();
            var exception = context.Error; // custom exception

            _logger.LogError(exception, exception.Message);

            if (exception is HttpStatusException httpException)
            {
                Response.StatusCode = (int)httpException.Status;
            }

            return Problem(
                detail: string.Empty,
                title: context.Error.Message);
        }

        [HttpGet]
        [Route("/error")]
        public IActionResult Error() => Problem();
    }

    public class ThesaurusErrorResponse
    {
        public string Type { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }

        public ThesaurusErrorResponse(Exception ex)
        {
            //Type = ex.GetType().Name;
            Message = ex.Message;
            //StackTrace = ex.ToString();
        }
    }
}
