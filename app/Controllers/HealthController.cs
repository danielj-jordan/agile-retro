using Microsoft.AspNetCore.Mvc;

namespace app.Controllers
{
    [Route("api/[controller]")]
    public class HealthController: Controller
    {
        [HttpGet]
        public ActionResult<string> GetHealth (){
            return "OK";
        }
        
    }
}