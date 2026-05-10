using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Employeee.Models
{
    public class Department
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}