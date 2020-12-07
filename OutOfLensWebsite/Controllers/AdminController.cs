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
            if (string.IsNullOrEmpty(subject))
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
            if (!ModelState.IsValid)
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
        
        public IActionResult Add(string id)
        {
            return ResolveView("Insertion/", new[] {"Customer", "Employee"}, id);

        }

        [HttpGet]
        public IActionResult Query(string id)
        {
            return ResolveView("Query/", new[] {"Employee", "Customer"}, id);
        }

        [HttpGet]
        public IActionResult Operation()
        {
            return View();
        }

        [HttpGet]
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
