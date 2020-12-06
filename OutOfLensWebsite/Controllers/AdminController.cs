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


        public IActionResult Add(string form)
        {
            string[] validForms = {"insertion/customer"};

            if (string.IsNullOrEmpty(form))
            {
                form = "customer";
            }

            string path = "insertion/" + form;

            if (validForms.Contains(path))
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

                return View("Index");
            }
        }

        public IActionResult Query(string table)
        {
            string[] validTables = {"query/employee", "query/customer"};

            if (string.IsNullOrEmpty(table))
            {
                table = "employee";
            }

            string path = "query/" + table;

            return validTables.Contains(path) ? (IActionResult) View(path) : NotFound();
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