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

namespace moongroup.Areas.Client.Controllers
{
    [Area("Client")]
    [Authorize(Roles = "User")]
    public class SettingsController : CommonController
    {

        //Init ASP.NET identity store to handle user sign-in & sign-up 
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IAdminService admin;

        public SettingsController(IAdminService adminService, UserManager<ApplicationUser> userManager, IHostingEnvironment hosting) : base(hosting)
        {
            admin = adminService;
            this.userManager = userManager;
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
            var user = admin.getLoginUser2(userManager.GetUserId(HttpContext.User));
            ProfileDtos profile = new ProfileDtos
            {
                FullName = user.FullName,
                Email = user.Email,
                ProfileImageUrl = user.ProfileImageUrl,
                companyImage = user.companyImage,
            };
            return JsonResponse(200, "success", profile);
        }
    }
}
