using JustSellIt.Application.ViewModels.Base;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JustSellIt.Web.Controllers
{
    public class BaseController : Controller
    {
        public void SetMessage(string message,MessageType type)
        {
            TempData["SM"] = message;
            TempData["SMT"] = type;
        }
    }
}
