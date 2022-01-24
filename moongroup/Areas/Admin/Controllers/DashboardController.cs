using BLL.AdminService;
using DAL;
using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace moongroup.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class DashboardController : CommonController
    {
        //Init ASP.NET identity store to handle user sign-in & sign-up 
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IAdminService admin;

        public DashboardController(IAdminService adminService, UserManager<ApplicationUser> userManager, IHostingEnvironment hosting) : base(hosting)
        {
            admin = adminService;
            this.userManager = userManager;
        }


        public IActionResult Index()
        {
            var s = admin.AdminDashboardStats(userManager.GetUserId(HttpContext.User));
            return View(s);
        }

        [Microsoft.AspNetCore.Mvc.HttpPost]
        public async Task<IActionResult> UpdateProfileImage(UpdateProfileImage modal)
        {
            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot/Images");
            if (modal.file.Length > 0)
            {
                var fileName = ContentDispositionHeaderValue.Parse(modal.file.ContentDisposition).FileName.Trim('"');
                var fullPath = Path.Combine(pathToSave, fileName);
                var dbPath = "/Images/" + fileName;
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await modal.file.CopyToAsync(stream);
                }
                string imageUrl = dbPath;
                var user = admin.UpdateProfileImage(imageUrl, userManager.GetUserId(HttpContext.User));
                ProfileDtos profile = new ProfileDtos
                {
                    FullName = user.FullName,
                    Email = user.Email,
                    ProfileImageUrl = user.ProfileImageUrl,
                };
                return JsonResponse(200, "success", profile);
            }
            else
            {
                return JsonResponse(400, "error", null);
            }

        }

        public IActionResult UpdateProfile(string Name)
        {
            var user = admin.UpdateProfile(Name, userManager.GetUserId(HttpContext.User));
            ProfileDtos profile = new ProfileDtos
            {
                FullName = user.FullName,
                Email = user.Email,
                ProfileImageUrl = user.ProfileImageUrl,
            };
            return JsonResponse(200, "success", profile);
        }

        // POST: /Manage/ChangePassword
        [Microsoft.AspNetCore.Mvc.HttpPost]
        public async Task<JsonResult> ChangePassword(UpdatePasswordVms model)
        {
            try
            {
                var response = await admin.ChangePassword(model, userManager.GetUserId(HttpContext.User));
                return Json(response);
            }
            catch (Exception ex)
            {
                return JsonResponse(501, ex.GetBaseException().Message, null);
            }
        }

        public IActionResult GetProfile()
        {
            var user = admin.getLoginUser(userManager.GetUserId(HttpContext.User));
            ProfileDtos profile = new ProfileDtos
            {
                FullName = user.FullName,
                Email = user.Email,
                ProfileImageUrl = user.ProfileImageUrl,
            };
            return JsonResponse(200, "success", profile);
        }

        public async Task<JsonResult> DeleteDocument(int Id)
        {
            var res = admin.DeleteDocument(Id);
            var list = (List<string>)res.Result;
            foreach (var i in list)
            {
                var s = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot/Images/" + i);
                FileInfo file = new FileInfo(s);
                if (file.Exists)
                {
                    file.Delete();
                }
                else
                {

                }
            }
            return Json(res);
        }

        public IActionResult MyProfile()
        {
            return View();
        }
    }
}
