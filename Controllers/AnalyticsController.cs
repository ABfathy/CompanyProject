using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EmployeeManagementMVC.Data;
using EmployeeManagementMVC.Models;
using Microsoft.AspNetCore.Authorization;

namespace EmployeeManagementMVC.Controllers
{
    [Authorize]
    public class AnalyticsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AnalyticsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Analytics
        public async Task<IActionResult> Index()
        {
            var departments = await _context.Departments
                .Include(d => d.Employees)
                .ToListAsync();

            var analyticsViewModel = new AnalyticsViewModel
            {
                Departments = departments.Select(d => new DepartmentAnalyticsViewModel
                {
                    Id = d.Id,
                    Name = d.Name,
                    Description = d.Description,
                    Employees = d.Employees.ToList()
                }).ToList()
            };

            return View(analyticsViewModel);
        }

        // GET: Analytics/DepartmentDetails/5
        public async Task<IActionResult> DepartmentDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var department = await _context.Departments
                .Include(d => d.Employees)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (department == null)
            {
                return NotFound();
            }

            var viewModel = new DepartmentAnalyticsViewModel
            {
                Id = department.Id,
                Name = department.Name,
                Description = department.Description,
                Employees = department.Employees.ToList()
            };

            return View(viewModel);
        }
    }
}