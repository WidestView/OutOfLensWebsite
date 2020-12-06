using System.Linq;
using Microsoft.AspNetCore.Mvc;
using OutOfLens_ASP.Models;
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

        public IActionResult Insert(string form)
        {
            string[] validForms = {"insertion/employee"};

            string path = "insertion/" + form;

            if (!validForms.Contains(path))
            {
                return View(path);
            }
            else
            {
                return NotFound();
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

                return View("Insertion/Employee");
            }
        }

        public IActionResult Query(string table)
        {
            string[] validTables = {"query/employee"};

            string path = "query/" + table;

            return validTables.Contains(path) ? (IActionResult) View(path) : NotFound();
        }
    }
}