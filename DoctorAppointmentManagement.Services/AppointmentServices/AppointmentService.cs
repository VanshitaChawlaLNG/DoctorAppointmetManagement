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
        public async Task<bool> DoctorAppointment(Contracts.Appointment appointment, ApplicationUser user)
        {
            try
            {
                appointment.PatientId = user.Id;

                _db.Appointments.Add(appointment);
                var result = await _db.SaveChangesAsync();

                if (result == 0)
                {
                    
                    return false;
                }

                if (!TryParseTimestamp(appointment.Timestamp, out var date, out var startTime, out var endTime))
                {
                    // Log or handle the issue with timestamp parsing
                    return false;
                }

                // Fetch TimingSlotsId using DoctorId
                var timingSlotsId = await _db.TimingSlots
                    .Where(ts => ts.DoctorId == appointment.DoctorId && ts.Date == date)
                    .Select(ts => ts.Id)
                    .FirstOrDefaultAsync();

                if (timingSlotsId != 0)
                {
                 
                    var slotsToDelete = await _db.Slots
                        .Where(slot =>
                            slot.TimingSlotsId == timingSlotsId &&
                            slot.StartTime == startTime &&
                            slot.EndTime == endTime)
                        .ToListAsync();

                    // Remove Slots
                    _db.Slots.RemoveRange(slotsToDelete);

                   
                    // Save changes
                    var saveResult = await _db.SaveChangesAsync();

                    if (saveResult == 0)
                    {
                       
                        return false;
                    }
                }
                else
                {
                   
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                // Log the exception
                return false;
            }
        }


        private bool TryParseTimestamp(string timestamp, out DateTime date, out TimeSpan startTime, out TimeSpan endTime)
        {
            date = default;
            startTime = default;
            endTime = default;

            // Your timestamp parsing logic here

            var parts = timestamp?.Split(" ");
            if (parts != null &&
                DateTime.TryParse(parts[0], out date) &&
                TimeSpan.TryParse(parts[1], out startTime) &&
                TimeSpan.TryParse(parts[3], out endTime))
            {
                return true;
            }

            // Log or handle the issue with timestamp parsing
            return false;
        }

    }
}