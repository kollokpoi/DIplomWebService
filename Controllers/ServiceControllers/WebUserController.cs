using DiplomService.Database;
using DiplomService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DiplomService.Controllers.ServiceControllers
{
    [Authorize(Roles = "WebUser")]
    public class WebUserController : CabinetController
    {
        private readonly ApplicationContext _context;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public WebUserController(ApplicationContext context, UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<IdentityRole> roleManager) : base(context, userManager, signInManager, roleManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public ActionResult Applications() => RedirectToAction("Index", "EventApplications");
        public ActionResult About() => View();
    }
}
