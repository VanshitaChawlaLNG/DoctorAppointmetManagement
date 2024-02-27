using DoctorAppointmentManagement.Contracts;
using DoctorAppointmentManagement.Data;
using DoctorAppointmentManagement.Services.AppointmentServices;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorAppointmentManagement.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly ApplicationDbContext _db;

        public AppointmentService(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<IActionResult> DoctorAppointment(Appointment appointment, ApplicationUser user, int DoctorId)
        {
            try
            {
                appointment.DoctorId = DoctorId;
                _db.Appointments.Add(appointment);
                await _db.SaveChangesAsync();

                return new OkResult();
            }
            catch (DbUpdateException ex)
            {

                Console.WriteLine($"Database update error: {ex.Message}");
                return new StatusCodeResult(500);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while adding available timings: {ex.Message}");
                return new StatusCodeResult(500);
            }
        }

    }
}
