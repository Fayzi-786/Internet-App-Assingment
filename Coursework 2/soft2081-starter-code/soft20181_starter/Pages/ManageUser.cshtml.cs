using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using soft20181_starter.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace soft20181_starter.Pages
{
    [Authorize(Roles = "Admin")]
    public class ManageUserModel : PageModel
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<UsersInfo> _userManager;

        public List<IdentityRole> Roles { get; set; } = new List<IdentityRole>();
        public List<UsersInfo> Users { get; set; } = new List<UsersInfo>();

        [BindProperty]
        [Required(ErrorMessage = "Role name is required.")]
        [StringLength(50, ErrorMessage = "Role name cannot exceed 50 characters.")]
        public string RoleName { get; set; }

        [BindProperty]
        public string Id { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        public ManageUserModel(RoleManager<IdentityRole> roleManager, UserManager<UsersInfo> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            Roles = _roleManager.Roles.ToList();
            Users = _userManager.Users.ToList();
            return Page();
        }

        public async Task<IActionResult> OnPostAddRoleAsync()
        {
            if (!ModelState.IsValid)
            {
                Roles = _roleManager.Roles.ToList();
                Users = _userManager.Users.ToList();
                return Page();
            }

            RoleName = RoleName.Trim();
            var roleExists = await _roleManager.RoleExistsAsync(RoleName);

            if (!roleExists)
            {
                var result = await _roleManager.CreateAsync(new IdentityRole(RoleName));

                if (result.Succeeded)
                {
                    return RedirectToPage();
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Role already exists.");
            }

            Roles = _roleManager.Roles.ToList();
            Users = _userManager.Users.ToList();
            return Page();
        }

        public async Task<IActionResult> OnPostDeleteRoleAsync()
        {
            if (string.IsNullOrEmpty(Id))
            {
                return NotFound();
            }

            var role = await _roleManager.FindByIdAsync(Id);
            if (role == null)
            {
                return NotFound();
            }

            var result = await _roleManager.DeleteAsync(role);
            if (result.Succeeded)
            {
                return RedirectToPage();
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            Roles = _roleManager.Roles.ToList();
            Users = _userManager.Users.ToList();
            return Page();
        }

        public async Task<IActionResult> OnPostDeleteUserAsync()
        {
            if (string.IsNullOrEmpty(Id))
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(Id);
            if (user == null)
            {
                return NotFound();
            }

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return RedirectToPage();
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            Roles = _roleManager.Roles.ToList();
            Users = _userManager.Users.ToList();
            return Page();
        }

        public async Task<IActionResult> OnPostUpdatePasswordAsync()
        {
            if (!ModelState.IsValid)
            {
                Roles = _roleManager.Roles.ToList();
                Users = _userManager.Users.ToList();
                return Page();
            }

            var user = await _userManager.FindByIdAsync(Id);
            if (user == null)
            {
                return NotFound();
            }

            // Remove existing password
            var removePasswordResult = await _userManager.RemovePasswordAsync(user);
            if (!removePasswordResult.Succeeded)
            {
                foreach (var error in removePasswordResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return Page();
            }

            // Add new password
            var addPasswordResult = await _userManager.AddPasswordAsync(user, NewPassword);
            if (addPasswordResult.Succeeded)
            {
                return RedirectToPage();
            }

            foreach (var error in addPasswordResult.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            Roles = _roleManager.Roles.ToList();
            Users = _userManager.Users.ToList();
            return Page();
        }
    }
}