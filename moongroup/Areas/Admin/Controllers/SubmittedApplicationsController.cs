using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
using Entities;
using System.IO;
using System.Net.Http.Headers;

namespace moongroup.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class SubmittedApplicationsController : CommonController
    {
        //Init ASP.NET identity store to handle user sign-in & sign-up
        private readonly UserManager<ApplicationUser> userManager;
        //Init ASP.NET identity store to handle user sign-in & sign-up 
        private readonly IAdminService admin;
        public SubmittedApplicationsController(IAdminService adminService, UserManager<ApplicationUser> userManager, IHostingEnvironment hosting) : base(hosting)
        {
            admin = adminService;
            this.userManager = userManager;
        }

        public IActionResult Index()
        {
            var getApplications = admin.GetUserProjects(userManager.GetUserId(HttpContext.User));
            return View(getApplications);
        }


        public IActionResult MarkInProgress(int Id)
        {
            try
            {
                var s = admin.MarkInProgress(Id);
                return Json(s);
            }
            catch (Exception ex)
            {
                throw new ValidationException(ex.GetBaseException().Message);
            }
        }

        public IActionResult CompleteProject(int Id)
        {
            try
            {
                var s = admin.CompleteProject(Id);
                return Json(s);
            }
            catch (Exception ex)
            {
                throw new ValidationException(ex.GetBaseException().Message);
            }
        }

        public IActionResult RejectProject(int Id)
        {
            try
            {
                var s = admin.RejectProject(Id);
                return Json(s);
            }
            catch (Exception ex)
            {
                throw new ValidationException(ex.GetBaseException().Message);
            }
        }

        public IActionResult Answers(int Id)
        {
            var s = admin.GetAnswersOfProjects(Id);
            return View(s);
        }

        public IActionResult ProjectDocuments(int Id)
        {
            var s = admin.GetAdminProjectsDocuments(Id, Entities.Enum.ApplicationTypes.Uploader);
            var a = (DocumentsVms)s.Result;
            return View(a);
        }

        public IActionResult ProjectInvoices(int Id)
        {
            var s = admin.GetAdminProjectsDocuments(Id,Entities.Enum.ApplicationTypes.Viewer);
            var a = (DocumentsVms)s.Result;
            return View(a);
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
                                fileName = doc_id.ToString() + "_" + fileName;
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
                    var s = admin.AdminUploadProjectDocuments(modal);
                    return Json(s);
                }
                var res = JsonResponse2(400, "error", null);
                return Json(res);
            }
            catch (Exception ex)
            {
                var jsonresponse = JsonResponse2(504, ex.Message, null);
                return Json(jsonresponse);
            }
        }
    }
}
