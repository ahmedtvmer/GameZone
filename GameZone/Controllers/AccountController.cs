using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GameZone.Controllers
{
    [Authorize(Roles ="Admin")]
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            this._userManager = userManager;
            this._roleManager = roleManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> ManageUsers()
        {
            var users = _userManager.Users.ToList();
            var model = new ManageUsersViewModel()
            {
                Users = users,
                UserRoles = new(),
                AllRoles = _roleManager.Roles.ToList()
            };
            foreach (var user in users)
            {
                model.UserRoles[user.Id] = await _userManager.GetRolesAsync(user);
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRole(string userId, string role)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();

            if (string.IsNullOrEmpty(role))
            {
                // Remove all roles if blank is selected
                var currentRoles = await _userManager.GetRolesAsync(user);
                if (currentRoles.Any())
                {
                    var result = await _userManager.RemoveFromRolesAsync(user, currentRoles);
                    if (!result.Succeeded)
                    {
                        ModelState.AddModelError("", "Failed to remove roles: " + string.Join(", ", result.Errors.Select(e => e.Description)));
                    }
                }
            }
            else
            {
                // Add or remove the selected role based on current state
                if (!await _roleManager.RoleExistsAsync(role))
                {
                    var roleResult = await _roleManager.CreateAsync(new IdentityRole(role));
                    if (!roleResult.Succeeded)
                    {
                        ModelState.AddModelError("", "Failed to create role: " + string.Join(", ", roleResult.Errors.Select(e => e.Description)));
                        return RedirectToAction(nameof(ManageUsers));
                    }
                }

                if (await _userManager.IsInRoleAsync(user, role))
                {
                    var removeResult = await _userManager.RemoveFromRoleAsync(user, role);
                    if (!removeResult.Succeeded)
                    {
                        ModelState.AddModelError("", "Failed to remove role: " + string.Join(", ", removeResult.Errors.Select(e => e.Description)));
                    }
                }
                else
                {
                    var addResult = await _userManager.AddToRoleAsync(user, role);
                    if (!addResult.Succeeded)
                    {
                        ModelState.AddModelError("", "Failed to add role: " + string.Join(", ", addResult.Errors.Select(e => e.Description)));
                    }
                }
            }

            return RedirectToAction(nameof(ManageUsers));
        }
    }
}
