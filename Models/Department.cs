using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementMVC.Models
{
    public class Department
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Department name is required.")]
        [StringLength(100, ErrorMessage = "Department name cannot exceed 100 characters.")]
        [Display(Name = "Department Name")] 
        public string Name { get; set; }

        [StringLength(500, ErrorMessage = "Department description cannot exceed 500 characters.")]
        [Display(Name = "Description")]
        public string Description { get; set; }

        public ICollection<Employee> Employees { get; set; } = new List<Employee>();
    }
}