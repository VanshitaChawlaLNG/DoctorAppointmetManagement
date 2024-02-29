using DoctorAppointmentManagement.Contracts;
using DoctorAppointmentManagement.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Numerics;


namespace DoctorAppointmentManagement.Services.AddTimingData
{
    public class Timing : ITiming
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public Timing(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public async Task<IActionResult> AddAvailableTimings(AvailableTiming availableTiming, ApplicationUser user)
        {
            try
            {
                if (user == null)
                {
                    return new NotFoundResult();
                }

                availableTiming.DoctorId = user.DoctorId;

                if (_db.AvailableTimings.Any(at => at.DoctorId == user.DoctorId
                                             && at.Date == availableTiming.Date
                                             && at.StartTimeHours == availableTiming.StartTimeHours
                                             && at.StartTimeMins == availableTiming.StartTimeMins
                                             && at.EndTimeHours == availableTiming.EndTimeHours
                                             && at.EndTimeMins == availableTiming.EndTimeMins))
                {

                    return new BadRequestObjectResult("The specified time period already exists for the doctor on that date.");
                }

                _db.AvailableTimings.Add(availableTiming);
                await _db.SaveChangesAsync();

                TimingSlots timingSlots = new TimingSlots
                {
                    DoctorId = user.DoctorId,
                    Date = availableTiming.Date
                    
                };
                _db.TimingSlots.Add(timingSlots);
                await _db.SaveChangesAsync();
                int insertedTimingSlotsId = timingSlots.Id;
                TimeSpan startTime = new TimeSpan(availableTiming.StartTimeHours, availableTiming.StartTimeMins, 0);
                TimeSpan endTime = new(availableTiming.EndTimeHours, availableTiming.EndTimeMins, 0);
                TimeSpan slotDuration = TimeSpan.FromMinutes(30);


                for (var i = startTime; i < endTime; i = i.Add(slotDuration))
                {
                    Slots timeSlot = new Slots
                    {
                        StartTime = i,
                        EndTime = i.Add(slotDuration),
                        TimingSlotsId = insertedTimingSlotsId 

                };

                    _db.Slots.Add(timeSlot);
                    await _db.SaveChangesAsync();
                }


               

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

