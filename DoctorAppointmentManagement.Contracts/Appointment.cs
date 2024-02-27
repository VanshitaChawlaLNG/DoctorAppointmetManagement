using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.General;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoctorAppointmentManagement.Contracts
{
	public class Appointment
    {
     
        public int Id { get; set; }

        [Required]
        public int DoctorId { get; set; }

        [Required]

        public string PatientName { get; set; }

        
        public int PatientId { get; set; }
        
      
       /* public Doctor Doctor { get; set; }*/

        [Required]
        public DateTime Timestamp { get; set; }

      
       

    }
}
