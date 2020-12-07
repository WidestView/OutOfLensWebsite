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
            return ResolveView("Insertion/", new[] {"Customer", "Employee", "Package", "Package_Type"}, id);

        }

        [HttpGet]
        public IActionResult Query(string id)
        {
            return ResolveView("Query/", new[] {"Employee", "Customer"}, id);
        }

        public IActionResult Operation()
        {
            var connection = new DatabaseConnection();
            
            TableViewModel model = new TableViewModel();
            model.Labels = new[] {"ID", "Nome", "Horário Entrada", "Horário Saída"};
            model.Data = EmployeeShift.GetTable(connection);
                
            return View(model);
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
