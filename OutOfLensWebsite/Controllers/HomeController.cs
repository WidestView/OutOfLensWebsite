using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using OutOfLens_ASP.Models;
using OutOfLensWebsite.Models;

namespace OutOfLensWebsite.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.isLogged = true;

            return View("Home");
        }

        [HttpPost]
        public ActionResult LoginUser(HomeModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Error = "Model Not Valid!";
                return View("GenericError");
            }


            /*Cadastrar*/


            return RedirectToAction("Index");
        }

        // MODELS CHECKING

    }
}