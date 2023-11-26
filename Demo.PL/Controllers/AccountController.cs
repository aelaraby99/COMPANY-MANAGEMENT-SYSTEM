using Demo.DAL.Models;
using Demo.PL.Helpers;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace Demo.PL.Controllers
{

    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
        }
        #region SignUP
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> SignUp(SignUpViewModel SignUpVM)
        {
            if (ModelState.IsValid)
            {
                if (IsExist(SignUpVM.Email).Result.Value)
                {
                    ModelState.AddModelError(string.Empty, "Email account already exists, Please try login instead");
                    return View(SignUpVM);
                }
                var User = new ApplicationUser()
                {
                    Email = SignUpVM.Email,
                    UserName = SignUpVM.Email.Split('@')[0],
                    FName = SignUpVM.FName,
                    LName = SignUpVM.LName,
                    IsAgree = SignUpVM.IsAgree,
                };
                var Result = await userManager.CreateAsync(User, SignUpVM.Password);

                if (Result.Succeeded)
                    return RedirectToAction(nameof(SignIn));

                foreach (var error in Result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);
            }
            return View(SignUpVM);
        }
        #endregion
        #region SignIn
        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> SignIn(SignInViewModel SignInVM)
        {
            dynamic user;
            if (ModelState.IsValid)
            {
                if (new EmailAddressAttribute().IsValid(SignInVM.Email))
                    user = await userManager.FindByEmailAsync(SignInVM.Email);
                else
                    user = SignInVM.Email;

                if (user is not null)
                {
                    var Result = (SignInResult)await signInManager.PasswordSignInAsync(user, SignInVM.Password, SignInVM.RememberMe, false);
                    if (Result.Succeeded)
                        return RedirectToAction("Index", "Home");
                    ModelState.AddModelError(string.Empty, "Result have a problem");
                }
                ModelState.AddModelError(string.Empty, "Username or Password is incorrect");
            }
            return View(SignInVM);
        }
        #endregion
        #region SignOut
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction(nameof(SignIn));
        }
        #endregion
        #region ForgetPassword
        public IActionResult ForgetPassword()
        {
            return View();
        }
        #endregion
        #region SendEmail
        [HttpPost]
        public async Task<IActionResult> SendEmail(ForgetPasswordViewModel forgetPasswordVM)
        {
            if (ModelState.IsValid)
            {
                var User = await userManager.FindByEmailAsync(forgetPasswordVM.Email);
                if (User is not null)
                {
                    var Token = await userManager.GeneratePasswordResetTokenAsync(User);
                    var GeneratePasswordResetLink = Url.Action("ResetPassword", "Account", new { User.Email, Token }, Request.Scheme);
                    ///var email = new Email()
                    ///{
                    ///    Subject = "Reset Password",
                    ///    To = User.Email,
                    ///    Body = GeneratePasswordResetLink,
                    ///};
                    EmailManager.SendEmail("ResetPassword", User.Email, GeneratePasswordResetLink);
                    return new OkObjectResult($"Email Sent Successfully to your E-Mail, Please Check your Inbox");
                }
                else
                    ModelState.AddModelError(string.Empty, "User not found");
            }
            return View(forgetPasswordVM);
        }
        #endregion
        #region ResetPassword
        public IActionResult ResetPassword(string Email, string Token)
        {
            TempData["Email"] = Email;
            TempData["Token"] = Token;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel resetPasswordVM)
        {
            if (ModelState.IsValid)
            {
                string email = TempData["Email"] as string;
                string token = TempData["Token"] as string;
                var user = await userManager.FindByEmailAsync(email);
                if (user != null)
                {
                    var result = await userManager.ResetPasswordAsync(user, token, resetPasswordVM.NewPassword);
                    if (result.Succeeded)
                        return RedirectToAction(nameof(SignIn));
                    else
                        foreach (var error in result.Errors)
                            ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(resetPasswordVM);
        }
        #endregion
        private async Task<ActionResult<bool>> IsExist(string email)
        {
            return await userManager.FindByEmailAsync(email) is not null;//exist True
        }
    }
}
