using EmployeeManagementSystem.Data;
using EmployeeManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class EmployeesApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public EmployeesApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        //PURPOSE - Get All Data 
        [HttpGet]
        public async Task<IActionResult> GetEmployees()
        {
            var employees = await _context.Employees
                .AsNoTracking()
                .ToListAsync();

            return Ok(employees);
        }

        //PURPOSE - Get Data by ID 
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployee(int id)
        {
            var employee = await _context.Employees
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == id);

            if (employee == null)
            {
                return NotFound();
            }

            return Ok(employee);
        }

        //PURPOSE - Create a New Records
        [HttpPost]
        public async Task<IActionResult> CreateEmployee(Employee employee)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Employees.Add(employee);

            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetEmployee),
                new { id = employee.Id },
                employee);
        }

        //PURPOSE - Update Data 
        //[HttpPut("{id}")]
        //public async Task<IActionResult> UpdateEmployee(int id,Employee employee)
        //{
        //    if (id != employee.Id)
        //    {
        //        return BadRequest();
        //    }

        //    var existingEmployee = await _context.Employees
        //        .FindAsync(id);

        //    if (existingEmployee == null)
        //    {
        //        return NotFound();
        //    }

        //    existingEmployee.Name = employee.Name;
        //    existingEmployee.Email = employee.Email;
        //    existingEmployee.Phone = employee.Phone;
        //    existingEmployee.Department = employee.Department;
        //    existingEmployee.Salary = employee.Salary;
        //    existingEmployee.JoiningDate = employee.JoiningDate;
        //    existingEmployee.IsActive = employee.IsActive;

        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmployee(int id, Employee employee)
        {
            var existingEmployee = await _context.Employees.FindAsync(id);

            if (existingEmployee == null)
            {
                return NotFound();
            }

            existingEmployee.Name = employee.Name;
            existingEmployee.Email = employee.Email;
            existingEmployee.Phone = employee.Phone;
            existingEmployee.Department = employee.Department;
            existingEmployee.Salary = employee.Salary;
            existingEmployee.JoiningDate = employee.JoiningDate;
            existingEmployee.IsActive = employee.IsActive;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        //PURPOSE - Delete Data 
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var employee = await _context.Employees.FindAsync(id);

            if (employee == null)
            {
                return NotFound();
            }

            _context.Employees.Remove(employee);

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}