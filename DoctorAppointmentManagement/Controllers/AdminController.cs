
using DoctorAppointmentManagement.Contracts;
using DoctorAppointmentManagement.Contracts.Constants;
using DoctorAppointmentManagement.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace DoctorAppointmentManagement.Controllers
{
	[Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AdminController(ApplicationDbContext db, UserManager<ApplicationUser> userManager ,IWebHostEnvironment webHostEnvironment)
        {
            _db = db;
            _userManager = userManager;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult IndexDoctor()
        {
            IEnumerable<Doctor> objCategoryList = _db.Doctors.ToList();
            return View(objCategoryList);
        }

        public IActionResult CreateDoctor()
        {
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]

        public async Task<IActionResult> CreateDoctor(Doctor obj)
        {
            if (obj == null)
            {
                ModelState.AddModelError("Name", "The Fields are Not Correct Check Again");
                return View();
            }
            ModelState.Clear();

            if (ModelState.IsValid)
            {
                if (obj.ProfilePictureFile != null && obj.ProfilePictureFile.Length > 0)
                {
                    
                    var fileName = Guid.NewGuid().ToString() + "_" + obj.ProfilePictureFile.FileName;

                   
                    var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "Uploads", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await obj.ProfilePictureFile.CopyToAsync(stream);
                    }

                    obj.ProfilePicture = "/Uploads/" + fileName;
                }
                

                _db.Doctors.Add(obj);
                _db.SaveChanges();
                TempData["Success"] = "Field Inserted";
                await _db.Doctors.SingleAsync(d => d.Id == obj.Id);
                var user = new ApplicationUser
                {
                    UserName = obj.Email,
                    Email = obj.Email,
                    Name = obj.Name,
                    DoctorId = obj.Id,
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,

                };
                var result = await _userManager.CreateAsync(user, obj.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, Roles.Doctors.ToString());
                }
                return RedirectToAction("Index");
            }

            return View(obj);
        }

		public IActionResult IndexUser()
		{
			IEnumerable<ApplicationUser> objUserList = _db.Users;
			return View(objUserList);
		}

		public async Task<IActionResult> DeleteDoctor(int id)
        {
            if(id == 0 || id == null)
            {
                return NotFound();
            } 
            var userDetails=_db.Doctors.Find(id);
            if(userDetails == null)
            {
                return NotFound();
            }
            return View(userDetails);
        }
       
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult DeleteDoctor(Doctor obj)
        {
            var userDetails = _db.Doctors.Find(obj.Id);
            if (userDetails == null) { return NotFound(); }
            _db.Doctors.Remove(userDetails);
            _db.SaveChanges();
            TempData["success"] = "Doctor Deleted";
            return RedirectToAction("IndexUser");

        }

        public async Task<IActionResult> EditDoctor(int id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var DoctorById=_db.Doctors.Find(id);
            if(DoctorById == null)
            {
                return NotFound();
            }
            return View(DoctorById);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]

        public async Task<IActionResult> EditDoctor(Doctor DoctorObj)
        {
           
            var existingDoctor = await _db.Doctors.FindAsync(DoctorObj.Id);

            if (existingDoctor == null)
            {
               
                return NotFound();
            }

           
            DoctorObj.ProfilePicture = existingDoctor.ProfilePicture;
            DoctorObj.Password = existingDoctor.Password;

           
            _db.Entry(existingDoctor).CurrentValues.SetValues(DoctorObj);

            await _db.SaveChangesAsync();

            return RedirectToAction("IndexDoctor");
        }

        public async Task<IActionResult> DeleteUser(string id)
        {
            if ( id == null)
            {
                return NotFound();
            }
            var userDetails = _db.Users.Find(id);
            if (userDetails == null)
            {
                return NotFound();
            }
            return View(userDetails);
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


    }
}
