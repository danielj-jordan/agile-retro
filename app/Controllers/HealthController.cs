using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace app.Controllers
{
    [Route("api/[controller]")]
    public class HealthController: Controller
    {

        private ILogger<HealthController> logger;
        public HealthController(ILogger<HealthController> logger)
        {
            this.logger=logger;
        }
        [HttpGet]
        public ActionResult<string> GetHealth (){
            logger.LogInformation("health has been called");
            return "OK";
        }
        
    }
}