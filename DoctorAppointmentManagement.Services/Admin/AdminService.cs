using DoctorAppointmentManagement.Contracts;

using DoctorAppointmentManagement.Contracts.Constants;
using DoctorAppointmentManagement.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorAppointmentManagement.Services.Admin
{
    public class AdminService : IAdminService
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminService(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }
        public async Task<IEnumerable> ShowDoctorsAdded()
        {

                        IEnumerable<Doctor> objDoctorList = await _db.Doctors.ToListAsync();                       
                        return objDoctorList;
        }

        public  Task<Doctor> FetchDoctorById(int DoctorId)
        {

            var doctorDetails = _db.Doctors.FirstOrDefaultAsync(i=>i.Id==DoctorId);
            return doctorDetails;
        }
        public async Task<bool> CreateDoctorServices(Doctor doctor)
        {

            //Handle Duplicacy
            if (_db.Doctors.Any(d => d.Id == doctor.Id))
            {
                return false; 
            }

            
            if (_db.Doctors.Any(d => d.Email == doctor.Email))
            {
                return false; 
            }
           CapitalizeFullName(doctor.Name);
            CapitalizeFullName(doctor.Email);
            CapitalizeFullName(doctor.Description);
            _db.Doctors.Add(doctor);
            int saveResult=await _db.SaveChangesAsync();
            if (saveResult <= 0)
            {
                return false;
            }
            //fetching Id to add in Application user
            await _db.Doctors.SingleAsync(d => d.Id == doctor.Id);
            var user = new ApplicationUser
            {
                UserName = doctor.Email,
                Email = doctor.Email,
                Name = doctor.Name,
                DoctorId = doctor.Id,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,

            };
            var result = await _userManager.CreateAsync(user, doctor.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, Roles.Doctors.ToString());
                return true;
            }
            return false;
        }

        public async Task<bool> UpdateDoctorServices(Doctor DoctorObj)
        {
            var existingDoctor = await _db.Doctors.FindAsync(DoctorObj.Id);

            if (existingDoctor == null)
            {

                return false;
            }


            DoctorObj.ProfilePicture = existingDoctor.ProfilePicture;
            DoctorObj.Password = existingDoctor.Password;


            _db.Entry(existingDoctor).CurrentValues.SetValues(DoctorObj);

            var result=await _db.SaveChangesAsync();
            return result > 0;

        }

        public async Task<bool> HasAppointmentsAsync(int doctorId)
        {
            return await _db.Appointments.AnyAsync(a => a.DoctorId == doctorId);
        }

        public async Task<Doctor> GetDoctorByIdAsync(int doctorId)
        {
            return await _db.Doctors.FindAsync(doctorId);
        }

        public async Task<bool> DeleteDoctorAndRelatedEntitiesAsync(int doctorId)
        {
            using (var transaction = await _db.Database.BeginTransactionAsync())
            {
                try
                {
                    // Check if there are appointments associated with the doctor
                    if (await HasAppointmentsAsync(doctorId))
                    {
                        return false; // Unable to delete, appointments exist
                    }

                    // Delete doctor's timing slots and slots
                    var timingSlots = await _db.TimingSlots
                        .Where(ts => ts.DoctorId == doctorId)
                        .ToListAsync();

                    foreach (var timingSlot in timingSlots)
                    {
                        var slots = await _db.Slots
                            .Where(s => s.TimingSlotsId == timingSlot.Id)
                            .ToListAsync();

                        _db.Slots.RemoveRange(slots);
                    }

                    _db.TimingSlots.RemoveRange(timingSlots);

                    // Delete doctor
                    var doctor = await _db.Doctors.FindAsync(doctorId);
                    _db.Doctors.Remove(doctor);

                    await _db.SaveChangesAsync();
                    transaction.Commit();

                    return true;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    return false;
                }
            
        }
    }

    public async Task<ApplicationUser> FetchUserById(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            return user;
        }

        public async Task<IEnumerable> ShowAppointmentsAdded()
        {
            IEnumerable<Appointment> appointmentList = await _db.Appointments.ToListAsync();
            return appointmentList;
        }

        private string CapitalizeFullName(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }

            // Split the name into parts
            string[] nameParts = input.Split(' ');

            // Capitalize each part and join them back together
            for (int i = 0; i < nameParts.Length; i++)
            {
                nameParts[i] = char.ToUpper(nameParts[i][0]) + nameParts[i].Substring(1).ToLower();
            }

            return string.Join(' ', nameParts);
        }
    }
}

