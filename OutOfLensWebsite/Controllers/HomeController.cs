using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OutOfLensWebsite.Models;

namespace OutOfLensWebsite.Controllers
{
    public class HomeController : Controller
    {
        private const string SessionLoggedId = "__session_logged_user_id";
        
        public ActionResult Index()
        {


            Employee employee = null;
            
            int? id = HttpContext.Session.GetInt32(SessionLoggedId);
            
            if (id != null)
            {
                using var connection = new DatabaseConnection();
                
               employee = Employee.From((int) id, connection);
            }

            return View("Home", new HomeModel { Employee = employee, LoginData = new Login()});
        }

        [HttpPost]
        public ActionResult LoginUser(HomeModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Error = "Model Not Valid!";
                return View("GenericError");
            }
            
            using var connection = new DatabaseConnection();

            var result = Employee.Login( model.LoginData.Email, model.LoginData.Password,
                connection);

            if (result != null)
            {
                HttpContext.Session.SetInt32(SessionLoggedId, result.Identifier);
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Error = "User not found";
                return View("Home", new HomeModel { LoginData = new Login(), Employee = null});
            }
        }

        public IActionResult LogOut()
        {
            HttpContext.Session.Clear();

            return Redirect("Index");
        }


    }
}