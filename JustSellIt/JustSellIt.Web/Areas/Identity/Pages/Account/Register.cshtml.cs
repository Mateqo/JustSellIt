using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using JustSellIt.Application;
using JustSellIt.Application.Interfaces;
using JustSellIt.Application.ViewModels.Base;
using JustSellIt.Domain.Model;
using JustSellIt.Web.Helpers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;

namespace JustSellIt.Web.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly IOwnerService _ownerService;
        private readonly IOwnerContactService _ownerContactService;
        private readonly IImageService _imageService;

        public RegisterModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            IOwnerService ownerService,
            IOwnerContactService ownerContactService,
            IImageService imageService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _ownerService = ownerService;
            _ownerContactService = ownerContactService;
            _imageService = imageService;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "Adres e-mail jest wymagany")]
            [EmailAddress(ErrorMessage = "Format adresu e-mail nie jest poprawny")]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required(ErrorMessage = "Hasło jest wymagane")]
            [StringLength(50, ErrorMessage = "Hasło powinno składać się od {2} do {1} znaków", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Hasło")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Potwierdź hasło")]
            [Compare("Password", ErrorMessage = "Hasła nie są identyczne")]
            public string ConfirmPassword { get; set; }

            [Required(ErrorMessage = "Imię jest wymagane")]
            [RegularExpression(@"^[A-Z a-z]*$", ErrorMessage = "Imię może zawierać tylko litery bez polskich znaków")]
            [StringLength(15, ErrorMessage = "Imię powinno zawierać się od {2} do {1} znaków", MinimumLength = 3)]
            [Display(Name = "Imię")]
            public string Name { get; set; }


            [Display(Name = "Zdjęcie")]
            public IFormFile AvatarImage { get; set; }

            [Required]
            [Display(Name = "Płeć")]
            public int SexId { get; set; }

            [RegularExpression(@"^[A-Z a-z żźćńółęąśŻŹĆĄŚĘŁÓŃ]*$", ErrorMessage = "Miasto może zawierać tylko litery")]
            [StringLength(30, ErrorMessage = "Miasto powinno zawierać do {1} znaków")]
            [Display(Name = "Miasto")]
            public string City { get; set; }

            [Display(Name = "Telefon")]
            [RegularExpression(@"^[0-9]{9}$", ErrorMessage = "Nieprawidłowy format numeru telefonu")]
            public string PhoneNumber { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public void SetMessage(string message, MessageType type)
        {
            TempData["SM"] = message;
            TempData["SMT"] = type;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            bool isExistsConfirmedEmail = false;
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            var userToVerify = await _userManager.FindByEmailAsync(Input.Email);

            if (userToVerify != null && userToVerify.EmailConfirmed)
            {
                isExistsConfirmedEmail = true;
            }

            if (ModelState.IsValid)
            {
                var user = new IdentityUser { UserName = Input.Email, Email = Input.Email };
                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    //_logger.LogInformation("User created a new account with password.");

                    var imageName = _imageService.UploadOwnerToAzure(Input.AvatarImage);

                    var owner = new Owner()
                    {
                        Name = StringHelper.CapitalizeFirstLetter(Input.Name),
                        AvatarImage = imageName,
                        SexId = Input.SexId,
                        City = StringHelper.CapitalizeFirstLetter(Input.City),
                        UserGuid = user.Id
                    };
                    var ownerId = _ownerService.AddOwner(owner);

                    var ownerContact = new OwnerContact()
                    {
                        Email = Input.Email,
                        PhoneNumber = Input.PhoneNumber,
                        OwnerRef = ownerId,
                    };

                    _ownerContactService.AddOwnerContact(ownerContact);

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    EmailSender.SendEmail(callbackUrl, Input.Email, Input.Name, EmailType.Confirmation);

                    return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });


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
                        case "DuplicateUserName":
                            if (isExistsConfirmedEmail)
                                SetMessage("Konto o podanym adresie e-mail już istnieje", MessageType.Error);
                            else
                                SetMessage("Na podany adres e-mail został wysłany już link aktywacyjny", MessageType.Error);
                            break;
                        default:
                            SetMessage(error.Description, MessageType.Error);
                            break;
                    }
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
