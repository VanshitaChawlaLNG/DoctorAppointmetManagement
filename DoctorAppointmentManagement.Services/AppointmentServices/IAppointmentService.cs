using DoctorAppointmentManagement.Contracts;
using DoctorAppointmentManagement.Data;
using Microsoft.AspNetCore.Mvc;


namespace DoctorAppointmentManagement.Services.AppointmentServices
{
    public interface IAppointmentService
    {
        public Task<bool> DoctorAppointment(Contracts.Appointment appointment,ApplicationUser user);
    }
}

