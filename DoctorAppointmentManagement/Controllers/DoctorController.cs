using DoctorAppointmentManagement.Contracts;
using DoctorAppointmentManagement.Data;
using DoctorAppointmentManagement.Models;
using DoctorAppointmentManagement.Services.AddTiming;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DoctorAppointmentManagement.Controllers
{
    [Authorize(Roles = "Doctors")]
    public class DoctorController : Controller
    {
        private readonly ITimingServices _timingServices;
        private readonly ApplicationDbContext _db;

        public DoctorController(ITimingServices timingServices, ApplicationDbContext db)
        {
            _timingServices = timingServices;
            _db=db;
        }
        public IActionResult AddTiming()
        {
            return View();
        }
        public IActionResult AddTiming(AvailableTiming availableTiming )
        {   
            var timing = _timingServices.AddAvailableTimings(availableTiming);
            return View();
        }
    }
}
