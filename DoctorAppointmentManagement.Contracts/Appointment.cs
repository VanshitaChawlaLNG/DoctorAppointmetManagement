using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.General;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoctorAppointmentManagement.Contracts
{
    public class Appointment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int DoctorId { get; set; }

        [Required]

        public string PatientName { get; set; }


        public string PatientId { get; set; }


        /* public Doctor Doctor { get; set; }*/

        [Required]
        public string Timestamp { get; set; }
        /*[Required]
        public TimeSpan StartTime { get; set; }
        [Required]

        public TimeSpan EndTime { get; set; }

*/

    }
}
