using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoctorAppointmentManagement.Contracts
{
    public class Doctor
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Fees is required")]
        [Range(0, int.MaxValue, ErrorMessage = "Fees must be a non-negative number")]
        public int Fees { get; set; }

        [Display(Name = "Profile Picture")]
        [NotMapped]
       
        [AllowedExtensions(new string[] { ".jpg", ".jpeg", ".png" }, ErrorMessage = "Only JPG, JPEG, and PNG files are allowed")]
        public IFormFile? ProfilePictureFile { get; set; }

        public string? ProfilePicture { get; set; }

        public IEnumerable<TimingSlots>? timingSlots { get; set; }
    }
}