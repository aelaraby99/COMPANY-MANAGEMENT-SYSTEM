using AutoMapper;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace Demo.PL.Controllers
{
    [Authorize]
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IMapper mapper;

        public RoleController(RoleManager<IdentityRole> roleManager, IMapper mapper)
        {
            this.roleManager = roleManager;
            this.mapper = mapper;
        }
        public async Task<IActionResult> Index(string Name)
        {

            if (string.IsNullOrEmpty(Name))
            {
                var roles = await roleManager.Roles.Select(R => new RoleViewModel()
                {
                    Id = R.Id,
                    RoleName = R.Name,
                }).ToListAsync();
                return View(roles);
            }
            else
            {
                var role = await roleManager.FindByNameAsync(Name);
                var RoleVM = new RoleViewModel()
                {
                    Id = role.Id,
                    RoleName = role.Name,
                };
                return View(new List<RoleViewModel>() { RoleVM });
            }
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(RoleViewModel RoleVM)
        {

            if (ModelState.IsValid)
            {

                var RoleM = mapper.Map<RoleViewModel, IdentityRole>(RoleVM);
                await roleManager.CreateAsync(RoleM);
                return RedirectToAction(nameof(Index));
            }
            return View(RoleVM);
        }

        public async Task<IActionResult> Details(string id, string viewName = "Details")
        {
            if (id is null)
                return BadRequest();
            var Role = await roleManager.FindByIdAsync(id);
            if (Role is null)
                return NotFound();
            var RoleVM = mapper.Map<IdentityRole, RoleViewModel>(Role);
            return View(viewName, RoleVM);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            return await Details(id, "Edit");
        }
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] string id, RoleViewModel roleVM)
        {
            if (id != roleVM.Id)
                return BadRequest();
            if (ModelState.IsValid)
            {
                try
                {
                    var role = await roleManager.FindByIdAsync(id);
                    role.Name = roleVM.RoleName;
                    await roleManager.UpdateAsync(role);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(roleVM);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            return await Details(id, "Delete");
        }
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute] string id, RoleViewModel roleVM)
        {
            if (id != roleVM.Id)
                return BadRequest();
            try
            {
                var role = await roleManager.FindByIdAsync(id);
                await roleManager.DeleteAsync(role);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(roleVM);
            }
        }
    }
}
