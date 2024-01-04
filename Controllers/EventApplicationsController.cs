using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DiplomService.Database;
using DiplomService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using DiplomService.Models.Users;
using DiplomService.ViewModels.EventApplication;
using DiplomService.Services;
using DiplomService.ViewModels;

namespace DiplomService.Controllers
{
    [Authorize(Roles = "WebUser")]
    public class EventApplicationsController : Controller
    {
        private readonly ApplicationContext _context;
        private readonly UserManager<User> _userManager;

        public EventApplicationsController(ApplicationContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Create(int Id)
        {
            if (await _userManager.GetUserAsync(User) is not WebUser user)
                return BadRequest();

            var @event = await _context.Events.FirstOrDefaultAsync(x => x.Id == Id);

            if (@event == null)
                return NotFound();

            var model = new EventApplicationViewModel()
            {
                EventId = Id,
                ApplicationDatas = new()
                {
                    new()
                },
                ApplicationSenderId = user.Id,
            };


            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EventApplicationViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (await _userManager.GetUserAsync(User) is not Models.Users.WebUser user)
                    return BadRequest();

                var @event = await _context.Events.FirstOrDefaultAsync(x => x.Id == model.EventId);
                if (@event == null) 
                    return NotFound();

                Dictionary<string, string> phones = new();

                foreach (var item in model.ApplicationDatas)
                {
                    item.PhoneNumber = PhoneWorker.NormalizePhone(item.PhoneNumber);
                    if (!phones.ContainsKey(item.PhoneNumber))
                    {
                        phones.Add(item.PhoneNumber, item.SecondName + " " + item.Name);
                    }
                    else
                    {
                        ModelState.AddModelError("", "Номера телефонов " + item.SecondName + " " + item.Name + " и " + phones.First(x => x.Key == item.PhoneNumber).Value + " совпадают");
                        return View(model);
                    }
                }
                
                var application = new EventApplication
                {
                    ApplicationData = model.ApplicationDatas,
                    ApplicationSender = user,
                    Email = user.Email,
                    Event = @event,
                    EventId = model.EventId,
                    ApplicationSenderId = model.ApplicationSenderId,
                    TimeOfSend = DateTime.Now,
                };

                await _context.AddAsync(application);

                return RedirectToAction("Index");
            }
            return View(model);
        }

        [Authorize(Roles = "OrganizationUser")]
        public async Task<IActionResult> Edit(int Id)
        {
            var application = await _context.EventApplications.FirstOrDefaultAsync(x => x.Id == Id);

            if (application == null)
                return NotFound();

            return View(application);
        }
        public async Task<IActionResult> Details(int Id)
        {
            var userId = _userManager.GetUserId(User);
            var application = await _context.EventApplications.FirstOrDefaultAsync(x => x.Id == Id && x.ApplicationSenderId == userId);

            if (application == null)
                return NotFound();

            string status = "";
            ViewBag.Status = status;

            if (application.Accepted != null)
                if (application.Accepted.Value)
                    status = "Принята";
                else
                    status = "Отклонена";
            else
                status = "На рассмотрении";

            return View(application);
        }

        [Authorize(Roles = "OrganizationUser")]
        public async Task<IActionResult> List(int eventId)
        {
            if (await _userManager.GetUserAsync(User) is not OrganizationUsers user)
                return BadRequest();

            bool canAccess = false;

            foreach (var item in user.Organization.Events)
            {
                if (canAccess = item.Id == eventId)
                {
                    break;
                }
            }

            if (!canAccess)
                return BadRequest();

            var application = _context.EventApplications.Where(x => x.EventId == eventId);

            if (application == null)
                return NotFound();

            return View(application);
        }
        public async Task<IActionResult> List()
        {
            if (await _userManager.GetUserAsync(User) is not WebUser user)
                return BadRequest();

            var application = _context.EventApplications.Where(x => x.ApplicationSenderId == user.Id);

            if (application == null)
                return NotFound();

            return View(application);
        }

        /// <summary>
        /// Создание нового элемента для пользователя
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> GetParticipantEntry(int Id,int Index)
        {
            var @event = await _context.Events.FirstOrDefaultAsync(x => x.Id == Id);

            if (@event == null)
                return NotFound();

            var model = new ApplicationDataViewModel
            {
                ApplicationDatas = new(),
                Divisions = @event.Divisions,
                DivisionsExist = @event.DivisionsExist,
            };
            ViewBag.Index = Index;
            return PartialView("_ApplicationDataEntry", model);
        }

        public async Task<WebUser?> FindByPhoneNumberAsync(string phoneNumber)
        {
            return await _context.WebUsers.FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber);
        }
    }
}
