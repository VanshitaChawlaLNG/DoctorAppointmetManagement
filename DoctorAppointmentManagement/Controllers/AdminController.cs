
using DoctorAppointmentManagement.Contracts;
using DoctorAppointmentManagement.Contracts.Constants;
using DoctorAppointmentManagement.Data;
using DoctorAppointmentManagement.Services.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace DoctorAppointmentManagement.Controllers
{
	[Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IAdminService _adminService;
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger<AdminController> _logger;

        public AdminController(ApplicationDbContext db, UserManager<ApplicationUser> userManager ,IWebHostEnvironment webHostEnvironment,IAdminService adminService, ILogger<AdminController> logger)
        {
            _logger = logger;
           _adminService=adminService;
            _db = db;
            _userManager = userManager;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> IndexDoctor()
        {
            try
            {

                var showDoctorsAdded = await _adminService.ShowDoctorsAdded();
                //TempData["SuccessMessage"] = "Unwanted exception occur";
                return View(showDoctorsAdded);
                } 
            
            catch (Exception ex)
            {
                _logger.LogInformation($"An Exception occurred at index of Doctor: {ex.Message}");
                TempData["ErrorMessage"] = "Unwanted exception occur";
                return View();
            }
        }


        public IActionResult CreateDoctor()
        {
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]

        public async Task<IActionResult> CreateDoctors(Doctor doctor)
        {
            try
            {
                if (doctor == null)
                {
                    TempData["ErrorMessage"] = "No Values Entered";
                    return View();
                }
                ModelState.Clear();

                if (ModelState.IsValid)
                {
                    if (doctor.ProfilePictureFile != null && doctor.ProfilePictureFile.Length > 0)
                    {

                        var fileName = Guid.NewGuid().ToString() + "_" + doctor.ProfilePictureFile.FileName;


                        var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "Uploads", fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await doctor.ProfilePictureFile.CopyToAsync(stream);
                        }

                        doctor.ProfilePicture = "/Uploads/" + fileName;
                    }

                    bool CreateDoctorCall = await _adminService.CreateDoctorServices(doctor);
                    if (!CreateDoctorCall)
                    {
                        TempData["ErrorMessage"] = "Unable To Post Details";
                        return View();
                    }
                    else
                    {
                        TempData["SucessMessage"] = "Values Inserted";
                       
                        return View();
                    }
                }
                TempData["ErrorMessage"] = "Model State Invalid";
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"An Exception occurred at Creation of Doctor: {ex.Message}");
                Console.WriteLine(ex.ToString());
                TempData["ErrorMessage"] = "Exception";
                return View();
            }
        }


        public async Task<IActionResult> DeleteDoctor(int Id)
        {
            var doctor = await _adminService.GetDoctorByIdAsync(Id);

            if (doctor == null)
            {
                TempData["ErrorMessage"] = "Unable to fetch data";
                return NotFound();
            }

            return View(doctor);
        }

        // POST: Doctor/Delete/5
        [HttpPost, ActionName("DeleteDoctor")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int Id)
        {
            try
            {
                var success = await _adminService.DeleteDoctorAndRelatedEntitiesAsync(Id);

                if (success)
                {
                    TempData["SuccessMessage"] = "Doctor Deleted";
                    return RedirectToAction("IndexDoctor");
                }

                TempData["ErrorMessage"] = "Unable to delete. Appointments exist.";
                return RedirectToAction("IndexDoctor");
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"An Exception occurred at deletion of Doctor: {ex.Message}");
                // Log the exception or handle it as needed
                TempData["ErrorMessage"] = "An error occurred while deleting the doctor.";
                return RedirectToAction("IndexDoctor");
            }
        }


        public async Task<IActionResult> EditDoctor(int Id)
        {
            try
            {
                if (Id <= 0)
                {
                    TempData["ErrorMessage"] = "Invalid Doctor Id";
                    return RedirectToAction("IndexDoctor"); 
                }

                var userDetails = await _adminService.FetchDoctorById(Id);

                if (userDetails == null)
                {
                    return NotFound(); 
                }

                return View(userDetails);
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"An Exception occurred at Editing  Doctor: {ex.Message}");
                TempData["ErrorMessage"] = "An error occurred while fetching doctor details.";
               
                return View("IndexDoctor"); 
            }
        }


        [HttpPost]
        [AutoValidateAntiforgeryToken]

        public async Task<IActionResult> EditDoctor(Doctor doctorObj)
        {
            try
            {
                ModelState.Clear();
                // Check if ModelState is valid
                if (!ModelState.IsValid)
                {
                    TempData["ErrorMessage"] = "Invalid or Missing Entries By The User";
                    return RedirectToAction("IndexDoctor"
                        );
                }

                var result = await _adminService.UpdateDoctorServices(doctorObj);

                if (result)
                {
                    TempData["SuccessMessage"] = "Doctor Updated";
                    return RedirectToAction("IndexDoctor"
                        );
                }

                TempData["ErrorMessage"] = "Failed to Update Doctor. Please try again.";
                return View(doctorObj);
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"An Exception occurred at Updation of Doctor: {ex.Message}");
                Console.WriteLine($"An error occurred: {ex.Message}");
                TempData["ErrorMessage"] = "An error occurred while processing your request.";
                return View(doctorObj);
            }
        }



        public IActionResult IndexUser()
        {
            try
            {
                IEnumerable<ApplicationUser> objUserList = _db.Users;
                return View(objUserList);
            }
            catch (Exception ex)
            {
                // Log the exception (consider using a logging framework like Serilog or NLog)
                _logger.LogInformation(ex, "An error occurred while fetching user data.");

                TempData["ErrorMessage"] = "An error occurred while fetching user data.";
                return RedirectToAction("Index"); // Redirect to a general error page or another suitable action
            }
        }



        public async Task<IActionResult> ShowAppointments()
        {
            try
            {
                
                
                    var showAppointmentssAdded = await _adminService.ShowAppointmentsAdded();

                    if (showAppointmentssAdded != null)
                    {
                        TempData["SuccessMessage"] = "Details";
                        return View(showAppointmentssAdded);
                    }
                TempData["ErrorMessage"] = "Can't Insert Your values";
                return View();
                
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine(ex.Message);
                TempData["ErrorMessage"] = "Database exception";
                return View();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                TempData["ErrorMessage"] = "exception ,Could Not Insert Your Data";
                return View();
            }
        }
       


       /* public async Task<IActionResult> DeleteUser(string id)
        {
            if ( id == null)
            {
                return NotFound();
            }
            var userDetails =_adminService.FetchUserById(id);
            if (userDetails == null)
            {
                return NotFound();
            }
            return  View(userDetails);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult DeleteUser(ApplicationUser obj)
        {
            var userDetails = _db.Users.Find(obj.Id);
            if (userDetails == null) { return NotFound(); }
            _db.Users.Remove(userDetails);
            _db.SaveChanges();
            TempData["success"] = "User Deleted";
            return RedirectToAction("IndexUser");

        }/*

        public async Task<IActionResult> EditUser(string id)
        {
            if (id == null
                
                )
            {
                return NotFound();
            }
            var UserById = _db.Users.Find(id);
            if (UserById == null)
            {
                return NotFound();
            }
            return View(UserById);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]

        public async Task<IActionResult> EditUser(ApplicationUser  UserObj)
        {

            var existingUser = await _db.Doctors.FindAsync(UserObj.Id);

            if (existingUser == null)
            {

                return NotFound();
            }



            _db.Users.Update(UserObj);

            await _db.SaveChangesAsync();

            return RedirectToAction("IndexDoctor");
        }
*/

    }
}
