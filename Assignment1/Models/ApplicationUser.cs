// Assignment1/Models/ApplicationUser.cs
using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;

namespace Assignment1.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [Display(Name = "First Name")]
        [StringLength(50, ErrorMessage = "First Name cannot exceed 50 characters.")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        [StringLength(50, ErrorMessage = "Last Name cannot exceed 50 characters.")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "User Name")]
        [StringLength(50, ErrorMessage = "User Name cannot exceed 50 characters.")]
        public override string UserName { get; set; }

        [Required]
        [EmailAddress]
        public override string Email { get; set; }

        [Phone]
        [Display(Name = "Phone Number")]
        public override string PhoneNumber { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Birth Date")]
        public DateTime? BirthDate { get; set; }
    }
}
