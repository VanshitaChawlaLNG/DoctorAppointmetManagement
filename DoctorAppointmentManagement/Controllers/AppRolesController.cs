using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace DoctorAppointmentManagement.Controllers
{
    public class AppRoles : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public AppRoles(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }
        //List All The Roles 
        public IActionResult Index()
        {
            var roles = _roleManager.Roles;
            return View();
        }

        [HttpGet]
        public IActionResult Create() {
            return View();
             
        }

        public async Task<IActionResult> Create(IdentityRole model)
        {
            if(!_roleManager.RoleExistsAsync(model.Name).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole ( model.Name )).GetAwaiter().GetResult();
            }
            return RedirectToAction("Index");
        }

    }
}
