using System.Collections.Generic;

namespace EmployeeManagementMVC.Models
{
    public class DepartmentAnalyticsViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Employee> Employees { get; set; }
        public int EmployeeCount => Employees?.Count ?? 0;
        public decimal TotalSalary => Employees?.Sum(e => e.Salary) ?? 0;
        public decimal AverageSalary => EmployeeCount > 0 ? TotalSalary / EmployeeCount : 0;
    }

    public class AnalyticsViewModel
    {
        public List<DepartmentAnalyticsViewModel> Departments { get; set; }
        public int TotalEmployees => Departments?.Sum(d => d.EmployeeCount) ?? 0;
        public int TotalDepartments => Departments?.Count ?? 0;
        public decimal TotalOrganizationSalary => Departments?.Sum(d => d.TotalSalary) ?? 0;
    }
}