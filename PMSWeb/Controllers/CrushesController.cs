using Microsoft.AspNetCore.Mvc;
using static PMS.Common.ErrorMessages;

namespace PMSWeb.Controllers
{
    public class CrushesController : Controller
    {
        public IActionResult NotFound(string caller)
        {

            /// TO DO Home controller to be replaced with called id controller

            ViewBag.Controller = "Home";
            ViewBag.Message = NotFoundMessage;
            return View();
        }

        public IActionResult ModelNotValid(string caller)
        {
            ViewBag.Controller = "Home";
            ViewBag.Message = ModelNotValidMessage;
            return View();
        }

        public IActionResult WrongData(string caller)
        {
            ViewBag.Controller = "Home";
            ViewBag.Message = WrongDataMessage;
            return View();
        }

        public IActionResult NotDeleted(string caller)
        {
            ViewBag.Controller = "Home";
            ViewBag.Message = NotDeletedMessage;
            return View();
        }

        public IActionResult NotEdited(string caller)
        {
            ViewBag.Controller = "Home";
            ViewBag.Message = NotEditedMessage;
            return View();
        }

        public IActionResult NotCreated(string caller)
        {
            ViewBag.Controller = "Home";
            ViewBag.Message = NotCreatedMessage;
            return View();
        }

        public IActionResult NotUpdated(string caller)
        {
            ViewBag.Controller = "Home";
            ViewBag.Message = NotUpdatedMessage;
            return View();
        }

        public IActionResult EmptyList(string caller)
        {
            ViewBag.Controller = "Home";
            ViewBag.Message = EmptyListMessage;
            return View();
        }

    }
}
