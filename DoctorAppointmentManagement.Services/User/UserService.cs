using DoctorAppointmentManagement.Contracts;
using DoctorAppointmentManagement.Data;
using DoctorAppointmentManagement.Services.AppointmentServices;
using JasperFx.CodeGeneration;
using JasperFx.CodeGeneration.Frames;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Options;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorAppointmentManagement.Services.User
{
    public class UserService:IUserService
    {
        
        private readonly ApplicationDbContext _db;

        public UserService(ApplicationDbContext db)
        {
            _db = db;
        }

        /*   public async Task<IEnumerable<string>> FetchTimingsToShow(int doctorId)
           {
               IEnumerable<TimingSlots> timingSlots =await _db.TimingSlots
                   .Include(ts => ts.Slots)
                   .Where(ts => ts.DoctorId == doctorId)
                   .ToListAsync();

               List<string> timingSlotsInString = new List<string>();

               foreach (var timingSlot in timingSlots)
               {
                   if (timingSlot.Slots != null)
                   {
                       foreach (var timeSlot in timingSlot.Slots)
                       {
                           // Add null checks to prevent NullReferenceException
                           if (timeSlot != null && timeSlot.TimingSlots != null)
                           {
                               var trimmedDate = timeSlot.TimingSlots.Date;
                               var startTimeFormatted = $"{(int)timeSlot.StartTime.TotalHours:D2}:{timeSlot.StartTime.Minutes:D2}";
                               var endTimeFormatted = $"{(int)timeSlot.EndTime.TotalHours:D2}:{timeSlot.EndTime.Minutes:D2}";

                               var dateTimeString = $"{trimmedDate} {startTimeFormatted} - {endTimeFormatted}";

                               timingSlotsInString.Add(dateTimeString);
                           }
                       }
                   }
               }

               // Return the timing slots as strings
               return timingSlotsInString;
           }
        */
        public async Task<IEnumerable<string>> FetchTimingsToShow(int doctorId)
        {
            var timingSlotsInString = await _db.TimingSlots
                .Where(ts => ts.DoctorId == doctorId)
                .Join(
                    _db.Slots,
                    timingSlot => timingSlot.Id,
                    timeSlot => timeSlot.TimingSlotsId,
                    (timingSlot, timeSlot) => new
                    {
                        timingSlot.Date,
                        timeSlot.StartTime,
                        timeSlot.EndTime
                    })
                .ToListAsync();

            return timingSlotsInString
                .Select(ts => $"{ts.Date:yyyy-MM-dd} {(int)ts.StartTime.TotalHours:D2}:{ts.StartTime.Minutes:D2} - {(int)ts.EndTime.TotalHours:D2}:{ts.EndTime.Minutes:D2}");
        }
        public async Task<IEnumerable> ShowDoctorsAdded()
        {
            IEnumerable<Doctor> objDoctorsList = await _db.Doctors.ToListAsync();
            return objDoctorsList;
        }

        public  IEnumerable<Appointment> fetchAppointments(ApplicationUser user)
        {
            var appointments = _db.Appointments
                       .Where(a => a.PatientId == user.Id)
                       .ToList();
            return appointments;
        }
        
    }
}
