using BLL.AdminService;
using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using moongroup.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace moongroup.Controllers
{
    public class HomeController : Controller
    {
        //Init ASP.NET identity store to handle user sign-in & sign-up 
        private readonly IAdminService admin;

        public HomeController(IAdminService adminService)
        {
            admin = adminService;
        }

        public IActionResult Login()
        {

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel LoginUser, string returnUrl)
        {
            var response = await admin.Login(LoginUser);
            if (response.Code == 200)
            {
                if (returnUrl != null)
                {
                    return Redirect(returnUrl);
                }
                else
                {
                    return RedirectToAction("Index", "Home", new { area = "Client" });
                }
            }
            else
            {
                ModelState.AddModelError("", response.ShortMessage);
                return View(LoginUser);
            }
        }

        public async Task<IActionResult> Logout()
        {
            var logout = await admin.Logout();
            if (logout.Code == 200)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        [Route("/Invite/{companyId}")]
        public IActionResult Register(string companyId)
        {
            var get = admin.ShowApplicationsForCompany(companyId);
            if(get.Code == 200)
            {
                List<ApplicaitionsVms> s = (List<ApplicaitionsVms>)get.Result;
                ViewBag.Applications = new SelectList(s.ToList(), "Id", "Name");
                return View();
            }
            return View("Error");
        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterUser user, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var response = await admin.SignUpUser(user);
                if (response.Code == 200)
                {
                    if (returnUrl != null)
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home", new { area = "Client" });
                    }
                }
                else
                {
                    ViewBag.Applications = new SelectList(admin.ShowApplications().ToList(), "Id", "Name");
                    ModelState.AddModelError("", response.ShortMessage);
                }
            }
            var message  = string.Join(" | ", ModelState.Values
                                   .SelectMany(v => v.Errors)
                                   .Select(e => e.ErrorMessage));
            ViewBag.Applications = new SelectList(admin.ShowApplications().ToList(), "Id", "Name");
            ModelState.AddModelError("", message);
            return View(user);

        }

        public IActionResult ForgotPassword()
        {
            return View();
        }
    }
}
