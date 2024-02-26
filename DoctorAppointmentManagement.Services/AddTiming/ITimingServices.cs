using DoctorAppointmentManagement.Contracts;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorAppointmentManagement.Services.AddTiming
{
    public interface ITimingServices
    {
        public Task<IActionResult> AddAvailableTimings(AvailableTiming availableTiming);
    }
}
