using Microsoft.AspNetCore.Mvc;


namespace AspNetCoreVueJs.Web.Controllers
{
    public class SpaController : Controller
    {
        
        public IActionResult Index()
        {
            return Redirect("~/Index.html");
        }
    }
}
