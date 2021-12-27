using BLL.AdminService;
using DAL;
using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace moongroup.Areas.Client.Controllers
{
    [Area("Client")]
    [Authorize(Roles ="User")]
    //[Authorize(Roles = "Client", AuthenticationSchemes = "UserAuth")]
    public class HomeController : CommonController
    {
        //Init ASP.NET identity store to handle user sign-in & sign-up 
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IAdminService admin;

        public HomeController(IAdminService adminService, UserManager<ApplicationUser> userManager, IHostingEnvironment hosting) : base(hosting)
        {
            admin = adminService;
            this.userManager = userManager;
        }

        // POST: /Manage/ChangePassword
        [HttpPost]
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

        public async Task<IActionResult> Logout()
        {
            var logout = await admin.Logout();
            if (logout.Code == 200)
            {
                return RedirectToAction("Login", "Home",new { area=""});
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public IActionResult Index()
        {
            var projects = admin.UserProjects(userManager.GetUserId(HttpContext.User));
            return View(projects);
        }

        public IActionResult AddProject()
        {
            ViewBag.Applications = new SelectList(admin.ShowApplications().ToList(), "Id", "Name");
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> AddProject(AddNewProjectVms modal)
        {
            modal.UserId = userManager.GetUserId(HttpContext.User);
            var res = admin.AddProject(modal);
            return Json(res);
        }

        public IActionResult FormSection(int Id)
        {
            ViewBag.SettingId = Id;
            var applicationForms = admin.GetProjectSettingForm(Id);
            return View(applicationForms);
        }

        public IActionResult FormEdit(int Id,int SettingId)
        {
            ViewBag.Id = Id;
            ViewBag.SettingId = SettingId;
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> SaveFormSection([FromBody] List<SaveFormSection> vs, int Id, int SettingId)
        {
            var saveFormInfo = admin.SaveFormInfo(vs,Id,SettingId);
            return Json(saveFormInfo);
        }

        public async Task<JsonResult> GetClientSectionDetail(int Id,int SettingId)
        {
            var fields = admin.GetClientSectionDetail(Id, SettingId);
            return Json(fields);
        }

        public async Task<JsonResult> GetClientSectionDetailWithAnswer(int Id, int SettingId)
        {
            var fields = admin.GetClientSectionDetailWithAnswer(Id, SettingId);
            return Json(fields);
        }

        public IActionResult UploadSection(int Id)
        {
            var getDocuments = admin.GetProjectsDocuments(Id);
            if(getDocuments.Code == 200)
            {
                var s = (DocumentsVms)getDocuments.Result;
                return View(s);
            }
            else
            {
                return View("Error");
            }
        }

        [HttpPost]
        public async Task<JsonResult> UploadDocuments(UploadDocuments modal)
        {
            try
            {
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot/Images");
                if (modal.files != null && modal.files.Count() > 0)
                {
                    List<AddListofDocumentsVms> vs = new List<AddListofDocumentsVms>();
                    foreach (var i in modal.files)
                    {
                        if (i != null)
                        {
                            if (i.Length > 0)
                            {
                                var fileName = ContentDispositionHeaderValue.Parse(i.ContentDisposition).FileName.Trim('"');
                                Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                                var doc_id = unixTimestamp.ToString();
                                fileName =  doc_id.ToString()  + "_" + fileName;
                                var fullPath = Path.Combine(pathToSave, fileName);
                                var dbPath = "/Images/" + fileName;
                                using (var stream = new FileStream(fullPath, FileMode.Create))
                                {
                                    await i.CopyToAsync(stream);
                                }
                                AddListofDocumentsVms addListofDocumentsVms = new AddListofDocumentsVms
                                {
                                    FileSize = i.Length.ToString(),
                                    Link = dbPath,
                                    Name = fileName,
                                };
                                vs.Add(addListofDocumentsVms);
                            }
                        }
                    }
                    modal.ListImages = vs;
                    modal.UserId = userManager.GetUserId(HttpContext.User);
                    var s = admin.UploadProjectDocuments(modal);
                    return Json(s);
                }
                var res = JsonResponse2(400, "error", null);
                return Json(res);
            }
            catch(Exception ex)
            {
                var jsonresponse = JsonResponse2(504, ex.Message, null);
                return Json(jsonresponse);
            }
        }

        public async Task<JsonResult> DeleteDocument(int Id)
        {
            var res = admin.DeleteDocument(Id);
            var list = (List<string>)res.Result;
            foreach(var i in list)
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

        public IActionResult ResultSection(int Id)
        {
            var getDocuments = admin.GetProjectsDocuments(Id);
            if (getDocuments.Code == 200)
            {
                var s = (DocumentsVms)getDocuments.Result;
                return View(s);
            }
            else
            {
                return View("Error");
            }
        }

        public IActionResult Settings()
        {
            return View();
        }
        
    }
}
