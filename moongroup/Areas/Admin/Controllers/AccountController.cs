using BLL.AdminService;
using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using moongroup.Models;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;

namespace moongroup.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AccountController : Controller
    {
        private readonly IAdminService admin;
        private readonly HttpContext _context;
        private readonly ICompositeViewEngine _viewEngine;
        private readonly ITempDataProvider _tempDataProvider;
        public AccountController(IAdminService adminService, ITempDataProvider tempDataProvider, ICompositeViewEngine viewEngine, IHttpContextAccessor accessor)
        {
            admin = adminService;
            _context = accessor.HttpContext;
            _viewEngine = viewEngine;
            _tempDataProvider = tempDataProvider;
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
                    return RedirectToAction("Index", "Dashboard");
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
                return RedirectToAction("Index", "Dashboard");
            }
        }

        public IActionResult ForgotPassword(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            if (TempData["Message"] != null)
            {
                ViewBag.Message = "Success";
            }
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordVMS Forgot)
        {
            var response = await admin.ForgotPassword(Forgot);
            if (response.Code == 200)
            {
                ForgotPasswordLink link = new ForgotPasswordLink();
                var callbackUrl = "http://dedramoon.assortcloud.com/Home/ResetPassword?Email=" + Forgot.Email + "&Code=" + response.ShortMessage;
                link.Link = callbackUrl;
                var data = await RenderToString("PartialForgotPassword", link);
                var s = SendVerificationLinkEmail("ubf5577@gmail.com", data);
                response.ShortMessage = s;
                TempData["Message"] = "Success";
                return RedirectToAction("ForgotPassword", "Account");
            }
            else
            {
                ModelState.AddModelError("", response.ShortMessage);
                return View(Forgot);
            }
        }

        internal string SendVerificationLinkEmail(string email, string link)
        {
            var fromEmail = new MailAddress("usamabusiness7861@gmail.com", "Password Reset Link");
            var toEmail = new MailAddress(email);
            var password = "1Q_2w3e4r";
            SmtpClient smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail.Address, password),
            };
            var m = new MailMessage(fromEmail, toEmail)
            {

                Subject = "Email Check",
                Body = link,
                IsBodyHtml = true
            };


            try
            {
                smtp.Send(m);
                return "We have sent you a email please check";
            }
            catch (Exception e)
            {
                var dd = "Message not sent retry" + e.Message;
                return dd;
            }

        }

        public async Task<string> RenderToString(string viewName, object model)
        {
            var controller = string.Empty;
            viewName = viewName?.TrimStart(new char[] { '/' });
            Regex rex = new Regex(@"^(\w+)\/(.*)$");
            Match match = rex.Match(viewName);
            if (match.Success)
            {
                controller = match.Groups[1].Value;
                viewName = match.Groups[2].Value;
            }

            var routeData = new RouteData();
            routeData.Values.Add("Api", controller);
            var actionContext = new ActionContext(_context, routeData, new ActionDescriptor());

            var viewResult = _viewEngine.FindView(actionContext, viewName, false);

            if (viewResult.View == null)
            {
                Console.WriteLine($"Searched the following locations: {string.Join(", ", viewResult.SearchedLocations)} for folder \"{controller}\" and view \"{viewName}\"");
                throw new ArgumentNullException($"{viewName} does not match any available view");
            }

            var viewDictionary = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary())
            {
                Model = model
            };

            using (var sw = new StringWriter())
            {
                var viewContext = new ViewContext(
                    actionContext,
                    viewResult.View,
                    viewDictionary,
                    new TempDataDictionary(_context, _tempDataProvider),
                    sw,
                    new HtmlHelperOptions()
                );
                viewContext.RouteData = _context.GetRouteData();

                await viewResult.View.RenderAsync(viewContext);
                return sw.ToString();
            }
        }
    }
}
