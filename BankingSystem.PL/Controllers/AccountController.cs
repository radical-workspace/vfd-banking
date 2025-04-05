using AutoMapper;
using BankingSystem.DAL.Models;
using BankingSystem.PL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BankingSystem.PL.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;

        public AccountController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            IMapper mapper)
            
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
          _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> Register()
        {
            var roles = await _roleManager.Roles
            .Select(r => new SelectListItem { Value = r.Name, Text = r.Name }).ToListAsync();

            var Model = new RegisterViewModel
            {
                AvailableRoles = roles
            };
            return View(Model);
         
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel UserToRegister)
        {
            var roles = await _roleManager.Roles
         .Select(r => new SelectListItem { Value = r.Name, Text = r.Name }).ToListAsync();

            var Model = new RegisterViewModel
            {
                AvailableRoles = roles
            };
            if (UserToRegister is not null)
            {
                if(ModelState.IsValid)
                {
                    ApplicationUser appUser = _mapper.Map<RegisterViewModel,ApplicationUser>(UserToRegister);
                   
                    IdentityResult result = await _userManager.CreateAsync(appUser, UserToRegister.Password);

                    if (result.Succeeded)
                    {
                        //Create Cookie
                        await _userManager.AddToRoleAsync(appUser, UserToRegister.Role);
                        //await _signInManager.SignInAsync(appUser, false);
                        return RedirectToAction("Index", "Home");
                    }

                    else
                    {
                        foreach (var item in result.Errors)
                        {
                            ModelState.AddModelError("", item.Description);
                        }
                    }

                }
                }
            return View(UserToRegister);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginUserViewModel UserToLogin)
        {
            if (UserToLogin is not null)
            {


                if (ModelState.IsValid)
                {
                    // Check User Is Found Or Not
                    ApplicationUser user = await _userManager.FindByEmailAsync(UserToLogin.Email);

                    if (user is not null)
                    {
                        bool found = await _userManager.CheckPasswordAsync(user, UserToLogin.Password);
                        if (found)
                        {
                            await _signInManager.SignInAsync(user, UserToLogin.RememberMe);
                            return RedirectToAction("Index", "Home");
                        }
                    }
                }
                ModelState.AddModelError("", "Wrong Email Or Password");
                return View(UserToLogin);
            }

            return View(UserToLogin);
        }
    }
}
