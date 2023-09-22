using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
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
        [Authorize(Roles = "Vet, Assistant, Receptionist")]
        public IActionResult Index()
        {
            var model = _notificationRepository.GetNotificationsAsync();
            return View(model);
        }
        [Authorize(Roles = "Vet")]
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
