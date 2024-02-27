using DoctorAppointmentManagement.Contracts;
using DoctorAppointmentManagement.Data;
using DoctorAppointmentManagement.Models;
using DoctorAppointmentManagement.Services.AddTimingData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;  


namespace DoctorAppointmentManagement.Controllers
{
    [Authorize(Roles = "Doctors")]
    public class DoctorController : Controller
    {
        private readonly ITimingServices _timingServices;
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;  

        public DoctorController(ITimingServices timingServices, ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            _timingServices = timingServices;
            _db = db;
            _userManager = userManager;  
        }

		[HttpGet]
		public IActionResult AddTiming()
		{
			return View();
		}

		[HttpPost]
		[AutoValidateAntiforgeryToken]
		public async Task<IActionResult> AddTimingpost(AvailableTiming availableTiming)
		{
			try
			{
                if (availableTiming== null)
                {
                    ModelState.AddModelError("Name", "The Fields are Not Correct Check Again");
                    return View();
                }
                ModelState.Clear();
                if (ModelState.IsValid)
				{
					var user = await _userManager.GetUserAsync(User);
					if (user == null)
					{
						return View("Error", new ErrorViewModel { ErrorMessage = "Failed to add timing. Please try again later. can't access user" });
					}

					var timingResult = await _timingServices.AddAvailableTimings(availableTiming, user);

					if (timingResult is OkResult)
					{
						return RedirectToAction("IndexDoctor");
					}

					return View("Error", new ErrorViewModel { ErrorMessage = "Failed to add timing. Please try again later." });
				}

				
				var errorMessages = ModelState.Values
					.SelectMany(v => v.Errors)
					.Select(e => e.ErrorMessage)
					.ToList();

				return View("Error", new ErrorViewModel { ErrorMessage = $"Invalid model state. Please check your input. Errors: {string.Join(", ", errorMessages)}" });
			}
			catch (Exception ex)
			{
				Console.WriteLine($"An error occurred: {ex.Message}");
				return View("Error", new ErrorViewModel { ErrorMessage = "An unexpected error occurred. Please contact support." });
			}
		}

	}
}

