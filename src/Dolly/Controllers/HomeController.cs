using Microsoft.AspNet.Mvc;

namespace Dolly.Controllers
{
    public class HomeController : BaseController
    {
        public IActionResult Index() => RedirectToActionPermanent(nameof(Index), "Items");

        public IActionResult About() => View();

        public IActionResult Contact() => View();

        public IActionResult Error() => View();

    }
}
