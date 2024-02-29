
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

        public AdminController(ApplicationDbContext db, UserManager<ApplicationUser> userManager ,IWebHostEnvironment webHostEnvironment,IAdminService adminService)
        {
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
                Console.WriteLine($"An Exception occurred: {ex.Message}");
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

        public async Task<IActionResult> CreateDoctor(Doctor doctor)
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
                Console.WriteLine(ex.ToString());
                TempData["ErrorMessage"] = "Exception";
                return View();
            }
        }

		

		public async Task<IActionResult> DeleteDoctor(int DoctorId)
        {
            try
            {
                if (DoctorId == 0 || DoctorId == null)
                {
                    return NotFound();
                }
                var userDetails = _adminService.FetchDoctorById(DoctorId);
                if (userDetails == null)
                {
                    return NotFound();
                }
                return View(userDetails);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString() );
                TempData["ErrorMessage"] = "Model State Invalid";
                return View();

            }
        }
       
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> DeleteDoctorAsync(Doctor obj)
        {
            try
            {
                if (obj != null)
                {

                    ModelState.Clear();

                    if (ModelState.IsValid)
                    {
                        var deleteResult = await _adminService.DeleteDoctorServices(obj);

                        if (deleteResult)
                        {
                            TempData["SuccessMessage"] = "Doctor Deleted";
                            return RedirectToAction(nameof(IndexDoctor));
                        }
                        else
                        {
                            // Handle the case where deleteResult is false
                            TempData["ErrorMessage"] = "Failed to delete the doctor.";
                            return View();
                        }
                    }

                    TempData["ErrorMessage"] = "Invalid input or doctor not found.";
                    return View();
                }
                TempData["ErrorMessage"] = "Invalid input .";
                return View();
            }
            catch (Exception ex)
            {
               
                TempData["ErrorMessage"] = "An error occurred while processing your request.";
                Console.WriteLine($"Exception in DeleteDoctorAsync: {ex.Message}");
                return View();
            }

           
        }


        public async Task<IActionResult> EditDoctor(int Doctorid)
        {
            if (Doctorid == null || Doctorid == 0)
            {
                TempData["ErrorMessage"] = "Id is Null";
                return View();
            }
            var userDetails = _adminService.FetchDoctorById(Doctorid);
            if (userDetails == null)
            {
                TempData["ErrorMessage"] = "User is Null";
                return View();
            }
            return View(userDetails);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]

        public async Task<IActionResult> EditDoctor(Doctor doctorObj)
        {
            try
            {
                // Check if ModelState is valid
                if (!ModelState.IsValid || doctorObj == null)
                {
                    TempData["ErrorMessage"] = "Invalid or Missing Entries By The User";
                    return View();
                }

                var result = await _adminService.UpdateDoctorServices(doctorObj);

                if (result)
                {
                    TempData["SuccessMessage"] = "Doctor Updated";
                    return View(nameof(IndexDoctor));
                }

                TempData["ErrorMessage"] = "Failed to Update Doctor. Please try again.";
                return RedirectToAction(nameof(IndexDoctor));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                TempData["ErrorMessage"] = "An error occurred while processing your request.";
                return RedirectToAction(nameof(IndexDoctor));
            }
        }



        public IActionResult IndexUser()
        {
            IEnumerable<ApplicationUser> objUserList = _db.Users;
            return View(objUserList);
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
*/
        /*[HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult DeleteUser(ApplicationUser obj)
        {
            var userDetails = _db.Users.Find(obj.Id);
            if (userDetails == null) { return NotFound(); }
            _db.Users.Remove(userDetails);
            _db.SaveChanges();
            TempData["success"] = "User Deleted";
            return RedirectToAction("IndexUser");

        }

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
