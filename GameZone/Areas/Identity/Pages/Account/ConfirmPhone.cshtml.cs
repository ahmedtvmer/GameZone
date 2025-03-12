using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GameZone.Areas.Identity.Pages.Account
{
    public class ConfirmPhoneModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly GameZone.Services.ISmsSender _smsSender;

        public ConfirmPhoneModel(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, GameZone.Services.ISmsSender smsSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _smsSender = smsSender;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            public string UserId { get; set; }

            [Required]
            [Display(Name = "Verification Code")]
            public string Code { get; set; }
        }

        public async Task<IActionResult> OnGetAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToPage("./Register");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{userId}'.");
            }

            Input = new InputModel { UserId = userId };
            await SendConfirmationCode(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.FindByIdAsync(Input.UserId);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{Input.UserId}'.");
            }

            var result = await _userManager.VerifyChangePhoneNumberTokenAsync(user, Input.Code, user.PhoneNumber);
            if (result)
            {
                user.PhoneNumberConfirmed = true;
                var updateResult = await _userManager.UpdateAsync(user);
                if (updateResult.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return LocalRedirect("/"); // Redirect to homepage after confirmation
                }
                foreach (var error in updateResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid verification code.");
            }

            return Page();
        }

        private async Task SendConfirmationCode(IdentityUser user)
        {
            var code = await _userManager.GenerateChangePhoneNumberTokenAsync(user, user.PhoneNumber);
            await _smsSender.SendSmsAsync(user.PhoneNumber, $"Your GameZone verification code is: {code}");
        }
    }
}
