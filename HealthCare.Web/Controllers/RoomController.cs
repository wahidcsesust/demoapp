using HealthCare.Data.Models;
using HealthCare.Data.Services;
using HealthCare.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace HealthCare.Web.Controllers
{
    public class RoomController : Controller
    {
        private readonly IGenericRepository<Room> _roomRepository;
        private readonly UserManager<User> _userManager;
        public RoomController(IGenericRepository<Room> roomRepository, UserManager<User> userManager)
        {
            _roomRepository = roomRepository;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Manage()
        {
            var rooms = await _roomRepository.GetAllActive();

            return View(rooms);
        }

        [HttpGet]
        public async Task<IActionResult> CreateEdit(long id = 0)
        {
            var room = new Room();
            if (!String.IsNullOrEmpty(id.ToString()) && id != 0)
            {
                room = await _roomRepository.Get(id);
            }
            return View("EditorTemplates/Room", room);
        }

        [HttpPost]
        public async Task<IActionResult> Save(Room model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                try
                {
                    var room = await _roomRepository.Get(model.Id);
                    if (room != null)
                    {
                        room.RoomNo = model.RoomNo;
                        room.Location = model.Location;
                        room.IsActive = true;
                        room.ModifiedDate = DateTime.Now;
                        room.ModifiedBy = user.Id;
                        await _roomRepository.Update(room);
                        return RedirectToAction("Manage");
                    }
                    else
                    {
                        room = model;
                        room.IsActive = true;
                        room.CreatedDate = DateTime.Now;
                        room.CreatedBy = user.Id;
                        room.ModifiedDate = DateTime.Now;
                        room.ModifiedBy = user.Id;

                        await _roomRepository.Insert(room);
                        return RedirectToAction("Manage");
                    }
                }
                catch (DbUpdateException ex)
                {
                    //Log the error (uncomment ex variable name and write a log.
                    ModelState.AddModelError("", "Unable to save changes. " +
                        "Try again, and if the problem persists " +
                        "see your system administrator." + ex.Message.ToString());
                }
            }
            return View("EditorTemplates/Room", model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(long id)
        {
            string name = string.Empty;
            if (!String.IsNullOrEmpty(id.ToString()) && id != 0)
            {
                var department = await _roomRepository.Get(id);
                if (department != null)
                {
                    name = department.RoomNo;
                }
            }
            return PartialView("DisplayTemplates/_DeleteRoom", name);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(ObjectDeleteViewModel model)
        {
            if (!String.IsNullOrEmpty(model.Id.ToString()) && model.Id != 0)
            {
                var room = await _roomRepository.Get(model.Id);
                if (room != null)
                {
                    //var result = _roomRepository.Delete(room);
                    room.IsDeleted = true; ;
                    await _roomRepository.Update(room);
                    return RedirectToAction("Manage");
                }
            }
            return View();
        }
    }
}