using Azure.Core;
using DoctorAppointmentManagement.Contracts;
using DoctorAppointmentManagement.Data;
using DoctorAppointmentManagement.Services.AppointmentServices;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace DoctorAppointmentManagement.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly ApplicationDbContext _db;

        public AppointmentService(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<bool> DoctorAppointment(Appointment appointment, ApplicationUser user)
        {
            appointment.PatientId = user.Id;

            _db.Appointments.Add(appointment);
            var result = await _db.SaveChangesAsync();

            if (result == 0)
            {
                // Log or handle the issue with saving appointment
                return false;
            }

            if (!TryParseTimestamp(appointment.Timestamp, out var date, out var startTime, out var endTime))
            {
                // Log or handle the issue with timestamp parsing
                return false;
            }

            /* var timingSlotToRemove = await _db.TimingSlots
      .Include(ts => ts.Slots)
      .Where(ts =>
          ts.DoctorId == appointment.DoctorId &&
          ts.Date == date &&
          ts.Slots.Any(timeSlot =>
              timeSlot.StartTime == startTime &&
              timeSlot.EndTime == endTime))
      .FirstOrDefaultAsync(ts =>
          ts.DoctorId == appointment.DoctorId &&
          ts.Date == date &&
          ts.Slots.Any(timeSlot =>
              timeSlot.StartTime == startTime &&
              timeSlot.EndTime == endTime));*/
           /* var timeSlotsToDelete = await _db.TimingSlots.Include
     .Where(timeSlot =>
         timeSlot.TimingSlots.DoctorId == appointment.DoctorId &&
         timeSlot.StartTime == startTime &&
         timeSlot.EndTime == endTime)
     .ToListAsync();
            if (timingSlotToRemove != null)
            {
                _db.TimingSlots.Remove(timingSlotToRemove);
                var timingResult = await _db.SaveChangesAsync();

                if (timingResult == 0)
                {
                    // Log or handle the issue with removing timing slot
                    return false;
                }
            }
            else
            {
                // Log or handle the case where no timing slot is found
                return false;
            }*/

            return true;
        }

        private bool TryParseTimestamp(string timestamp, out DateTime date, out TimeSpan startTime, out TimeSpan endTime)
        {
            date = default;
            startTime = default;
            endTime = default;

            // Your timestamp parsing logic here

            var parts = timestamp?.Split(" ");
            if (parts != null && parts.Length == 6 &&
                DateTime.TryParse(parts[0], out date) &&
                TimeSpan.TryParse(parts[3], out startTime) &&
                TimeSpan.TryParse(parts[5], out endTime))
            {
                return true;
            }

            // Log or handle the issue with timestamp parsing
            return false;
        }

    }
}