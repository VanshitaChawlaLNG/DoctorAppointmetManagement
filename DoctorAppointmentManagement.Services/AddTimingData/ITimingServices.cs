﻿using DoctorAppointmentManagement.Contracts;
using DoctorAppointmentManagement.Data;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;


namespace DoctorAppointmentManagement.Services.AddTimingData
{
    public interface ITimingServices
    {
        public Task<IActionResult> AddAvailableTimings(AvailableTiming availableTiming, ApplicationUser user);
    }
}