using Microsoft.AspNetCore.Identity;

namespace DoctorAppointmentManagement.Data
{
    public class ApplicationUser:IdentityUser
    {
        public string?  Name { get; set; }
        public String? ProfilePicture { get; set; }

        public int DoctorId {  get; set; }
    }
}
