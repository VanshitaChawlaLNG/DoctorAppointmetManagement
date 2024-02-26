using System.ComponentModel.DataAnnotations;

namespace DoctorAppointmentManagement.Models
{
    public class AvailableTiming
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Day { get; set; }
        [Required]
        public string StartTime { get; set; }
        [Required]
        public string EndTime { get; set; }

        [Required]
      
        public string DoctorId { get; set; }
        //public Doctor Doctor { get; set; }
    }
}
