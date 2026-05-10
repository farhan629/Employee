using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Employeee.Dto;
using Employeee.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.Extensions.Logging;

namespace Employeee.Controller
{
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly EmployeeDbcContexet _context;

        public EmployeeController(EmployeeDbcContexet context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult GetEmployees([FromQuery] Guid Id)
        {
            
            if(Id == Guid.Empty)
            {
                var employees = _context.Employees.Include(e => e.Department).ToList();
                List<EmployeeDto> employeeDtos = new List<EmployeeDto>();
                foreach(var employee in employees)                {
                    EmployeeDto employeeDto = new EmployeeDto();
                    employeeDto.Id = employee.Id;
                    employeeDto.Name = employee.Name;
                    employeeDto.Salary = employee.Salary;
                    employeeDto.DepartmentName = employee.Department.Name;
                    employeeDtos.Add(employeeDto);
                }
                return Ok(employeeDtos);
            }
            else
            {
                var employee = _context.Employees.Include(e => e.Department).FirstOrDefault(e => e.Id == Id);
                EmployeeDto employeeDto = new EmployeeDto();
                        employeeDto.Id = employee.Id;
                        employeeDto.Name = employee.Name;
                        employeeDto.Salary = employee.Salary;
                        employeeDto.DepartmentName = employee.Department.Name;  

                if(employee == null)
                {
                    return NotFound();
                }
                return Ok(employeeDto);
            }
        }


        [HttpPost]
        public IActionResult CreateEmployee([FromBody] EmployeeRegisterDto employee)
        {
            if(employee == null)
            {
                return BadRequest();
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

        [HttpPost("/deptartment")]
        public IActionResult CreateDepartment([FromBody] Department department)
        {
            if(department == null)
            {
                return BadRequest();
            }
            department.Id = Guid.NewGuid();
            _context.Departments.Add(department);
            _context.SaveChanges();
            return Ok(department);
        }

        [HttpGet("/deptartment")]
        public IActionResult GetDepartments([FromQuery] Guid Id)
        {
            if(Id == Guid.Empty)
            {
                List<Department> departments = _context.Departments.ToList();
                return Ok(departments);
            }
            else
            {
                var department = _context.Departments.FirstOrDefault(d => d.Id == Id);
                if(department == null)
                {
                    return NotFound();
                }
                return Ok(department);
            }
        }


        [HttpPut("{id}")]
        public IActionResult UpdateEmployee(Guid id, [FromBody] Employee employee)
        {
            if(employee == null)
            {
                return BadRequest();
            }
            var existingEmployee = _context.Employees.FirstOrDefault(e => e.Id == id);
            if(existingEmployee == null)
            {
                return NotFound();
            }
            existingEmployee.Name = employee.Name;
            existingEmployee.Salary = employee.Salary;
            _context.SaveChanges();
            return Ok(existingEmployee);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteEmployee(Guid id)
        {
            var existingEmployee = _context.Employees.FirstOrDefault(e => e.Id == id);
            if(existingEmployee == null)
            {
                return NotFound();
            }
            _context.Employees.Remove(existingEmployee);
            _context.SaveChanges();
            return Ok();
        }
    }
}