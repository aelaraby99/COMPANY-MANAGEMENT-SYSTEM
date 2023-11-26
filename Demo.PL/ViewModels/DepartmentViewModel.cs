using Demo.DAL.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;

namespace Demo.PL.ViewModels
{
    public class DepartmentViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Code is Required!!") , MinLength(3  , ErrorMessage ="Code must be more than 3 Characters")]
        public string Code { get; set; }
        [Required(ErrorMessage = "Name is Required!!"), MaxLength(50) , MinLength(2 , ErrorMessage = "Department Name must be more than 3 Characters")]
        public string Name { get; set; }
        public DateTime DateOfCreation { get; set; }
        public ICollection<Employee> Employees { get; set; } = new HashSet<Employee>();
    }
}
