using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Employeee.Dto
{
    public class EmployeeDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Salary { get; set; }
        public string DepartmentName { get; set; }
    }

    public class EmployeeRegisterDto
    {
        public string Name { get; set; }
        public decimal Salary { get; set; }
        public Guid DepartmentId { get; set; }
    }
}