using JustSellIt.Application;
using JustSellIt.Application.Interfaces;
using JustSellIt.Application.ViewModels.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace JustSellIt.Web.Controllers
{
    public class AdminController : BaseController
    {
        private readonly IAdminService _adminService;
        private readonly ILogger<AdminController> _logger;

        public AdminController(IAdminService adminService,
            ILogger<AdminController> logger)
        {
            _adminService = adminService;
            _logger = logger;
        }

        [HttpGet]
        [Authorize]
        public IActionResult Index(int? actualPage)
        {
            try
            {
                if (!HttpContext.User.HasClaim("AdminManage", "AdminManage"))
                    return View("Error");

                int pageSize = SystemConfiguration.DefaultPageSize;
                var products = _adminService.GetProductsToVerify(actualPage, pageSize);
                return View(products);
            }
            catch (Exception e)
            {
                _logger.LogInformation(String.Format("Data: {0}, Błąd: {1}", DateTime.Now, e));
                return View("Error");
            }
        }

        [HttpGet]
        [Authorize]
        public IActionResult RejectProduct(int id, string rejectionReason)
        {
            try
            {
                if (!HttpContext.User.HasClaim("AdminManage", "AdminManage"))
                    return View("Error");

                _adminService.RejectProduct(id, rejectionReason);
                SetMessage("Ogłoszenie zostało odrzucone", MessageType.Error);
                return Redirect("Index");
            }
            catch (Exception e)
            {
                _logger.LogInformation(String.Format("Data: {0}, Błąd: {1}", DateTime.Now, e));
                return View("Error");
            }
        }

        [HttpGet]
        [Authorize]
        public IActionResult AcceptProduct(int id)
        {
            try
            {
                if (!HttpContext.User.HasClaim("AdminManage", "AdminManage"))
                    return View("Error");

                _adminService.AcceptProduct(id);
                SetMessage("Ogłoszenie zostało zaakceptowane", MessageType.Success);
                return Redirect("Index");
            }
            catch (Exception e)
            {
                _logger.LogInformation(String.Format("Data: {0}, Błąd: {1}", DateTime.Now, e));
                return View("Error");
            }
        }
    }
}
