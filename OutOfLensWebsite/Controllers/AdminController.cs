using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OutOfLensWebsite.Models;
using OutOfLensWebsite.Models.Data;

namespace OutOfLensWebsite.Controllers
{
    // This class does to much stuff, therefore breaking the Single Responsibility Principle.
    // Not that our other code is SOLID or anything
    // TODO: Move into different controllers


    public class AdminController : Controller
    {
        // TODO: Implement LoggedOnlyAttribute


        private bool IsLogged()
        {
            return HttpContext.Session.GetInt32(HomeController.SessionLoggedId) != null;
        }

        // GET
        public IActionResult Index()
        {
            if (!IsLogged())
                return NotFound();

            ViewBag.PageLocation = PageLocation.Home;
            return View();
        }


        [HttpPost]
        public IActionResult AddEmployee(Employee employee)
        {
            if (!IsLogged())
                return NotFound();
            
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
            if (!IsLogged())
                return NotFound();
            
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
            if (!IsLogged())
                return NotFound();
            
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
            if (!IsLogged())
                return NotFound();
            
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
            if (!IsLogged())
                return NotFound();
            
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

        [HttpPost]
        public IActionResult AddOrder(Order order)
        {
            if (!IsLogged())
                return NotFound();
            
            if (ModelState.IsValid)
            {
                var connection = new DatabaseConnection();
                order.Insert(connection);
                return View("Operation/Index");
            }
            else
            {
                return View("Operation/Order");
            }
        }

        [HttpPost]
        public IActionResult AddSession(Session session)
        {
            if (!IsLogged())
                return NotFound();
            
            if (ModelState.IsValid)
            {
                var connection = new DatabaseConnection();
                session.Insert(connection);
                return View("Operation/Index");
            }
            else
            {
                return View("Operation/Session");
            }
        }

        [HttpPost]
        public IActionResult AddRole(Role role)
        {
            
            if (!IsLogged())
                return NotFound();
            
            if (ModelState.IsValid)
            {
                var connection = new DatabaseConnection();
                role.Insert(connection);
                return View("Insertion/Index");
            }
            else
            {
                return View("Insertion/Role");
            }
        }

        public IActionResult Add(string id)
        {
            if (!IsLogged())
                return NotFound();
            ViewBag.PageLocation = PageLocation.Insertion;

            id ??= "Index";

            switch (id)
            {
                case "Index":
                    return View("Insertion/Index");
                case "Employee":
                    ViewBag.CurrentOption = "Funcionário";
                    return View("Insertion/Employee");
                case "Customer":
                    ViewBag.CurrentOption = "Cliente";
                    return View("Insertion/Customer");
                case "Package":
                    ViewBag.CurrentOption = "Pacote";
                    return View("Insertion/Package");
                case "Package_Type":
                    ViewBag.CurrentOption = "Tipo de Pacote";
                    return View("Insertion/Package_Type");
                case "Role":
                    ViewBag.CurrentOption = "Cargo";
                    return View("Insertion/Role");
                default:
                    return NotFound();
            }
        }

        [HttpGet]
        public IActionResult Query(string id)
        {
            if (!IsLogged())
                return NotFound();
            
            ViewBag.PageLocation = PageLocation.Query;

            id ??= "Index";

            TableViewModel model;

            using var connection = new DatabaseConnection();

            switch (id)
            {
                case "Index":
                    ViewBag.CurrentOption = null;
                    return View("Query/Index");

                case "Customer":
                    ViewBag.CurrentOption = "Cliente";
                    ViewBag.Header = "Informações dos clientes";
                    model = Customer.GetTable(connection);
                    break;
                case "Employee":
                    ViewBag.CurrentOption = "Funcionário";
                    ViewBag.Header = "Informações dos funcionários";
                    ViewBag.Contained = true;
                    model = Employee.GetTable(connection);
                    break;
                case "Package":
                    ViewBag.CurrentOption = "Pacote";
                    ViewBag.Header = "Informações dos pacotes";
                    model = Package.GetTable(connection);
                    break;
                case "Order":
                    ViewBag.CurrentOption = "Pedido";
                    ViewBag.Header = "Informações dos pedidos";
                    model = Order.GetTable(connection);
                    break;
                case "Session":
                    ViewBag.CurrentOption = "Sessão";
                    ViewBag.Header = "Informações das sessões";
                    model = Session.GetTable(connection);
                    break;
                case "Report":
                    ViewBag.CurrentOption = "Relatório";
                    ViewBag.Header = "Relatórios";
                    model = Models.Data.Report.GetTale(connection);
                    break;
                case "Role":
                    ViewBag.CurrentOption = "Cargo";
                    ViewBag.Header = "Cargos disponíveis";
                    model = Role.GetTable(connection);
                    break;
                default:
                    return NotFound();
            }

            return View("Query/Display", model);
        }

        public IActionResult Operation(string id)
        {
            if (!IsLogged())
                return NotFound();
            ViewBag.PageLocation = PageLocation.Operation;

            id ??= "Index";

            switch (id)
            {
                case "Index":

                    return View("Operation/Index");

                case "Order":
                    ViewBag.CurrentOption = "Fechar Pedido";
                    return View("Operation/Order");
                case "Session":
                    ViewBag.CurrentOption = "Definir Sessão";
                    return View("Operation/Session");
                case "Shift":
                    ViewBag.CurrentOption = "GerenciarTurno";
                    var connection = new DatabaseConnection();

                    TableViewModel model = new TableViewModel();
                    model.Labels = new[] {"ID", "Nome", "Horário Entrada", "Horário Saída"};
                    model.Data = EmployeeShift.GetTable(connection);
                    return View("Operation/Shift", model);

                default:
                    return NotFound();
            }
        }

        public IActionResult Report()
        {
            if (!IsLogged())
                return NotFound();
            
            ViewBag.PageLocation = PageLocation.Report;
            return View();
        }

        public IActionResult Agenda()
        {
            if (!IsLogged())
                return NotFound();
            
            var connection = new DatabaseConnection();

            ViewBag.PageLocation = PageLocation.Agenda;

            return View(Order.ListAll(connection));
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