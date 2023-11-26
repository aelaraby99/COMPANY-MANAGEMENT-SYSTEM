using AutoMapper;
using Demo.DAL.Models;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.PL.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IMapper mapper;

        public UserController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IMapper mapper)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.mapper = mapper;
        }
        public async Task<IActionResult> Index(string email)
        {

            if (string.IsNullOrEmpty(email))
            {
                var users = await userManager.Users.Select(u => new UserViewModel()
                {
                    Id = u.Id,
                    FName = u.FName,
                    LName = u.LName,
                    Email = u.Email,
                    PhoneNumber = u.PhoneNumber,
                    Roles = userManager.GetRolesAsync(u).Result

                }).ToListAsync();
                return View(users);
            }
            else
            {
                var user = await userManager.FindByEmailAsync(email);
                var MappedUser = new UserViewModel()
                {
                    Id = user.Id,
                    FName = user.FName,
                    LName = user.LName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    Roles = userManager.GetRolesAsync(user).Result
                };
                return View(new List<UserViewModel>() { MappedUser });
            }
        }

        public async Task<IActionResult> Details(string id, string viewName = "Details")
        {
            if (id is null)
                return BadRequest();
            var User = await userManager.FindByIdAsync(id);
            if (User is null)
                return NotFound();
            var userVM = mapper.Map<ApplicationUser, UserViewModel>(User);
            return View(viewName, userVM);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            return await Details(id, "Edit");
        }
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] string id, UserViewModel userVM)
        {
            if (id != userVM.Id)
                return BadRequest();
            if (ModelState.IsValid)
            {
                try
                {
                    var user = await userManager.FindByIdAsync(id);
                    var userM = mapper.Map<UserViewModel, ApplicationUser>(userVM);
                    user.FName = userVM.FName;
                    user.LName = userVM.LName;
                    user.PhoneNumber = userVM.PhoneNumber;
                    await userManager.UpdateAsync(user);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    //1. Log Exception
                    //2. Friendly Message
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(userVM);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            return await Details(id, "Delete");
        }
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute] string id, UserViewModel UserVM)
        {
            if (id != UserVM.Id)
                return BadRequest();
            try
            {
                var user = await userManager.FindByIdAsync(id);
                await userManager.DeleteAsync(user);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(UserVM);
            }
        }
    }
}
