using JustSellIt.Application;
using JustSellIt.Application.Interfaces;
using JustSellIt.Application.ViewModels.Base;
using JustSellIt.Web.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace JustSellIt.Web.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IOwnerService _ownerService;
        private readonly IOwnerContactService _ownerContactService;
        private readonly IImageService _imageService;
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            IOwnerContactService ownerContactService,
            IOwnerService ownerService,
            IImageService imageService,
            ILogger<IndexModel> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _ownerService = ownerService;
            _ownerContactService = ownerContactService;
            _imageService = imageService;
            _logger = logger;
        }

        public string Username { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "Imię jest wymagane")]
            [RegularExpression(@"^[A-Z a-z]*$", ErrorMessage = "Imię może zawierać tylko litery bez polskich znaków")]
            [StringLength(15, ErrorMessage = "Imię powinno zawierać się od {2} do {1} znaków", MinimumLength = 3)]
            [Display(Name = "Imię")]
            public string Name { get; set; }
            public IFormFile AvatarImage { get; set; }
            public string AvatarUrl { get; set; }
            [Required]
            [Display(Name = "Płeć")]
            public int SexId { get; set; }
            [RegularExpression(@"^[A-Z a-z żźćńółęąśŻŹĆĄŚĘŁÓŃ]*$", ErrorMessage = "Miasto może zawierać tylko litery")]
            [StringLength(30, ErrorMessage = "Miasto powinno zawierać się do {1} znaków")]
            [Display(Name = "Miasto")]
            public string City { get; set; }
            public string Email { get; set; }
            [Display(Name = "Telefon")]
            [RegularExpression(@"^[0-9]{9}$", ErrorMessage = "Nieprawidłowy format numeru telefonu")]
            public string PhoneNumber { get; set; }
        }

        private async Task LoadAsync(IdentityUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var owner = _ownerService.GetOwnerByGuid(user.Id);
            var ownerContact = _ownerContactService.GetOwnerContactByOwner(owner.Id);

            Username = userName;

            Input = new InputModel
            {
                Name = owner.Name,
                AvatarUrl = owner.AvatarImage == null ? null : SystemConfiguration.OwnerImageUrl.Replace("{{name}}", owner.AvatarImage),
                SexId = owner.SexId,
                City = owner.City,
                Email = ownerContact.Email,
                PhoneNumber = ownerContact.PhoneNumber
            };


        }

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
                }

                await LoadAsync(user);
                return Page();
            }
            catch (Exception e)
            {
                _logger.LogInformation(String.Format("Data: {0}, Błąd: {1}", DateTime.Now, e));
                return Redirect("Error");
            }
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

                if (!ModelState.IsValid)
                {
                    await LoadAsync(user);
                    return Page();
                }

                var image = _imageService.UploadOwnerToAzure(Input.AvatarImage);
                var owner = _ownerService.GetOwnerByGuid(user.Id);
                var ownerContact = _ownerContactService.GetOwnerContactByOwner(owner.Id);

                if (string.IsNullOrEmpty(Input.AvatarUrl) || image != null)
                {
                    _imageService.DeleteImageOwnerFromAzure(owner.AvatarImage);
                }

                ownerContact.PhoneNumber = Input.PhoneNumber;
                _ownerContactService.UpdateOwnerContact(ownerContact);

                owner.Name = StringHelper.CapitalizeFirstLetter(Input.Name);
                owner.AvatarImage = Input.AvatarUrl == null ? image : owner.AvatarImage;
                owner.SexId = Input.SexId;
                owner.City = StringHelper.CapitalizeFirstLetter(Input.City);
                _ownerService.UpdateOwner(owner);

                await _signInManager.RefreshSignInAsync(user);
                SetMessage("Profil został zaktualizowany", MessageType.Success);
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
