using DoctorAppointmentManagement.Contracts;
using DoctorAppointmentManagement.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorAppointmentManagement.Services.User
{
    public interface IUserService
    {
        public  Task<IEnumerable> ShowDoctorsAdded();
        public Task<IEnumerable<string>> FetchTimingsToShow(int doctorId);
        public IEnumerable<Appointment> fetchAppointments(ApplicationUser user);
    }
}
