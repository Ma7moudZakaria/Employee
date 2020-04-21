
using System.Threading.Tasks;
using Employee.Repository;
using Employee.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Employee.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly EmployeeRepository employeeRepository;
        private readonly DepartmentRepository departmentRepository;

        public EmployeesController(EmployeeRepository _employeeRepository, DepartmentRepository _departmentRepository)
        {
            employeeRepository = _employeeRepository;
            departmentRepository = _departmentRepository;
        }

        [HttpGet]
        public async Task<IActionResult> CreateDatabase()
        {
            await employeeRepository.CurrentDbContext.Database.EnsureCreatedAsync();
            return Ok();
        }

        // GET: Employee
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await employeeRepository.GetAllAsync());
        }

        // GET: Employee/Details/5
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            return View(await employeeRepository.GetByIDAsync(id));
        }

        // GET: Employee/Create       
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Employee/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EmployeeViewModel viewModel)
        {
            try
            {
                await employeeRepository.CreateAsync(viewModel);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(viewModel);
            }
        }

        // GET: Employee/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var Result = await employeeRepository.GetByIDAsync(id);
            return View(new EmployeeViewModel 
            {
                Id = Result.Id , FirstName = Result.FirstName , LastName = Result.LastName , Age = Result.Age , 
                Email = Result.Email , PhoneNumber = Result.PhoneNumber , DepartmentIDFK = Result.DepartmentIDFK 
            });
        }

        // POST: Employee/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EmployeeViewModel viewModel)
        {
            try
            {
                await employeeRepository.UpdateAsync(viewModel);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(viewModel);
            }
        }

        // GET: Employee/Delete/5
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            return View(await employeeRepository.GetByIDAsync(id));
        }

        // POST: Employee/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, IFormCollection collection)
        {
            try
            {
                await employeeRepository.DeleteAsync(await employeeRepository.GetByIDAsync(id));
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(await employeeRepository.GetByIDAsync(id));
            }
        }
    }
}