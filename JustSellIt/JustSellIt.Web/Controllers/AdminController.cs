using JustSellIt.Application;
using JustSellIt.Application.Interfaces;
using JustSellIt.Application.ViewModels.Base;
using Microsoft.AspNetCore.Mvc;

namespace JustSellIt.Web.Controllers
{
    public class AdminController : BaseController
    {
        private readonly IAdminService _adminService;
        private readonly IStatusService _statusService;

        public AdminController(IAdminService adminService, IStatusService statusService)
        {
            _adminService = adminService;
            _statusService = statusService;
        }

        [HttpGet]
        public IActionResult Index(int? actualPage)
        {
            int pageSize = SystemConfiguration.DefaultPageSize;
            var products = _adminService.GetProductsToVerify(actualPage,pageSize);
            return View(products);
        }

        [HttpGet]
        public IActionResult RejectProduct(int id,string rejectionReason)
        {
            _adminService.RejectProduct(id, rejectionReason);
            SetMessage("Ogłoszenie zostało odrzucone", MessageType.Error);
            return Redirect("Index");
        }

        [HttpGet]
        public IActionResult AcceptProduct(int id)
        {
            _adminService.AcceptProduct(id);
            SetMessage("Ogłoszenie zostało zaakceptowane", MessageType.Success);
            return Redirect("Index");
        }
    }
}
