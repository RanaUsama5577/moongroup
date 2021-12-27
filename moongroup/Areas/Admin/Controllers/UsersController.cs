using BLL.AdminService;
using DAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace moongroup.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class UsersController : CommonController
    {

        //Init ASP.NET identity store to handle user sign-in & sign-up
        private readonly UserManager<ApplicationUser> userManager;
        //Init ASP.NET identity store to handle user sign-in & sign-up 
        private readonly IAdminService admin;
        public UsersController(IAdminService adminService, UserManager<ApplicationUser> userManager, IHostingEnvironment hosting) : base(hosting)
        {
            admin = adminService;
            this.userManager = userManager;
        }
        public IActionResult Index()
        {
            var users = admin.GetAppUsers(userManager.GetUserId(HttpContext.User));
            return View(users);
        }

        public IActionResult BlockUser(string Id)
        {
            try
            {
                var s = admin.BlockUser(Id);
                return Json(s);
            }
            catch (Exception ex)
            {
                throw new ValidationException(ex.GetBaseException().Message);
            }
        }

        public IActionResult UnBlockUser(string Id)
        {
            try
            {
                var s = admin.UnBlockUser(Id);
                return Json(s);
            }
            catch (Exception ex)
            {
                throw new ValidationException(ex.GetBaseException().Message);
            }
        }
    }
}
