using AutoMapper;
using Demo.BLL.Interfaces;
using Demo.DAL.Models;
using Demo.PL.Helpers;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Demo.PL.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
        private readonly IMapper mapper;
        private readonly IUnitOfWork _unitOfWork;

        public EmployeeController(IMapper mapper, IUnitOfWork unitOfWork)
        {
            this.mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index(string SearchValue)
        {
            IEnumerable<Employee> employees;
            if (string.IsNullOrEmpty(SearchValue))
                employees = await _unitOfWork.EmployeeRepository.GetAll();
            else
                employees = await _unitOfWork.EmployeeRepository.GetEmployeesByName(SearchValue);

            var employeesVM = mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeViewModel>>(employees);
            return View(employeesVM);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(EmployeeViewModel employeeVM)
        {
            if (ModelState.IsValid)
            {
                employeeVM.ImageName = DocumentSettings.UploadFile(employeeVM.Image, "Images");
                var employeeM = mapper.Map<EmployeeViewModel, Employee>(employeeVM);
                await _unitOfWork.EmployeeRepository.Add(employeeM);
                await _unitOfWork.Complete();
                return RedirectToAction(nameof(Index));
            }
            return View(employeeVM);
        }
        public async Task<IActionResult> Details(int? id, string viewName = "Details")
        {
            if (id is null)
                return BadRequest();
            var Employees = await _unitOfWork.EmployeeRepository.Get(id.Value);
            if (Employees is null)
                return NotFound();
            var employeeVM = mapper.Map<Employee, EmployeeViewModel>(Employees);
            return View(viewName, employeeVM);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            return await Details(id, "Edit");
        }
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] int id, EmployeeViewModel employeeVM)
        {
            if (id != employeeVM.Id)
                return BadRequest();
            if (ModelState.IsValid)
            {
                var emp = await _unitOfWork.EmployeeRepository.Get(employeeVM.Id);
                try
                {
                    if (employeeVM.Image != null)
                    {
                        if (!string.IsNullOrEmpty(emp.ImageName))
                            DocumentSettings.DeleteFile(emp.ImageName, "Images");
                        employeeVM.ImageName = DocumentSettings.UploadFile(employeeVM.Image, "Images");
                    }
                    else
                        employeeVM.ImageName = emp.ImageName;

                    _unitOfWork.EmployeeRepository.DetachEntity(emp);
                    var employeeM = mapper.Map<EmployeeViewModel, Employee>(employeeVM);
                    _unitOfWork.EmployeeRepository.Update(employeeM);
                    await _unitOfWork.Complete();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(employeeVM);
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            return await Details(id, "Delete");
        }
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute] int id, EmployeeViewModel employeeVM)
        {
            if (id != employeeVM.Id)
                return BadRequest();
            try
            {
                var employeeM = mapper.Map<EmployeeViewModel, Employee>(employeeVM);
                _unitOfWork.EmployeeRepository.Delete(employeeM);
                var count = await _unitOfWork.Complete();
                if (count > 0)
                    DocumentSettings.DeleteFile(employeeVM.ImageName, "Images");
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(employeeVM);
            }
        }
    }
}
