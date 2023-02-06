using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DapperDemo.Data;
using DapperDemo.Models;
using DapperDemo.Repository.IRepository;

namespace DapperDemo.Controllers
{
    public class CompaniesController : Controller
    {
        private readonly IEmployeeRepository _employeeRepo;
        private readonly ICompanyRepository _companyRepo;
        private readonly IBonusRepository _bonusRepo;
        private readonly IDapperSprocRepo _dapperRepo;


        public CompaniesController(ICompanyRepository companyRepo, IBonusRepository bonusRepository, IEmployeeRepository employeeRepository, IDapperSprocRepo dapperRepo)
        {
            _companyRepo = companyRepo;
            _bonusRepo = bonusRepository;
            _employeeRepo = employeeRepository;
            _dapperRepo = dapperRepo;
        }

        // GET: Companies
        public async Task<IActionResult> Index()
        {
            //return View(_companyRepo.FindAll());
            return View(_dapperRepo.List<Company>("usp_GetALLCompany"));
        }

        // GET: Companies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var company = _bonusRepo.GetCompanyWithAddresses(id.GetValueOrDefault());
            if (company == null)
            {
                return NotFound();
            }

            return View(company);
        }

        // GET: Companies/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Companies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CompanyId,Name,Address,City,State,PostalCode")] Company company)
        {
            if (ModelState.IsValid)
            {
                _companyRepo.Add(company);
                return RedirectToAction(nameof(Index));
            }
            return View(company);
        }

        // GET: Companies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var company =_companyRepo.Find(id.Value);
            var company = _dapperRepo.Single<Company>("usp_GetCompany", new { CompanyId = id.GetValueOrDefault()});
            if (company == null)
            {
                return NotFound();
            }
            return View(company);
        }

        // POST: Companies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CompanyId,Name,Address,City,State,PostalCode")] Company company)
        {
            if (id != company.CompanyId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _companyRepo.Update(company);

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CompanyExists(company.CompanyId))
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
            return View(company);
        }

        // GET: Companies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var company = _companyRepo.Find(id.Value);  
            if (company == null)
            {
                return NotFound();
            }

            return View(company);
        }

        // POST: Companies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {

            var company = _companyRepo.Find(id);
            if (company != null)
            {
                _companyRepo.Remove(id);
            }
            

            return RedirectToAction(nameof(Index));
        }

        private bool CompanyExists(int id)
        {
          return _companyRepo.Find(id) != null;
        }
    }
}
