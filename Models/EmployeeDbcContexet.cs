using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Employeee.Models
{
    public class EmployeeDbcContexet : DbContext
    {
        public EmployeeDbcContexet(DbContextOptions<EmployeeDbcContexet> options) : base(options)
        {

        }

        
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Department> Departments { get; set; }
        
    }
}