using DapperDemo.Models;
using DapperDemo.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop.Implementation;
using System.Runtime.CompilerServices;

namespace DapperDemo.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeRepository _employeeRepo;
        private readonly ICompanyRepository _companyRepo;
        private readonly IBonusRepository _bonusRepo;

        [BindProperty]
        public Employee Employee { get; set; }
        public EmployeeController(IEmployeeRepository employeeRepository, ICompanyRepository companyRepository, IBonusRepository bonusRepo)
        {
            _employeeRepo = employeeRepository;
            _companyRepo = companyRepository;
            _bonusRepo = bonusRepo; 
        }
        public IActionResult Index(int companyId = 0)
        {
            var employees = _bonusRepo.GetEmployeeWithCompanies(companyId);
            //foreach (var employee in employees)
            //{
            //    employee.Company = _companyRepo.Find(employee.CompanyId);
            //}
            return View(employees);
        }
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = _employeeRepo.Find(id.Value);
            if (employee == null)
            {
                return NotFound();
            }
            IEnumerable<SelectListItem> companyList = _companyRepo.FindAll().Select(it => new SelectListItem
            {
                Text = it.Name,
                Value = it.CompanyId.ToString()
            });
            ViewBag.CompanyList = companyList;
            return View(employee);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id)
        {
            if (id != Employee.CompanyId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _employeeRepo.Update(Employee);

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeExists(Employee.EmployeeId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
     
            return View(Employee);
        }
        public IActionResult Create()
        {
            IEnumerable<SelectListItem> companyList = _companyRepo.FindAll().Select(it => new SelectListItem
            {
                Text = it.Name,
                Value = it.CompanyId.ToString()
            });
            ViewBag.CompanyList = companyList;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Create")]
        public async Task<IActionResult> CreatePOST()
        {
            if (ModelState.IsValid)
            {
                _employeeRepo.Add(Employee);
                return RedirectToAction(nameof(Index));
            }
            return View(Employee);
        }

        private bool EmployeeExists(int id)
        {
            return _employeeRepo.Find(id) != null;
        }

        [ActionName("Delete")]
        public async Task<IActionResult> Delete(int id)
        {

            var employee = _employeeRepo.Find(id);
            if (employee != null)
            {
                _employeeRepo.Remove(id);
            }


            return RedirectToAction(nameof(Index));
        }


    }
}
