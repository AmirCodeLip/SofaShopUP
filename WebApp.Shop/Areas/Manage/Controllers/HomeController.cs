using Microsoft.AspNetCore.Mvc;

namespace WebApp.Shop.Areas.Manage.Controller
{
    [Area("Manage")]
    public class HomeController : ControllerBase
    {
        public string Index()
        {
            return "Welcome to the Jungle SEXY!";
        }
    }
}
