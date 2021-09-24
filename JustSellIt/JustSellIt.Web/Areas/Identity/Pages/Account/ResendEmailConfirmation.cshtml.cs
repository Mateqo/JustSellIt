using System;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using JustSellIt.Application;
using JustSellIt.Application.ViewModels.Base;
using JustSellIt.Web.Helpers;
using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;

namespace JustSellIt.Web.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ResendEmailConfirmationModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IEmailSender _emailSender;

        public ResendEmailConfirmationModel(UserManager<IdentityUser> userManager, IEmailSender emailSender)
        {
            _userManager = userManager;
            _emailSender = emailSender;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "Adres e-mail jest wymagany")]
            [EmailAddress(ErrorMessage = "Format adresu e-mail nie jest poprawny")]
            public string Email { get; set; }
        }

        public void OnGet()
        {
        }

        public void SetMessage(string message, MessageType type)
        {
            TempData["SM"] = message;
            TempData["SMT"] = type;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.FindByEmailAsync(Input.Email);
            if (user == null)
            {
                SetMessage("Podany adres e-mail nie istnieje", MessageType.Error);
                return Page();
            }

            if (user.EmailConfirmed)
            {
                SetMessage("Adres e-mail został już potwierdzony", MessageType.Warning);
            }
            else
            {
                var userId = await _userManager.GetUserIdAsync(user);
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = Url.Page(
                    "/Account/ConfirmEmail",
                    pageHandler: null,
                    values: new { userId = userId, code = code },
                    protocol: Request.Scheme);

                EmailSender.SendEmail(callbackUrl, Input.Email, Input.Email, SystemConfiguration.EmailResendBody);

                SetMessage("Potwierdzenie zostało wysłane ponownie", MessageType.Success);
            }
            
            return Page();
        }
    }
}
