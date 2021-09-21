using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using JustSellIt.Application.Interfaces;
using JustSellIt.Domain.Model;
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

        public RegisterModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            IOwnerService ownerService,
            IOwnerContactService ownerContactService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _ownerService = ownerService;
            _ownerContactService = ownerContactService;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required (ErrorMessage ="Adres e-mail jest wymagany")]
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
            [StringLength(15, ErrorMessage = "Imię powinno zawierać się od {2} do {1} znaków", MinimumLength = 3)]
            [Display(Name = "Imię")]
            public string Name { get; set; }


            [Display(Name = "Zdjęcie")]
            public IFormFile AvatarImage { get; set; }

            [Required]
            [Display(Name = "Płeć")]
            public int SexId { get; set; }

            [StringLength(30, ErrorMessage = "Miasto powinno zawierać się od {2} do {1} znaków")]
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

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = new IdentityUser { UserName = Input.Email, Email = Input.Email };
                var result = await _userManager.CreateAsync(user, Input.Password);
                
                if (result.Succeeded)
                {
                    //_logger.LogInformation("User created a new account with password.");
                    var owner = new Owner()
                    {
                        Name = Input.Name,
                        //AvatarImage
                        SexId = Input.SexId,
                        City = Input.City,
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

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
