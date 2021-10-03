using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using JustSellIt.Application.ViewModels.Base;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
namespace JustSellIt.Web.Areas.Identity.Pages.Account.Manage
{
    public class ChangePasswordModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ILogger<ChangePasswordModel> _logger;

        public ChangePasswordModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            ILogger<ChangePasswordModel> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "Obecne hasło jest wymagane")]
            [DataType(DataType.Password)]
            [Display(Name = "Obecne hasło")]
            public string OldPassword { get; set; }

            [Required(ErrorMessage = "Nowe hasło jest wymagane")]
            [StringLength(50, ErrorMessage = "Hasło powinno składać się od {2} do {1} znaków", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Nowe hasło")]
            public string NewPassword { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Potwierdź hasło")]
            [Compare("NewPassword", ErrorMessage = "Hasła nie są identyczne")]
            public string ConfirmPassword { get; set; }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var hasPassword = await _userManager.HasPasswordAsync(user);
            if (!hasPassword)
            {
                return RedirectToPage("./SetPassword");
            }

            return Page();
        }

        public void SetMessage(string message, MessageType type)
        {
            TempData["SM"] = message;
            TempData["SMT"] = type;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Page();
                }

                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
                }

                var changePasswordResult = await _userManager.ChangePasswordAsync(user, Input.OldPassword, Input.NewPassword);
                if (!changePasswordResult.Succeeded)
                {
                    foreach (var error in changePasswordResult.Errors)
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
                            case "PasswordMismatch":
                                SetMessage("Obecne hasło jest nieprawidłowe", MessageType.Error);
                                break;
                            default:
                                SetMessage(error.Description, MessageType.Error);
                                break;
                        }
                    }
                    return Page();
                }

                await _signInManager.RefreshSignInAsync(user);
                _logger.LogInformation("User changed their password successfully.");
                SetMessage("Hasło zostało zmienione pomyślnie", MessageType.Success);

                return RedirectToPage();
            }
            catch (Exception e)
            {
                _logger.LogInformation(String.Format("Data: {0}, Błąd: {1}", DateTime.Now, e));
                return Redirect("Error");
            }
        }
    }
}
