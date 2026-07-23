using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TaskManagementSystem.Service.DTOs.Common;
using TaskManagementSystem.Service.DTOs.User;
using TaskManagementSystem.Service.IServices;
using TaskManagementSystem.Web.Extensions;
using TaskManagementSystem.Web.Models;

namespace TaskManagementSystem.Web.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Index()
        {
            var users = await _userService.GetAllAsync();
            var viewModels = users.Select(ToViewModel);
            return View(viewModels);
        }

        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var user = await _userService.GetByIdAsync(id);
                return View(ToViewModel(user));
            }
            catch (AppException ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Create()
        {
            await LoadRolesAsync();
            return View(new CreateUserViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Create(CreateUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await LoadRolesAsync();
                return View(model);
            }

            try
            {
                var dto = new CreateUserDto
                {
                    UserName = model.UserName,
                    Password = model.Password,
                    FullName = model.FullName,
                    Email = model.Email,
                    Phone = model.Phone,
                    RoleId = model.RoleId
                };

                await _userService.CreateAsync(dto, User.GetUserId());
                TempData["Success"] = "Đã cấp tài khoản mới.";
                return RedirectToAction(nameof(Index));
            }
            catch (AppException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                await LoadRolesAsync();
                return View(model);
            }
        }

        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var user = await _userService.GetByIdAsync(id);
                var model = new UpdateUserViewModel
                {
                    FullName = user.FullName,
                    Email = user.Email,
                    Phone = user.Phone,
                    RoleId = user.RoleId,
                    IsActive = user.IsActive
                };

                ViewBag.UserId = id;
                ViewBag.UserName = user.UserName;
                await LoadRolesAsync();
                return View(model);
            }
            catch (AppException ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Edit(int id, UpdateUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.UserId = id;
                await LoadRolesAsync();
                return View(model);
            }

            try
            {
                var dto = new UpdateUserDto
                {
                    FullName = model.FullName,
                    Email = model.Email,
                    Phone = model.Phone,
                    RoleId = model.RoleId,
                    IsActive = model.IsActive
                };

                await _userService.UpdateAsync(id, dto, User.GetUserId());
                TempData["Success"] = "Đã cập nhật tài khoản.";
                return RedirectToAction(nameof(Index));
            }
            catch (AppException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                ViewBag.UserId = id;
                await LoadRolesAsync();
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _userService.DeleteAsync(id, User.GetUserId());
                TempData["Success"] = "Đã khóa tài khoản.";
            }
            catch (AppException ex)
            {
                TempData["Error"] = ex.Message;
            }
            return RedirectToAction(nameof(Index));
        }

        private async Task LoadRolesAsync()
        {
            var roles = await _userService.GetRolesAsync();
            ViewBag.Roles = new SelectList(roles, "RoleId", "RoleName");
        }

        private static UserViewModel ToViewModel(UserDto dto) => new()
        {
            UserAccountId = dto.UserAccountId,
            UserName = dto.UserName,
            FullName = dto.FullName,
            Email = dto.Email,
            Phone = dto.Phone,
            RoleId = dto.RoleId,
            RoleName = dto.RoleName,
            IsActive = dto.IsActive
        };
    }
}