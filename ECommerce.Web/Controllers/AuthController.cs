using ECommerce.Service.Operations.Users;
using ECommerce.Service.Operations.Users.Dtos;
using ECommerce.Service.Types;
using ECommerce.Web.Jwt;
using ECommerce.Web.Models.User;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IUserService _userService;
        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        public IActionResult Register() => View();

        [HttpPost("register")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(UserRegisterRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }

            var result = await _userService.AddAsync(new UserAddDto
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                Password = request.Password,
                PhoneNumber = request.PhoneNumber,
            });

            if (!result.IsSucced)
            {
                ModelState.AddModelError("", result.Massage);
                return View(request);
            }

            return RedirectToAction("Login");
        }

        public IActionResult Login() => View();

        [HttpPost("login")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(UserLoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }

            var result = await _userService.LoginAsync(new LoginUserDto
            {
                Email = request.Email,
                Password = request.Password
            });

            if (!result.IsSucced)
            {
                ModelState.AddModelError("", result.Massage);
                return View(request);
            }

            ViewBag.Token = result.Data != null ? result.Data.Token : null;

            // Login başarılıysa, ana sayfaya yönlendir
            return RedirectToAction("Index", "Home");
        }

        public IActionResult ChangePassword() => View();

        [HttpPost("change-password")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(UserChangePasswordRequest request)
        {

            if (!ModelState.IsValid)
            {
                return View(request);
            }

            var result = await _userService.ChangePasswordAsync(new UserChangePasswordDto
            {
                UserId = request.UserId,
                CurrentPassword = request.CurrentPassword,
                NewPassword = request.NewPassword,
                ConfrimPassword = request.ConfirmPassword
            });

            if (!result.IsSucced)
            {
                ModelState.AddModelError("", result.Massage);
                return View(request);
            }

            return RedirectToAction("Index", "Home");
        }

        public IActionResult ForgotPassword() => View();

        [HttpPost("forgot-password")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(UserForgotPasswordRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }

            var result = await _userService.ForgotPasswordAsync(new UserForgotPasswordDto
            {
                Email = request.Email,
            });

            ViewBag.Message = result.Massage;
            return View();
        }

        public IActionResult ResetPassword() => View();

        [HttpPost("reset-password")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(UserResetPasswordRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }

            var result = await _userService.ResetPasswordAsync(new UserResetPasswordDto
            {
                Token = request.Token,
                NewPassword = request.NewPassword,
                ConfirmPassword = request.ConfirmPassword
            });

            ViewBag.Message = result.Massage;
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> UpdateUser(int id)
        {
            var result = await _userService.GetByIdAsync(id);

            if (!result.IsSucced || result.Data is null)
            {
                TempData["Error"] = result.Massage;
                return RedirectToAction("Index", "Home");
            }

            var model = new UserUpdateRequest
            {
                Id = result.Data.Id,
                Email = result.Data.Email,
                FirstName = result.Data.FirstName,
                LastName = result.Data.LastName,
                PhoneNumber = result.Data.PhoneNumber,
            };

            return View(model);
        }

        [HttpPost("update-user")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateUser(UserUpdateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }

            var result = await _userService.UpdateAsync(new UserUpdateDto
            {
                Id = request.Id,
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                PhoneNumber = request.PhoneNumber,
            });

            if (!result.IsSucced)
            {
                ModelState.AddModelError("", result.Massage);

                return View(result);
            }

            TempData["Success"] = "Kullanıcı başarıyla güncellendi";
            return RedirectToAction("Index", "Home");
        }

        [HttpPost("delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _userService.Delete(id);

            if (!result.IsSucced)
            {
                TempData["Error"] = result.Massage;

            }
            else
            {
                TempData["Success"] = result.Massage;

            }
            return RedirectToAction("Index", "Home");
        }
    }
}
