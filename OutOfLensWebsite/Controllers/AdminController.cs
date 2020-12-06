using System.Linq;
using Microsoft.AspNetCore.Mvc;
using OutOfLensWebsite.Models;

namespace OutOfLensWebsite.Controllers
{
    public class AdminController : Controller
    {
        // GET
        public IActionResult Index()
        {
            return View();
        }

        private IActionResult ResolveView(string prefix, string[] items, string subject)
        {
<<<<<<< Updated upstream
            if (string.IsNullOrEmpty(subject))
=======
            string[] validForms = {"insertion/customer", "insertion/employee"};

            if (string.IsNullOrEmpty(form))
>>>>>>> Stashed changes
            {
                if (!items.Any())
                {
                    return NotFound();
                }
                else
                {
                    return View(prefix + items.First());
                }
            }
     
            if (!items.Contains(subject))
            {
                return NotFound();
            }
            else
            {
                return View(prefix + subject);
            }
        }

        [HttpPost]
        public IActionResult AddEmployee(Employee employee)
        {
            if (ModelState.IsValid)
            {
                return NotFound();
            }
            else
            {
                using (var database = new DatabaseConnection())
                {
                    employee.Register(database);
                }

                return View("Index");
            }
        }
        public IActionResult Add(string form)
        {
            return ResolveView("insertion/", new[] {"customer"}, form);

        }

        public IActionResult Query(string table)
        {
            return ResolveView("query/", new[] {"employee", "customer"}, table);
        }

        public IActionResult Operation()
        {
            return View();
        }

        public IActionResult Report()
        {
            return View();
        }

        public IActionResult Agenda()
        {
            return View();
        }
    }
}