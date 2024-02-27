using Microsoft.AspNetCore.Mvc;

namespace DoctorAppointmentManagement.Services
{
    public interface IFileService
    {
        Tuple<int, string> SaveImage(IFormFile imageFile);
        public bool DeleteImage(string imageFileName);
    }
}
