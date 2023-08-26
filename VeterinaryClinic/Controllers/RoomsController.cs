using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Vereyon.Web;
using VeterinaryClinic.Data;
using VeterinaryClinic.Data.Entities;
using VeterinaryClinic.Helpers;
using VeterinaryClinic.Models;
using VeterinaryClinic.Repositories;
using static System.Net.Mime.MediaTypeNames;

namespace VeterinaryClinic.Controllers
{
    public class RoomsController : Controller
    {
        private readonly DataContext _context;
        private readonly IRoomRepository _roomRepository;
        private readonly IConverterHelper _converterHelper;
        private readonly IFlashMessage _flashMessage;

        public RoomsController(DataContext context,
            IRoomRepository roomRepository,
            IConverterHelper converterHelper,
            IFlashMessage flashMessage)
        {
            _context = context;
            _roomRepository = roomRepository;
            _converterHelper = converterHelper;
           _flashMessage = flashMessage;
        }

        // GET: Rooms
        public IActionResult Index()
        {
            return View(_roomRepository.GetAllWithVets());
        }

        // GET: Rooms/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("RoomNotFound");
            }

            var room = await _roomRepository.GetByIdAsync(id.Value);
            if (room == null)
            {
                return new NotFoundViewResult("RoomNotFound");
            }

            return View(room);
        }

        // GET: Rooms/Create
        public IActionResult Create()
        {
            var model = new RoomViewModel
            {
               
                Vets = _roomRepository.GetComboVets(),
                Types = _roomRepository.GetComboTypes(),
                          
            };
            return View(model);
        }

        // POST: Rooms/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RoomViewModel model)
        {
           
            if (ModelState.IsValid)
            {
                var rooms = await _roomRepository.GetRoomAsync(this.User.Identity.Name);
                
                bool room = rooms.Any(r =>
                r.RoomNumber == model.RoomNumber);

                if (room)
                {
                    _flashMessage.Warning("The room is unvailable");
                    model = new RoomViewModel
                    {
                        Vets = _roomRepository.GetComboVets(),
                        Types = _roomRepository.GetComboTypes(),

                    };
                    return View(model);

                }

                bool vet = rooms.Any(r =>
                r.Vet.Id == model.VetId);

                if (vet)
                {
                    _flashMessage.Warning("The vet is in another room");
                    model = new RoomViewModel
                    {
                        Vets = _roomRepository.GetComboVets(),
                        Types = _roomRepository.GetComboTypes(),

                    };
                    return View(model);

                }
                else
                {
                    await _roomRepository.AddVetToRoomAsync(model, this.User.Identity.Name);
                    return RedirectToAction("Index");


                }


            }
           
           
            return View(model);
        }

        // GET: Rooms/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {   
                      
            if (id == null)
            {
                return new NotFoundViewResult("RoomNotFound");
            }

            var room = await _roomRepository.GetByIdAsync(id.Value);
            if (room == null)
            {
                return new NotFoundViewResult("RoomNotFound");
            }
            var model = new RoomViewModel
            {
                RoomNumber = room.RoomNumber,
                Vets = _roomRepository.GetComboVets(),
                Types = _roomRepository.GetComboTypes(),
                Status = room.Status,

            };
            return View(model);
        }

        // POST: Rooms/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(RoomViewModel model)
        {
          
            if (ModelState.IsValid)
            {
                try
                {
                    var rooms = await _roomRepository.GetRoomAsync(this.User.Identity.Name);
               
                    bool vet = rooms.Any(r =>
                    r.Vet.Id == model.VetId);

                    if (vet)
                    {
                        _flashMessage.Warning("The vet is in another room");
                        model = new RoomViewModel
                        {
                            Vets = _roomRepository.GetComboVets(),
                            Types = _roomRepository.GetComboTypes(),

                        };
                        return View(model);

                    }
                    else
                    {
                        await _roomRepository.EditRoomAsync(model, this.User.Identity.Name, false);
                        return RedirectToAction("Index");


                    }
                   
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _roomRepository.ExistAsync(model.Id))
                    {
                        return new NotFoundViewResult("RoomNotFound");
                    }
                    else
                    {
                        throw;
                    }
                }
             
            }
            return View(model);
        }

        // GET: Rooms/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("RoomNotFound");
            }

            var room = await _roomRepository.GetByIdAsync(id.Value);
            if (room == null)
            {
                return new NotFoundViewResult("RoomNotFound");
            }

           return View(room);
        }

        // POST: Rooms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var room = await _roomRepository.GetByIdAsync(id);
            try
            {
                await _roomRepository.DeleteAsync(room);
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {

                if (ex.InnerException != null && ex.InnerException.Message.Contains("DELETE"))
                {
                    ViewBag.ErrorTitle = $"{room.Type} probably in been used!!!";
                   
                }
                return View("Error");
            }
           
        }

        public IActionResult RoomNotFound()
        {
            return View();
        }
    }
}
