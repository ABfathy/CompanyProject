// Controllers/DepartmentsController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EmployeeManagementMVC.Data;
using EmployeeManagementMVC.Models; 
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

[Authorize]
public class DepartmentsController : Controller
{
    private readonly ApplicationDbContext _context;

    // Constructor to inject the DbContext
    public DepartmentsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: Departments
    // Displays a list of all departments
    public async Task<IActionResult> Index()
    {
        // Retrieve all departments from the database asynchronously
        var departments = await _context.Departments.ToListAsync();
        // Pass the list of departments to the Index view
        return View(departments);
    }

    // GET: Departments/Details/5
    // Displays the details of a specific department
    public async Task<IActionResult> Details(int? id)
    {
        // Check if an ID was provided
        if (id == null)
        {
            return NotFound(); // Return 404 if no ID is provided
        }

        // Find the department by ID asynchronously
        var department = await _context.Departments
            .FirstOrDefaultAsync(m => m.Id == id);

        // Check if the department was found
        if (department == null)
        {
            return NotFound(); // Return 404 if department is not found
        }

        // Pass the department object to the Details view
        return View(department);
    }

    // GET: Departments/Create
    // Displays the form to create a new department
    public IActionResult Create()
    {
        return View(); // Simply return the Create view
    }

    // POST: Departments/Create
    // Handles the submission of the new department form
    [HttpPost]
    [ValidateAntiForgeryToken] // Protects against cross-site request forgery attacks
    public async Task<IActionResult> Create([Bind("Id,Name,Description")] Department department)
    {
        // Check if the submitted model is valid based on data annotations
        if (ModelState.IsValid)
        {
            // Add the new department to the DbContext
            _context.Add(department);
            // Save the changes to the database asynchronously
            await _context.SaveChangesAsync();
            // Redirect to the Index action after successful creation
            return RedirectToAction(nameof(Index));
        }
        // If the model is not valid, return the Create view with validation errors
        return View(department);
    }

    // GET: Departments/Edit/5
    // Displays the form to edit an existing department
    public async Task<IActionResult> Edit(int? id)
    {
        // Check if an ID was provided
        if (id == null)
        {
            return NotFound();
        }

        // Find the department by ID asynchronously
        var department = await _context.Departments.FindAsync(id);

        // Check if the department was found
        if (department == null)
        {
            return NotFound();
        }

        // Pass the department object to the Edit view
        return View(department);
    }

    // POST: Departments/Edit/5
    // Handles the submission of the edit department form
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description")] Department department)
    {
        // Check if the provided ID matches the department ID
        if (id != department.Id)
        {
            return NotFound();
        }

        // Check if the submitted model is valid
        if (ModelState.IsValid)
        {
            try
            {
                // Update the department in the DbContext
                _context.Update(department);
                // Save the changes to the database asynchronously
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                // Handle concurrency conflicts (if the record was changed by someone else)
                if (!DepartmentExists(department.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw; // Re-throw the exception if it's not a "not found" issue
                }
            }
            // Redirect to the Index action after successful update
            return RedirectToAction(nameof(Index));
        }
        // If the model is not valid, return the Edit view with validation errors
        return View(department);
    }

    // GET: Departments/Delete/5
    // Displays the confirmation page to delete a department
    public async Task<IActionResult> Delete(int? id)
    {
        // Check if an ID was provided
        if (id == null)
        {
            return NotFound();
        }

        // Find the department by ID, including related employees for display (optional but helpful)
        var department = await _context.Departments
            .Include(d => d.Employees) // Include employees to check if the department has any
            .FirstOrDefaultAsync(m => m.Id == id);

        // Check if the department was found
        if (department == null)
        {
            return NotFound();
        }

        return View(department); // Pass the department to the Delete view
    }

    // POST: Departments/Delete/5
    // Handles the confirmation of deleting a department
    [HttpPost, ActionName("Delete")] // Specify the action name for routing
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        // Find the department by ID
        var department = await _context.Departments.FindAsync(id);

        // Check if the department exists before attempting to remove
        if (department != null)
        {
             // Check if the department has any employees before deleting (based on OnDelete(DeleteBehavior.Restrict))
            var employeeCount = await _context.Employees.CountAsync(e => e.DepartmentId == id);
            if (employeeCount > 0)
            {
                // Add a model error if the department has employees and cannot be deleted
                ModelState.AddModelError("", "Cannot delete this department because it has associated employees.");
                // Re-fetch the department with employees to display on the delete view again
                 department = await _context.Departments
                    .Include(d => d.Employees)
                    .FirstOrDefaultAsync(m => m.Id == id);
                 return View("Delete", department); // Return the delete view with the error
            }

            // Remove the department from the DbContext
            _context.Departments.Remove(department);
        }

        // Save the changes to the database asynchronously
        await _context.SaveChangesAsync();
        // Redirect to the Index action after successful deletion
        return RedirectToAction(nameof(Index));
    }

    // Helper method to check if a department exists
    private bool DepartmentExists(int id)
    {
        return _context.Departments.Any(e => e.Id == id);
    }
}
