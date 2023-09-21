using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Syncfusion.EJ2.Linq;
using VeterinaryClinic.Data;
using VeterinaryClinic.Data.Entities;
using VeterinaryClinic.Helpers;
using VeterinaryClinic.Repositories;

namespace VeterinaryClinic.Controllers
{
    public class NotificationsController : Controller
    {
    
        private readonly INotificationRepository _notificationRepository;

        public NotificationsController(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        // GET: Notifications
      
        public IActionResult Index()
        {
            var model = _notificationRepository.GetNotificationsAsync();
            return View(model);
        }
     
        public async Task<IActionResult> DeleteNotification(int? id)
        {
            if (id == null)
            {

                return new NotFoundViewResult("NotificationNotFound");

            }
            await _notificationRepository.DeleteNotificationAsync(id.Value);
            return RedirectToAction("Index");
        }
        [Route("notificationnotfound")]
        public IActionResult NotificationNotFound()
        {
            return View();
        }


    }
}
