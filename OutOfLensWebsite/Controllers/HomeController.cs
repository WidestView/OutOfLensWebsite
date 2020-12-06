using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using OutOfLens_ASP.Models;
using OutOfLensWebsite.Models;

namespace OutOfLensWebsite.Controllers
{
    public class HomeController : Controller
    {
        private readonly List<string> _brazilStates = new List<string>
        {
            "AC", "AL", "AP", "AM", "BA", "CE", "DF", "ES", "GO", "MA", "MT", "MS", "MG", "PA", "PB",
            "PR", "PE", "PI", "RJ", "RN", "RS", "RO", "RR", "SC", "SP", "SE", "TO"
        };

        public ActionResult Index()
        {
            ViewBag.isLogged = true;

            return View("Home");
        }

        [HttpPost]
        public ActionResult SignUser(User user)
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

        public JsonResult CheckState(string state)
        {
            return Json(_brazilStates.Contains(state));
        }
    }
}