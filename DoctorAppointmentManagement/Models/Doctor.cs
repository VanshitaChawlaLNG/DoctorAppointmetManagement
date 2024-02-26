using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoctorAppointmentManagement.Models
{
    public class Doctor
    {

        
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        [Required]

       
        public String Email {  get; set; }

        [Required]
        public string Password { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public int Fees { get; set; }

        [Display(Name = "Profile Picture")]
        [NotMapped]
        public IFormFile ProfilePictureFile { get; set; }

        public string ProfilePicture { get; set; }

    }
}
