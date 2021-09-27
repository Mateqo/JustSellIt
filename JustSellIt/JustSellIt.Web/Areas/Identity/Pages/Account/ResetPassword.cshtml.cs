using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JustSellIt.Application.ViewModels.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;

namespace JustSellIt.Web.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ResetPasswordModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;

        public ResetPasswordModel(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "Adres e-mail jest wymagany")]
            [EmailAddress(ErrorMessage = "Format adresu e-mail nie jest poprawny")]
            public string Email { get; set; }

            [Required(ErrorMessage = "Hasło jest wymagane")]
            [StringLength(50, ErrorMessage = "Hasło powinno składać się od {2} do {1} znaków", MinimumLength = 6)]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Potwierdź hasło")]
            [Compare("Password", ErrorMessage = "Hasła nie są identyczne")]
            public string ConfirmPassword { get; set; }

            public string Code { get; set; }
        }

        public IActionResult OnGet(string code = null)
        {
            if (code == null)
            {
                //TO DO 404
                return BadRequest("A code must be supplied for password reset.");
            }
            else
            {
                Input = new InputModel
                {
                    Code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code))
                };
                return Page();
            }
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
                // Don't reveal that the user does not exist
                SetMessage("Nieprawidłowy adres e-mail", MessageType.Error);
                return Page();
            }

            var result = await _userManager.ResetPasswordAsync(user, Input.Code, Input.Password);
            if (result.Succeeded)
            {
                return RedirectToPage("./ResetPasswordConfirmation");
            }

            foreach (var error in result.Errors)
            {
                switch (error.Code)
                {
                    case "PasswordRequiresLower":
                        SetMessage("Hasło musi zawierać co najmniej jedną małą literę ('a' - 'z')", MessageType.Error);
                        break;
                    case "PasswordRequiresUpper":
                        SetMessage("Hasło musi zawierać co najmniej jedną wielką literę ('A' - 'Z')", MessageType.Error);
                        break;
                    case "PasswordRequiresDigit":
                        SetMessage("Hasło musi zawierać co najmniej jedną cyfrę ('0' - '9')", MessageType.Error);
                        break;
                    case "PasswordRequiresNonAlphanumeric":
                        SetMessage("Hasło musi zawierać co najmniej jeden znak specjalny", MessageType.Error);
                        break;
                    case "InvalidToken":
                        SetMessage("Błędny token restartowania hasła", MessageType.Error);
                        break;
                    default:
                        SetMessage(error.Description, MessageType.Error);
                        break;
                }
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return Page();
        }
    }
}
