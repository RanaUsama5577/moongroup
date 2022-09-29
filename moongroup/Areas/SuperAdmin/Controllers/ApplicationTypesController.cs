using BLL.AdminService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace moongroup.Areas.SuperAdmin.Controllers
{
    [Area("SuperAdmin")]
    [Authorize(Roles = "SuperAdmin")]
    public class ApplicationTypesController : Controller
    {
        private readonly IAdminService admin;

        public ApplicationTypesController(IAdminService adminService)
        {
            admin = adminService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CreateApps(string name)
        {
            var s = admin.CreateApplications(name);
            return Json(s);
        }

        public IActionResult DeleteApplication(int Id)
        {
            var s = admin.DeleteApplication(Id);
            return Json(s);
        }
    }
}
