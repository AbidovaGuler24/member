using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApplication7.DAL;
using WebApplication7.Models;
using WebApplication7.Utils.Enums;
using WebApplication7.ViewModel.Account;

namespace WebApplication7.Controllers
{
    public class AccountController : Controller
    {
      UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(
            UserManager<AppUser> userManager, 
            SignInManager<AppUser> signInManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public IActionResult Register()
            {
                return View();
            }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVm registerVm) 
        {
            if (!ModelState.IsValid) 
            {
                return View();
            }
            AppUser appUser = new AppUser()
            {
                Name = registerVm.Name,
                Email = registerVm.Email,
                Surname = registerVm.Surname,
                UserName = registerVm.UserName,

            };
           var result = await _userManager.CreateAsync(appUser, registerVm.Password);
            if (!result.Succeeded) 
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
                return View();
            }
          //await  _userManager.AddClaimAsync(User, UserRoles.Admin.ToString());
            return RedirectToAction("Login");
        }

        public async Task<IActionResult> LogOut()
        {
           await _signInManager.SignOutAsync();
            return RedirectToAction("Index","Home");
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVm loginVm,string? ReturnUrl)
        {
            if (!ModelState.IsValid)

            {  return View(); }

            AppUser user = await _userManager.FindByEmailAsync(loginVm.EmailOrUsername)
            ??  await _userManager.FindByNameAsync(loginVm.EmailOrUsername);
            if (user == null)
            {
                ModelState.AddModelError("", "EmailOrUsername ve ya password sehvdir");
                return View();
            }
            var result =await _signInManager.CheckPasswordSignInAsync(user, loginVm.Password,true);
            if (!result.IsLockedOut) 
             {
                ModelState.AddModelError("", "yeniden sinayin");
                return View();
            }
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "yeniden sinayin");
                return View();

            }
            await _signInManager.SignInAsync(user, loginVm.Remember);

            if (ReturnUrl != null)
            {
                return RedirectToAction(ReturnUrl);
            }
            return RedirectToAction("Index","Home");

        }
        public async  Task<IActionResult> CreateRole()
        {
            await _roleManager.CreateAsync(new IdentityRole()
            {
                Name="Admin"

            });
            await _roleManager.CreateAsync(new IdentityRole()
            {
                Name = "Member"

            });
            
            return RedirectToAction("Index", "Home");

        }
    }
}
