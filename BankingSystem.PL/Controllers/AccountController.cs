using AutoMapper;
using BankingSystem.BLL.Interfaces;
using BankingSystem.DAL.Models;
using BankingSystem.PL.ViewModels.Auth;
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
        private readonly IUniitOfWork _uniitOfWork;

        public AccountController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            IMapper mapper,IUniitOfWork uniitOfWork)
            
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
          _mapper = mapper;
            _uniitOfWork = uniitOfWork;
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
        //public async Task<IActionResult> Register(RegisterViewModel UserToRegister)
        //{
        //    var roles = await _roleManager.Roles
        // .Select(r => new SelectListItem { Value = r.Name, Text = r.Name }).ToListAsync();

        //    var Model = new RegisterViewModel
        //    {
        //        AvailableRoles = roles
        //    };
        //    if (UserToRegister is not null)
        //    {
        //        if(ModelState.IsValid)
        //        {
        //            if(UserToRegister.Role =="Customer")
        //            {
        //                Customer appUser = _mapper.Map<RegisterViewModel, ApplicationUser>(UserToRegister);

        //            }

        //            //if (appUser.Discriminator == "Customer")
        //            //{
        //            //    var Customer = new Customer()
        //            //    {
        //            //        Id = appUser.Id
        //            //    };
        //            //    _uniitOfWork.Repository<Customer>().Add(Customer);
        //            //}

        //            IdentityResult result = await _userManager.CreateAsync(appUser, UserToRegister.Password);

        //            if (result.Succeeded)
        //            {
        //                //Create Cookie
        //                await _userManager.AddToRoleAsync(appUser, UserToRegister.Role);
        //                //await _signInManager.SignInAsync(appUser, false);
        //                return RedirectToAction("Index", "Home");
        //            }

        //            else
        //            {
        //                foreach (var item in result.Errors)
        //                {
        //                    ModelState.AddModelError("", item.Description);
        //                }
        //            }

        //        }
        //        }
        //    return View(UserToRegister);
        //}
        public async Task<IActionResult> Register(RegisterViewModel UserToRegister)
        {
            // Load roles again in case of return to the view
            var roles = await _roleManager.Roles
                .Select(r => new SelectListItem { Value = r.Name, Text = r.Name })
                .ToListAsync();

            if (UserToRegister is not null)
            {
                if (ModelState.IsValid)
                {
                    ApplicationUser appUser;

                    // Create the correct derived class based on role
                    if (UserToRegister.Role == "Customer")
                    {
                        appUser = _mapper.Map<Customer>(UserToRegister);
                    }
                    else if (UserToRegister.Role == "Admin")
                    {
                        appUser = _mapper.Map<Admin>(UserToRegister);
                    }  
                    else if (UserToRegister.Role == "Manager")
                    {
                        appUser = _mapper.Map<Manager>(UserToRegister);
                    } 
                    else if (UserToRegister.Role == "Teller")
                    {
                        appUser = _mapper.Map<Teller>(UserToRegister);
                    }

                    else
                    {
                        appUser = _mapper.Map<ApplicationUser>(UserToRegister);
                    }

                    IdentityResult result = await _userManager.CreateAsync(appUser, UserToRegister.Password);

                    if (result.Succeeded)
                    {
                        // Assign role
                        await _userManager.AddToRoleAsync(appUser, UserToRegister.Role);

                        // Optional: Sign in
                        // await _signInManager.SignInAsync(appUser, false);

                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                    }
                }
            }

            // Return view with roles and model in case of error
            UserToRegister.AvailableRoles = roles;
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
                ModelState.AddModelError(string.Empty, "Wrong Email Or Password");
                return View(UserToLogin);
            }

            return View(UserToLogin);
        }
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    return RedirectToAction("ForgotPasswordConfirmation");
                }
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var resetLink = Url.Action("ResetPassword", "Account", new { token, email = user.Email }, Request.Scheme);

                Console.WriteLine(resetLink);

                return RedirectToAction("ForgotPasswordConfirmation");
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

    }
}
