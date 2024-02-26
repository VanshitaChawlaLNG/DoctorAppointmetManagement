namespace DoctorAppointmentManagement.Contracts
{
	public class Appointment
    {
       // public ApplicationUser ApplicationUser { get; set; }
      
        public Doctor Doctor { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
