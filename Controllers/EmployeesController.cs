// Controllers/EmployeesController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EmployeeManagementMVC.Data;
using EmployeeManagementMVC.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

[Authorize]
public class EmployeesController : Controller
{
    private readonly ApplicationDbContext _context;

    public EmployeesController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: Employees
    public async Task<IActionResult> Index()
    {
        var employees = _context.Employees.Include(e => e.Department);
        return View(await employees.ToListAsync());
    }

    // GET: Employees/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var employee = await _context.Employees
            .Include(e => e.Department)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (employee == null)
        {
            return NotFound();
        }

        return View(employee);
    }

    // GET: Employees/Create
    public IActionResult Create()
    {
        ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Name");
        return View();
    }

    // POST: Employees/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,Position,HireDate,Salary,Email,PhoneNumber,DepartmentId")] Employee employee)
    {
        // Debug information
        Console.WriteLine("Create POST action triggered");
        Console.WriteLine($"Model is valid: {ModelState.IsValid}");
        Console.WriteLine($"Department ID: {employee.DepartmentId}");
        
        // The validation is failing for DepartmentId even though it has a value
        // This could be due to a model binding issue, so let's manually validate it
        if (employee.DepartmentId > 0)
        {
            // Ensure that any error related to DepartmentId is removed
            ModelState.Remove("Department");
            ModelState.Remove("DepartmentId");
        }
        
        if (!ModelState.IsValid)
        {
            foreach (var state in ModelState)
            {
                if (state.Value.Errors.Count > 0)
                {
                    Console.WriteLine($"Error in {state.Key}: {string.Join(", ", state.Value.Errors.Select(e => e.ErrorMessage))}");
                }
            }
        }
        
        // Re-check model state after manually handling DepartmentId
        if (ModelState.IsValid)
        {
            try
            {
                Console.WriteLine("Adding employee to database");
                // Add the employee to the database
                _context.Employees.Add(employee);
                await _context.SaveChangesAsync();
                Console.WriteLine("Employee saved successfully");
                // Redirect to the index page upon successful creation
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error saving employee: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                ModelState.AddModelError("", "Error creating employee: " + ex.Message);
            }
        }
        
        // If we get here, something failed, so redisplay the form
        Console.WriteLine("Redisplaying form");
        // Make sure to populate the dropdown with departments
        ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Name", employee.DepartmentId);
        return View(employee);
    }

    // GET: Employees/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var employee = await _context.Employees.FindAsync(id);
        if (employee == null)
        {
            return NotFound();
        }
        ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Name", employee.DepartmentId);
        return View(employee);
    }

    // POST: Employees/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,Position,HireDate,Salary,Email,PhoneNumber,DepartmentId")] Employee employee)
    {
        if (id != employee.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(employee);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeExists(employee.Id))
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
        ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Name", employee.DepartmentId);
        return View(employee);
    }

    // GET: Employees/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var employee = await _context.Employees
            .Include(e => e.Department)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (employee == null)
        {
            return NotFound();
        }

        return View(employee);
    }

    // POST: Employees/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var employee = await _context.Employees.FindAsync(id);
        if (employee != null)
        {
            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }

    private bool EmployeeExists(int id)
    {
        return _context.Employees.Any(e => e.Id == id);
    }
}
