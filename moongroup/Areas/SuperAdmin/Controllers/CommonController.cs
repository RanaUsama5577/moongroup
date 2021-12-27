using Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace moongroup.Areas.SuperAdmin.Controllers
{
    public class CommonController : Controller
    {
        internal DateTime currentTime = DateTime.Now;
        internal string currentTimeString = DateTime.Now.ToLongDateString();
        internal IHostingEnvironment environment;

        public CommonController(IHostingEnvironment hostingEnvironment)
        {
            environment = hostingEnvironment;
        }

        internal JsonResult JsonResponse(int errorCode, string message, object responseData)
        {
            ResponseDto response = new ResponseDto
            {
                Code = errorCode,
                ShortMessage = message,
                Result = responseData,
            };
            return Json(response);
        }

        internal ResponseDto JsonResponse2(int errorCode, string message, object responseData)
        {
            ResponseDto response = new ResponseDto
            {
                Code = errorCode,
                ShortMessage = message,
                Result = responseData,
            };
            return response;
        }

        internal string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        [HttpPost]
        internal JsonResult UploadImage(IFormFile imageFile)
        {
            if (imageFile == null)
            {
                return JsonResponse(404, "Image not found!", null);
            }
            try
            {
                string ImagePath = "/Images/";
                string uploadsFolder = Path.Combine(environment.WebRootPath, ImagePath + imageFile.FileName);
                var uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
                var path = Path.Combine(
                            uploadsFolder, uniqueFileName);
                imageFile.CopyTo(new FileStream(path, FileMode.Create));
                return JsonResponse(200, "successfully uploaded", uniqueFileName);
            }
            catch (Exception ex)
            {
                var BaseError = ex.GetBaseException();
                return JsonResponse(501, BaseError.Message, null);
            }
        }

        internal string UploadFile(IFormFile imageFile)
        {
            if (imageFile == null)
            {
                return null;
            }
            try
            {
                string uniqueFileName = null;
                if (imageFile != null)
                {
                    string uploadsFolder = Path.Combine(environment.WebRootPath, "Images");
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        imageFile.CopyTo(fileStream);
                    }
                }
                return "/Images/" + uniqueFileName;
            }
            catch (Exception ex)
            {
                var BaseError = ex.GetBaseException();
                return "Not Uploaded";
            }
        }

        internal List<string> AddErrors(IdentityResult result)
        {
            List<string> vs = new List<string>();
            foreach (var error in result.Errors)
            {
                vs.Add(error.ToString());
            }
            return vs;
        }

        internal async Task<bool> SendMessageToClient(string Fcm, string body, string title)
        {
            string content = string.Empty;
            HttpClient httpClient = null;
            HttpResponseMessage response = null;
            try
            {
                // Get this from your Firebase Developer Console Login  
                string serverApiKey = "AAAAQQsuXuk:APA91bHbZ0KVFVXPczsVgCn0F6Xk2MAY-hAtKLM6Edf8-gTVnOPaSgpkU09XV8GWyHAYHb-voPZTRSlKv95vydJn3wwX-RVt-i0A6fVlVjTYVkhnR7jWcF3rP_Ba9MZpP6Q2dQWEkCX9";
                string apiEndpoint = "https://fcm.googleapis.com/fcm/send";
                using (httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", (string.Format("key={0}", serverApiKey)));
                    httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
                    var dynamicPostData = new
                    {
                        to = Fcm,
                        notification = new
                        {
                            sound = "default",
                            body = body,
                            title = title,
                            content_available = true,
                            priority = "high"
                        },
                    };
                    //response = httpClient.PostAsJsonAsync(new Uri(apiEndpoint), dynamicPostData).Result;

                    content = await response.Content.ReadAsStringAsync();
                    response.EnsureSuccessStatusCode();

                    //FCMGroupResponse resp = JsonConvert.DeserializeObject<FCMGroupResponse>(content);
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
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
    }
}
