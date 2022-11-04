using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using SwiftTestingFrameworkAPI.Utils;

namespace SwiftTestingFrameworkAPI.Controllers
{
    [ApiController]
    [Route("/")]
    public class DefaultController : ControllerBase
    {

        private readonly ILogger<DefaultController> _logger;

        public DefaultController(ILogger<DefaultController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public ObjectResult GetInfo()
        {
            return StatusCode(222, "Site deployed.");
        }
    }
}