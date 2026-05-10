using System;
using System.Collections.Generic;
using System.Linq;
using Employeee.Dto;
using Employeee.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Employeee.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly EmployeeDbcContexet _context;

        public EmployeeController(EmployeeDbcContexet context)
        {
            _context = context;
        }

        // GET api/Employee
        // GET api/Employee?id=guid
        [HttpGet]
        public ActionResult GetEmployees([FromQuery] Guid Id)
        {
            try
            {
                // Get All Employees
                if (Id == Guid.Empty)
                {
                    var employees = _context.Employees
                        .Include(e => e.Department)
                        .ToList();

                    List<EmployeeDto> employeeDtos = new List<EmployeeDto>();

                    foreach (var employee in employees)
                    {
                        EmployeeDto employeeDto = new EmployeeDto();

                        employeeDto.Id = employee.Id;
                        employeeDto.Name = employee.Name;
                        employeeDto.Salary = employee.Salary;

                        employeeDto.DepartmentName =
                            employee.Department != null
                            ? employee.Department.Name
                            : "No Department";

                        employeeDtos.Add(employeeDto);
                    }

                    return Ok(employeeDtos);
                }

                // Get Single Employee
                var singleEmployee = _context.Employees
                    .Include(e => e.Department)
                    .FirstOrDefault(e => e.Id == Id);

                if (singleEmployee == null)
                {
                    return NotFound();
                }

                EmployeeDto dto = new EmployeeDto();

                dto.Id = singleEmployee.Id;
                dto.Name = singleEmployee.Name;
                dto.Salary = singleEmployee.Salary;

                dto.DepartmentName =
                    singleEmployee.Department != null
                    ? singleEmployee.Department.Name
                    : "No Department";

                return Ok(dto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.ToString());
            }
        }

        // POST api/Employee
        [HttpPost]
        public IActionResult CreateEmployee([FromBody] EmployeeRegisterDto employee)
        {
            try
            {
                if (employee == null)
                {
                    return BadRequest();
                }

                var departmentExists = _context.Departments
                    .Any(d => d.Id == employee.DepartmentId);

                if (!departmentExists)
                {
                    return BadRequest("Department not found");
                }

                var newEmployee = new Employee
                {
                    Id = Guid.NewGuid(),
                    Name = employee.Name,
                    Salary = employee.Salary,
                    DepartmentId = employee.DepartmentId
                };

                _context.Employees.Add(newEmployee);
                _context.SaveChanges();

                return Ok(newEmployee);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.ToString());
            }
        }

        // POST deptartment
        [HttpPost("/deptartment")]
        public IActionResult CreateDepartment([FromBody] Department department)
        {
            try
            {
                if (department == null)
                {
                    return BadRequest();
                }

                department.Id = Guid.NewGuid();

                _context.Departments.Add(department);
                _context.SaveChanges();

                return Ok(department);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.ToString());
            }
        }

        // GET deptartment
        [HttpGet("/deptartment")]
        public IActionResult GetDepartments([FromQuery] Guid Id)
        {
            try
            {
                // Get All Departments
                if (Id == Guid.Empty)
                {
                    List<Department> departments = _context.Departments.ToList();

                    return Ok(departments);
                }

                // Get Single Department
                var department = _context.Departments
                    .FirstOrDefault(d => d.Id == Id);

                if (department == null)
                {
                    return NotFound();
                }

                return Ok(department);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.ToString());
            }
        }

        // PUT api/Employee/{id}
        [HttpPut("{id}")]
        public IActionResult UpdateEmployee(Guid id, [FromBody] Employee employee)
        {
            try
            {
                if (employee == null)
                {
                    return BadRequest();
                }

                var existingEmployee = _context.Employees
                    .FirstOrDefault(e => e.Id == id);

                if (existingEmployee == null)
                {
                    return NotFound();
                }

                existingEmployee.Name = employee.Name;
                existingEmployee.Salary = employee.Salary;
                existingEmployee.DepartmentId = employee.DepartmentId;

                _context.SaveChanges();

                return Ok(existingEmployee);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.ToString());
            }
        }

        // DELETE api/Employee/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteEmployee(Guid id)
        {
            try
            {
                var existingEmployee = _context.Employees
                    .FirstOrDefault(e => e.Id == id);

                if (existingEmployee == null)
                {
                    return NotFound();
                }

                _context.Employees.Remove(existingEmployee);

                _context.SaveChanges();

                return Ok("Employee Deleted Successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.ToString());
            }
        }
    }
}