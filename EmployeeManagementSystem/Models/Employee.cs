using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementSystem.Models
{
    public class Employee
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Phone { get; set; }

        [Required]
        public string Department { get; set; }

        [Range(0, 10000000)]
        public decimal Salary { get; set; }

        [DataType(DataType.Date)]
        public DateTime JoiningDate { get; set; }

        public bool IsActive { get; set; }
    }
}
