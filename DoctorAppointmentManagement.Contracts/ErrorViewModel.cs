namespace DoctorAppointmentManagement.Models
{
    public class ErrorViewModel
    {
		public string ErrorMessage;

		public string? RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
