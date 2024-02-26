using Microsoft.AspNetCore.Mvc;


namespace DoctorAppointmentManagement.Services.Appointment
{
    public interface IAppointmentServices
    {
        public Task<IActionResult> DoctorAppointment(int id);
        
    }
}
