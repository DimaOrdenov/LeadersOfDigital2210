using Microsoft.AspNetCore.Mvc;

namespace LeadersWeb.Controllers
{
    [Route("[controller]")]
    public class EasyDataController : Controller
    {
        [Route("{**entity}")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
