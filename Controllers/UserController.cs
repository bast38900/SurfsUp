using SurfsUp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SurfsUp.Data;
using Microsoft.AspNetCore.Authorization;

namespace Identity.Controllers
{
    // Controller class for CRUD opreations on users
    // Authorization used to deny access for unapproved users
    [Authorize(Roles = "SuperAdmin")]
    public class UserController : Controller
    {
        // Get instance of UserManager and IpasswordHasher through Dependency Injection
        private UserManager<AppUser> userManager;
        private IPasswordHasher<AppUser> passwordHasher;

        
        // Constructor for controller, adds user and passwordhasher (to encrypt passwords)
        public UserController(UserManager<AppUser> usrMgr, IPasswordHasher<AppUser> passwordHash)
        {
            userManager = usrMgr;
            passwordHasher = passwordHash;
        }

        // Get all users from identity DB, via Users Property (IEnumerable object)
        public IActionResult Index()
        {
            return View(userManager.Users);
        }

        #region CREATE 
        // Create action method for new users to Identity DB

        public ViewResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(User user)
        {
            
            // Model Validation, if its succeded, it creates new instance of AppUser
            if (ModelState.IsValid)
            {
                // Sets Email to UserName
                AppUser appUser = new AppUser
                {
                    UserName = user.Email,
                    Email = user.Email
                };

                // Create user from method, from userManager class. Return result.                
                IdentityResult result = await userManager.CreateAsync(appUser, user.Password);

                // Succes = return to index site, else give proper errormessage
                if (result.Succeeded)
                    return RedirectToAction("Index");
                else
                {
                    foreach (IdentityError error in result.Errors)
                        ModelState.AddModelError("", error.Description);
                }
            }
            // Return to create view, here errors will be shown
            return View(user);
        }

        #endregion

        #region EDIT
        // Edit action method for users in Identity DB

        // GET: user
        public async Task<IActionResult> Edit(string id)
        {
            // Recieve user id and fetches record
            AppUser user = await userManager.FindByIdAsync(id);
            if (user != null)
                return View(user);
            else
                return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id, string email, string password)
        {
            // Recieve user id and fetches record
            AppUser user = await userManager.FindByIdAsync(id);
            if (user != null)
            {
                
                // If AppUser object is not null, check new values not empty and update properties 
                if (!string.IsNullOrEmpty(email))
                    user.Email = email;
                else
                    ModelState.AddModelError("", "Email cannot be empty");

                if (!string.IsNullOrEmpty(password))
                    user.PasswordHash = passwordHasher.HashPassword(user, password);
                else
                    ModelState.AddModelError("", "Password cannot be empty");

                // Use UpdateAsync method to update user
                if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(password))
                {
                    IdentityResult result = await userManager.UpdateAsync(user);
                    if (result.Succeeded)
                        return RedirectToAction("Index");
                    else
                        Errors(result);
                }
            }
            else
                ModelState.AddModelError("", "User Not Found");
            return View(user);
        }

        // Errormessaging if method isn't succesfull
        private void Errors(IdentityResult result)
        {
            foreach (IdentityError error in result.Errors)
                ModelState.AddModelError("", error.Description);
        }

        #endregion

        #region DELETE
        // Delete action method for users in Identity DB

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            // Recieve user id and fetches record
            AppUser user = await userManager.FindByIdAsync(id);
            if (user != null)
            {
                // Deletes the user and returns to index view
                IdentityResult result = await userManager.DeleteAsync(user);
                if (result.Succeeded)
                    return RedirectToAction("Index");
                else
                    Errors(result);
            }
            // Errormessaging if method isn't succesfull
            else
                ModelState.AddModelError("", "User Not Found");
            return View("Index", userManager.Users);
        }

        #endregion

    }
}