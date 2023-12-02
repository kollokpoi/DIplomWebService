using DiplomService.Database;
using DiplomService.Models;
using DiplomService.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DiplomService.Controllers
{
    public class EventsController : Controller
    {
        private readonly ApplicationContext _context;

        public EventsController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: Events
        public async Task<IActionResult> Index()
        {
            return _context.Events != null ?
                        View(await _context.Events.Where(x => x.ReadyToShow).ToListAsync()) :
                        Problem("Entity set 'ApplicationContext.Events'  is null.");
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Events == null)
            {
                return NotFound();
            }
            var @event = await _context.Events.FirstOrDefaultAsync(m => m.Id == id);
            if (@event == null)
            {
                return NotFound();
            }

            EventViewModel eventViewModel = new()
            {
                Id = @event.Id,
                Name = @event.Name,
                Description = @event.Description,
                DateOfStart = @event.DateOfStart,
                DateOfEnd = @event.DateOfEnd,
                PriviewImage = @event.PriviewImage,
                PublicEvent = @event.PublicEvent,
                Divisions = @event.Divisions,
                Measures = @event.Measures,
                Organizations = @event.Organizations,
            };
            return View(eventViewModel);
        }

        [Authorize(Roles = "OrganizationUser")]
        public IActionResult Create(int? id)
        {
            if (id is not null)
            {
                return RedirectToAction(nameof(Edit), new { id = id });
            }

            return View();
        }

        [Authorize(Roles = "OrganizationUser")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Events == null)
            {
                return NotFound();
            }

            var @event = await _context.Events.FindAsync(id);

            if (@event == null)
            {
                return NotFound();
            }
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var organizationUser = await _context.OrganizationUsers.FirstOrDefaultAsync(x => x.Id == currentUserId);

            if (organizationUser != null)
            {
                var organization = organizationUser.Organization;

                if (organization != null)
                {
                    if (!@event.Organizations.Contains(organization))
                    {
                        return Forbid();
                    }
                }
            }
            EventViewModel eventViewModel = new()
            {
                Id = @event.Id,
                Name = @event.Name,
                Description = @event.Description,
                DateOfStart = @event.DateOfStart,
                DateOfEnd = @event.DateOfEnd,
                PriviewImage = @event.PriviewImage,
                PublicEvent = @event.PublicEvent,
                Divisions = @event.Divisions,
                Measures = @event.Measures,
                DivisionsExist = @event.DivisionsExist,
            };
            return View(eventViewModel);
        }

        [HttpPost]
        [Authorize(Roles = "OrganizationUser")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EventViewModel EventViewModel)
        {
            if (ModelState.IsValid)
            {
                var newEvent = new Event()
                {
                    Name = EventViewModel.Name,
                    Description = EventViewModel.Description,
                    DateOfStart = EventViewModel.DateOfStart,
                    DateOfEnd = EventViewModel.DateOfEnd,
                    PriviewImage = EventViewModel.PriviewImage,
                    PublicEvent = EventViewModel.PublicEvent,
                    DivisionsExist = EventViewModel.DivisionsExist,
                };
                _context.Events.Add(newEvent);

                var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var organizationUser = await _context.OrganizationUsers.FirstOrDefaultAsync(x => x.Id == currentUserId);

                if (organizationUser != null)
                {
                    var organization = organizationUser.Organization;

                    if (organization != null)
                    {
                        newEvent.Organizations.Add(organization);
                    }
                }

                await _context.SaveChangesAsync();
                return RedirectToAction("Index", controllerName: "Measures", new { Id = newEvent.Id });
            }
            return View(EventViewModel);
        }

        [HttpPost]
        [Authorize(Roles = "OrganizationUser")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EventViewModel EventViewModel)
        {
            if (id != EventViewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var @event = await _context.Events.FirstOrDefaultAsync(x => x.Id == id);
                if (@event == null)
                    return NotFound();

                @event.Name = EventViewModel.Name;
                @event.Description = EventViewModel.Description;
                @event.DateOfStart = EventViewModel.DateOfStart;
                @event.DateOfEnd = EventViewModel.DateOfEnd;
                if (EventViewModel.PriviewImage != null)
                {
                    @event.PriviewImage = EventViewModel.PriviewImage;
                }

                @event.PublicEvent = EventViewModel.PublicEvent;
                @event.DivisionsExist = EventViewModel.DivisionsExist;

                var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var organizationUser = await _context.OrganizationUsers.FirstOrDefaultAsync(x => x.Id == currentUserId);

                if (organizationUser != null)
                {
                    var organization = organizationUser.Organization;

                    if (organization != null)
                    {
                        if (!@event.Organizations.Contains(organization))
                        {
                            @event.Organizations.Add(organization);
                        }
                    }
                }

                await _context.SaveChangesAsync();

                return RedirectToAction("Index", controllerName: "Measures", new { Id = @event.Id });
            }
            return View(EventViewModel);
        }

        // GET: Events/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Events == null)
            {
                return NotFound();
            }

            var @event = await _context.Events
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@event == null)
            {
                return NotFound();
            }

            return View(@event);
        }

        [Authorize]
        public async Task<IActionResult> Publish(int id)
        {
            var @event = await _context.Events.FirstOrDefaultAsync(x => x.Id == id);
            if (@event != null)
            {
                var News = new News()
                {
                    Id = @event.Id,
                    Event = @event,
                    EventId = @event.Id,
                    Title = $"Событие {@event.Name.ToLower()} создано.",
                    Description = "Событие создано. Теперь есть возможность принять участие.",
                    Author = "Система",
                    DateTime = DateTime.Now,
                    Sections = new()
                };

                

                @event.ReadyToShow = true;
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return BadRequest();
            }
        }

        private bool EventExists(int id)
        {
            return (_context.Events?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
