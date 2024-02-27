using System.ComponentModel.DataAnnotations;

namespace DoctorAppointmentManagement.Contracts
{
	public class Appointment
    {
     
        public int Id { get; set; }

        [Required]
        public int DoctorId { get; set; }

        [Required]
        public int PatientId { get; set; }
      
        public Doctor Doctor { get; set; }

        [Required]
        public DateTime Timestampstart { get; set; }

        [Required]
        public DateTime TimestampEnd { get; set; }
        [Required]
        public DateTime Date { get; set; }

    }
}
