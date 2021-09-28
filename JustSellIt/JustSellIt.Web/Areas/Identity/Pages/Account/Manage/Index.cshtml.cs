using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using JustSellIt.Application.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace JustSellIt.Web.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IOwnerService _ownerService;
        private readonly IOwnerContactService _ownerContactService;

        public IndexModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            IOwnerContactService ownerContactService,
            IOwnerService ownerService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _ownerService = ownerService;
            _ownerContactService = ownerContactService;
        }

        public string Username { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            public string Name { get; set; }
            public string AvatarImage { get; set; }
            public int SexId { get; set; }
            public string City { get; set; }
            public string Email { get; set; }
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
                AvatarImage = owner.AvatarImage,
                SexId = owner.SexId,
                City = owner.City,
                Email = ownerContact.Email,
                PhoneNumber = ownerContact.PhoneNumber
            };


        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
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


            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}
