using Microsoft.AspNetCore.Mvc;

namespace ResumeManager.Controllers
{
    public class ResumeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
