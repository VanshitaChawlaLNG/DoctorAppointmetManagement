using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorAppointmentManagement.Services.Appointment
{
    public interface IAppointmentServices
    {
        public Task<IActionResult> DoctorAppointment(int id);
        
    }
}
