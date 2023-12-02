using DiplomService.Database;
using DiplomService.Models;
using DiplomService.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DiplomService.Controllers
{
    [Authorize]
    public class CabinetController : Controller
    {
        private readonly ApplicationContext _context;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public CabinetController(ApplicationContext context, UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<ActionResult> Edit()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }
            else
            {
                var model = new UserViewModel
                {
                    Name = user.Name,
                    SecondName = user.SecondName,
                    LastName = user.LastName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                };
                return View(model);
            }
        }
        public ActionResult Index()
        {
            if (User.IsInRole("Administrator"))
            {
                return RedirectToAction("Dashboard", "Admin");
            }
            else if (User.IsInRole("WebUser"))
            {
                return RedirectToAction("Applications", "WebUser");
            }
            else if (User.IsInRole("OrganizationUser"))
            {
                return RedirectToAction("Organization", "OrganizationUser");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public async Task<ActionResult> EditLogin()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }
            else
            {
                var model = new UserViewModel
                {
                    Name = user.Name,
                    SecondName = user.SecondName,
                    LastName = user.LastName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                };
                return View(model);
            }
        }
        public async Task<ActionResult> EditPassword()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }
            else
            {
                var model = new UserViewModel
                {
                    Name = user.Name,
                    SecondName = user.SecondName,
                    LastName = user.LastName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                };
                return View(model);
            }
        }

        [HttpPost]
        public async Task<ActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<ActionResult> Edit(UserViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.FindByIdAsync(model.Id);

            if (user != null)
            {
                user.Name = model.Name;
                user.SecondName = model.SecondName;
                user.LastName = model.LastName;

                if (model.Email != user.Email)
                {

                }
                if (model.PhoneNumber != user.PhoneNumber)
                {

                }

                _context.Update(user);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Error = "Произошла ошибка";
                return View(model);
            }
        }
    }
}
