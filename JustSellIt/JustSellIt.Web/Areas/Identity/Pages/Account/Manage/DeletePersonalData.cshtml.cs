using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using JustSellIt.Application.Interfaces;
using JustSellIt.Application.ViewModels.Base;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace JustSellIt.Web.Areas.Identity.Pages.Account.Manage
{
    public class DeletePersonalDataModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ILogger<DeletePersonalDataModel> _logger;
        private readonly IProductService _productService;
        private readonly IImageService _imageService;
        private readonly IOwnerService _ownerService;
        private readonly IOwnerContactService _ownerContactService;

        public DeletePersonalDataModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            ILogger<DeletePersonalDataModel> logger,
            IProductService productService,
            IImageService imageService,
            IOwnerService ownerService,
            IOwnerContactService ownerContactService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _productService = productService;
            _imageService = imageService;
            _ownerService = ownerService;
            _ownerContactService = ownerContactService;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "Hasło jest wymagane")]
            [DataType(DataType.Password)]
            public string Password { get; set; }
        }

        public bool RequirePassword { get; set; }

        public async Task<IActionResult> OnGet()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            RequirePassword = await _userManager.HasPasswordAsync(user);
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
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
                }

                RequirePassword = await _userManager.HasPasswordAsync(user);
                if (RequirePassword)
                {
                    if (!await _userManager.CheckPasswordAsync(user, Input.Password))
                    {
                        SetMessage("Podane hasło jest nieprawidłowe", MessageType.Error);
                        return Page();
                    }
                }

                var owner = _ownerService.GetOwnerByGuid(user.Id);
                _imageService.DeleteImageOwnerFromAzure(owner.AvatarImage);
                _ownerContactService.DeactivateOwnerContact(owner.Id);
                _productService.DeactivateProducts(user.Id);
                _ownerService.DeactivateOwner(user.Id);

                var result = await _userManager.DeleteAsync(user);
                var userId = await _userManager.GetUserIdAsync(user);
                if (!result.Succeeded)
                {
                    SetMessage("Niespodziewany błąd podczas dezaktywacji konta)", MessageType.Error);

                    throw new InvalidOperationException($"Unexpected error occurred deleting user with ID '{userId}'.");
                }

                await _signInManager.SignOutAsync();

                _logger.LogInformation("User with ID '{UserId}' deleted themselves.", userId);

                return Redirect("~/");
            }
            catch (Exception e)
            {
                _logger.LogInformation(String.Format("Data: {0}, Błąd: {1}", DateTime.Now, e));
                return Redirect("Error");
            }
        }
    }
}
