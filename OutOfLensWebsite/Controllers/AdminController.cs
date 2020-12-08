using System.Linq;
using Microsoft.AspNetCore.Mvc;
using OutOfLens_ASP.Models;
using OutOfLensWebsite.Models;
using OutOfLensWebsite.Models.Data;

namespace OutOfLensWebsite.Controllers
{
    public class AdminController : Controller
    {
        // GET
        public IActionResult Index()
        {
            ViewBag.PageLocation = PageLocation.Home;
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
            ViewBag.PageLocation = PageLocation.Insertion;
            
            if (!ModelState.IsValid)
            {
                return View("Insertion/Employee");
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
        

        [HttpPost]
        public IActionResult AddCustomer(Customer customer)
        {
            ViewBag.PageLocation = PageLocation.Insertion;
            
            if (ModelState.IsValid)
            {
                using var database = new DatabaseConnection();

                customer.Insert(database);

                return View("Index");
            }
            else
            {
                return View("Insertion/Customer");
            }
        }

        [HttpPost]
        public IActionResult AddPackageType(PackageType packageType)
        {
            ViewBag.PageLocation = PageLocation.Insertion;
            
            if (ModelState.IsValid)
            {
                using DatabaseConnection connection = new DatabaseConnection();

                packageType.Insert(connection);

                return View("Index");
            }
            else
            {
                return View("Insertion/Package_Type");
            }
        }

        [HttpPost]
        public IActionResult AddPackage(Package package)
        {
            ViewBag.PageLocation = PageLocation.Insertion;
            
            if (ModelState.IsValid)
            {
                using DatabaseConnection connection = new DatabaseConnection();

                package.Insert(connection);

                return View("Index");
            }
            else
            {
                return View("Insertion/Package");
            }
        }

        [HttpPost]
        public IActionResult AddReport(Report report)
        {
            ViewBag.PageLocation = PageLocation.Insertion;
            
            if (ModelState.IsValid)
            {
                using DatabaseConnection connection = new DatabaseConnection();

                report.Insert(connection);

                return View("Index");
            }
            else
            {
                return View("Report");
            }
        }

        public IActionResult Add(string id)
        {
            ViewBag.PageLocation = PageLocation.Insertion;
            return ResolveView("Insertion/", new[] {"Customer", "Employee", "Package", "Package_Type"}, id);
        }

        [HttpGet]
        public IActionResult Query(string id)
        {
            ViewBag.PageLocation = PageLocation.Query;
            
            using var connection = new DatabaseConnection();

            if (id == null || id.ToLower() == "Employee".ToLower())
            {
                return View("Query/Employee", Employee.GetTable(connection));
            }

            return ResolveView("Query/", new[] {"Employee", "Customer"}, id);
        }

        public IActionResult Operation()
        {
            ViewBag.PageLocation = PageLocation.Operation;
            
            var connection = new DatabaseConnection();

            TableViewModel model = new TableViewModel();
            model.Labels = new[] {"ID", "Nome", "Horário Entrada", "Horário Saída"};
            model.Data = EmployeeShift.GetTable(connection);

            return View(model);
        }

        public IActionResult Report()
        {
            ViewBag.PageLocation = PageLocation.Report;
            return View();
        }

        public IActionResult Agenda()
        {
            ViewBag.PageLocation = PageLocation.Agenda;
            return View();
        }

        public enum PageLocation
        {
            Home,
            Insertion,
            Query,
            Report,
            Agenda,
            Operation
        }
    }
}