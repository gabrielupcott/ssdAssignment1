// Assignment1/Models/Employer.cs
using System;
using System.ComponentModel.DataAnnotations;

namespace Assignment1.Models
{
    public class Employer
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Employer Name")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
        public string Name { get; set; }

        [Required]
        [Phone]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Required]
        [Url]
        public string Website { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Incorporated Date")]
        public DateTime? IncorporatedDate { get; set; }
    }
}
