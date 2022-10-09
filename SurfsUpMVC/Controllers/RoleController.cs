using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using SurfsUp.Models;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace SurfsUp.Controllers
{
    // Controller class for CRUD opreations on users
    // Authorization used to deny access for unapproved users
    [Authorize(Roles = "SuperAdmin")]
    public class RoleController : Controller
    {
        // Get instance of RoleManager and UserManager through Dependency Injection
        private RoleManager<IdentityRole> roleManager;
        private UserManager<AppUser> userManager;

        // Constructor for controller, adds roleManager and UserManager
        public RoleController(RoleManager<IdentityRole> roleMgr, UserManager<AppUser> userMrg)
        {
            roleManager = roleMgr;
            userManager = userMrg;
        }

        // Get all identity roles and return as a model, to the index view
        public ViewResult Index() => View(roleManager.Roles);

        private void Errors(IdentityResult result)
        {
            foreach (IdentityError error in result.Errors)
                ModelState.AddModelError("", error.Description);
        }

        #region CREATE
        // Create action method for new users to Identity DB

        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create([Required] string name)
        {
            // Model Validation, if its succeded, it creates new instance of AppUser
            if (ModelState.IsValid)
            {
                // Create role (name) from method, from roleManager class. Return result.
                IdentityResult result = await roleManager.CreateAsync(new IdentityRole(name));
                
                // Succes = return to index site, else give proper errormessage
                if (result.Succeeded)
                    return RedirectToAction("Index");
                else
                    Errors(result);
            }
            // Return to create view, here errors will be shown
            return View(name);
        }

        #endregion

        #region EDIT
        // Edit action method for users in Identity DB

        
        // GET: Roles
        public async Task<IActionResult> Edit(string id)
        {
            IdentityRole role = await roleManager.FindByIdAsync(id);
            List<AppUser> members = new List<AppUser>();
            List<AppUser> nonMembers = new List<AppUser>();
            foreach (AppUser user in userManager.Users)
            {
                // Returns true if the user is a member of a specified role else returns false.
                var list = await userManager.IsInRoleAsync(user, role.Name) ? members : nonMembers;
                list.Add(user);
            }
            //Uses RoleEdit class to find roles and associated users
            return View(new RoleEdit
            {
                Role = role,
                Members = members,
                NonMembers = nonMembers
            });
        }

        //Uses RoleModification class to edit associated users to role
        [HttpPost]
        public async Task<IActionResult> Edit(RoleModification model)
        {
            IdentityResult result;
            
            // Model Validation, if its succeded, it adds or removes user from role
            if (ModelState.IsValid)
            {
                foreach (string userId in model.AddIds ?? new string[] { })
                {
                    // Recieve user id and fetches record
                    AppUser user = await userManager.FindByIdAsync(userId);
                    if (user != null)
                    {
                        // Adds a user to an Identity Role.
                        result = await userManager.AddToRoleAsync(user, model.RoleName);
                        if (!result.Succeeded)
                            Errors(result);
                    }
                }
                foreach (string userId in model.DeleteIds ?? new string[] { })
                {
                    // Recieve user id and fetches record
                    AppUser user = await userManager.FindByIdAsync(userId);
                    if (user != null)
                    {
                        // Removes a user to an Identity Role.
                        result = await userManager.RemoveFromRoleAsync(user, model.RoleName);
                        if (!result.Succeeded)
                            Errors(result);
                    }
                }
            }

            if (ModelState.IsValid)
                return RedirectToAction(nameof(Index));
            else
                return await Edit(model.RoleId);
        }

        #endregion

        #region DELETE
        // Delete action method for roles in Identity DB

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            // Recieve role id and fetches record
            IdentityRole role = await roleManager.FindByIdAsync(id);
            if (role != null)
            {
                // Deletes the role and returns to index view
                IdentityResult result = await roleManager.DeleteAsync(role);
                if (result.Succeeded)
                    return RedirectToAction("Index");
                else
                    Errors(result);
            }
            // Errormessaging if method isn't succesfull
            else
                ModelState.AddModelError("", "No role found");
            return View("Index", roleManager.Roles);
        }

        #endregion

    }
}
