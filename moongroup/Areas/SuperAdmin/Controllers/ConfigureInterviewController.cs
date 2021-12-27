using BLL.AdminService;
using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace moongroup.Areas.SuperAdmin.Controllers
{
    [Area("SuperAdmin")]
    [Authorize(Roles = "SuperAdmin")]
    public class ConfigureInterviewController : Controller
    {
        private readonly IAdminService admin;

        public ConfigureInterviewController(IAdminService adminService)
        {
            admin = adminService;
        }

        public IActionResult Index()
        {
            var list = admin.ShowApplications();
            return View(list);
        }

        public IActionResult CreateForms(int Id)
        {
            ViewBag.Id = Id;
            return View();
        }
        
        public async Task<JsonResult> AppendForms(int Id)
        {
            var list = admin.ShowForms(Id);
            return Json(list);
        }

        [HttpPost]
        public async Task<JsonResult> AddSection([FromForm] AddSectionVms modal)
        {
            var reponse = admin.AddSection(modal);
            return Json(reponse);
        }

        public async Task<JsonResult> DeleteSection(int Id)
        {
            var delete = admin.DeleteSection(Id);
            return Json(delete);
        }

        [HttpPost]
        public async Task<JsonResult> AddField([FromForm] AddFieldVms modal)
        {
            var reponse = admin.AddFields(modal);
            return Json(reponse);
        }

        public async Task<JsonResult> DeleteField(int Id)
        {
            var delete = admin.DeleteField(Id);
            return Json(delete);
        }

        public async Task<JsonResult> GetFieldValues(int Id)
        {
            var field = admin.GetFormsFieldOptions(Id);
            return Json(field);
        }

        [HttpPost]
        public async Task<JsonResult> AddOptionField([FromForm] AddOptionOfFieldVms modal)
        {
            var reponse = admin.AddOptionOfFieldVms(modal);
            return Json(reponse);
        }

        public async Task<JsonResult> DeleteOptionField(int Id)
        {
            var delete = admin.DeleteOption(Id);
            return Json(delete);
        }
    }
}
