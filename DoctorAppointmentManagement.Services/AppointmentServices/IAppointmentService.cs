using DoctorAppointmentManagement.Contracts;
using DoctorAppointmentManagement.Data;
using Microsoft.AspNetCore.Mvc;


namespace DoctorAppointmentManagement.Services.AppointmentServices
{
    public interface IAppointmentService
    {
        public Task<IActionResult> DoctorAppointment(Appointment appointment,ApplicationUser user,int DoctorId);
    }
}
