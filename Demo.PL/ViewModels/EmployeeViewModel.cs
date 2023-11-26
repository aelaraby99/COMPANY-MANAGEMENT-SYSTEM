using Demo.DAL.Models;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using System;
using Microsoft.AspNetCore.Http;

namespace Demo.PL.ViewModels
{
    public class EmployeeViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is Required"), MaxLength(50, ErrorMessage = "Max Length is 50 chars"), MinLength(3, ErrorMessage = "Min Length is 3 chars")]
        public string Name { get; set; }
        [Required]
        public IFormFile Image { get; set; }
        public string ImageName { get; set; }
        [Required, Range(22, 35, ErrorMessage = "Age  Must be Between 22 , 35 Years !")]
        public int? Age { get; set; }
        [Required, RegularExpression("^[0-9]{1,3}-[a-zA-Z]{5,10}-[a-zA-Z]{4,10}-[a-zA-Z]{5,10}$", ErrorMessage = "Address Must be Like 123-Street-City-Country")]
        public string Address { get; set; }
        [DataType(DataType.Currency)]
        public decimal Salary { get; set; }
        [Display(Name = "Status")]
        public bool IsActive { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [Phone]
        public string PhoneNumber { get; set; }
        public DateTime HireDate { get; set; }
        public int? DepartmentId { get; set; }
        public Department Department { get; set; }
    }
}
