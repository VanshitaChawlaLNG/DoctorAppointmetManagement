using DoctorAppointmentManagement.Contracts;
using DoctorAppointmentManagement.Data;
using System.Collections;


namespace DoctorAppointmentManagement.Services.Admin
{
    public interface IAdminService
    {
        public  Task<IEnumerable> ShowDoctorsAdded();

        public Task<Doctor> FetchDoctorById(int DoctorId);

        public Task<bool> CreateDoctorServices(Doctor doctor);

        public Task<bool> UpdateDoctorServices(Doctor doctor);

        public Task<bool> DeleteDoctorServices(Doctor doctor);
        public Task<ApplicationUser> FetchUserById(string id);

        
        public Task<IEnumerable> ShowAppointmentsAdded();
    }
}
