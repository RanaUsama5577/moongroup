using DAL;
using DAL.Applications;
using DAL.Projects;
using Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using static Entities.Enum;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace BLL.AdminService
{
    public class AdminService : commonclass, IAdminService
    {
        private readonly ApplicationDbContext db;
        //Init ASP.NET identity store to handle user sign-in & sign-up 
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public IConfiguration Configuration { get; }

        public AdminService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager2, SignInManager<ApplicationUser> signInManager,
            ApplicationDbContext database, IHostingEnvironment hostingEnvironment, IConfiguration configuration) : base(hostingEnvironment)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.roleManager = roleManager2;
            db = database;
            Configuration = configuration;
        }

        async Task<ResponseDto> IAdminService.SignUpUser(RegisterUser SignUpUser)
        {
            if (!ModelState.IsValid)
            {
                var message = string.Join(" | ", ModelState.Values
                                .SelectMany(v => v.Errors)
                                .Select(e => e.ErrorMessage));

                return JsonResponse2(400, message, null);
            }
            try
            {
                var EmailRegistered = db.Users.Where(u => u.Email == SignUpUser.Email).FirstOrDefault();
                if (EmailRegistered != null)
                {
                    return JsonResponse2(400, "Email already registered!", null);
                }
                var applicaiton = db.Applications.Find(SignUpUser.ApplicationId);
                if (applicaiton == null)
                {
                    return JsonResponse2(400, "application not found", null);
                }
                var imageUrl = "";
                ApplicationUser newUser = new ApplicationUser
                {
                    Email = SignUpUser.Email,
                    UserName = SignUpUser.Email,
                    RegisteredAt = currentTime,
                    FullName = SignUpUser.FirstName + " " + SignUpUser.LastName,
                    FirstName  =SignUpUser.FirstName,
                    LastName  =SignUpUser.LastName,
                    MobileNumber  =SignUpUser.MobileNumber,
                    WorkNumber  =SignUpUser.WorkNumber,
                    PhoneNumber  =SignUpUser.PhoneNumber,
                    UserType  = UserType.User,
                    ExternalType = LoginProvider.WithEmail,
                    Status = EntityStatus.Active,
                    Type = UserType.User,
                    ProfileImageUrl = imageUrl,
                    CompanyId = "8e514976-2bdd-46e9-a263-613f4a227bea",
                    CreatedAt  =currentTime,
                    UpdatedAt = currentTime,
                };
                var result = await userManager.CreateAsync(newUser, SignUpUser.Password);
                if (result.Succeeded)
                {
                    if (await roleManager.RoleExistsAsync("User"))
                    {
                        await userManager.AddToRoleAsync(newUser, "User");
                    }
                    else
                    {
                        IdentityResult identityResult = await roleManager.CreateAsync(new IdentityRole("User"));
                        if (identityResult.Succeeded)
                        {
                            await userManager.AddToRoleAsync(newUser, "User");
                        }
                    }
                    UserProjects userProjects = new UserProjects
                    {
                        Status = ApplicationStatus.Pending,
                        Application = SignUpUser.ApplicationId,
                        CreatedAt = currentTime,
                        UpdatedAt = currentTime,
                        UserId = newUser.Id,
                        Year = SignUpUser.Year,
                    };
                    db.UserProjects.Add(userProjects);

                    var applicationSetting = db.ApplicationSettings.Where(p => p.ApplicationId == applicaiton.Id);
                    foreach (var i in applicationSetting)
                    {
                        ProjectsSettings projectsSettings = new ProjectsSettings
                        {
                            SettingName = i.Name,
                            Status = ProjectSettingStatus.InProgress,
                            CreatedAt = currentTime,
                            ProjectObject = userProjects,
                            Type = i.ApplicationType,
                            UpdatedAt = currentTime,
                        };
                        db.ProjectsSettings.Add(projectsSettings);
                    }
                    db.SaveChanges();

                    return JsonResponse2(200, "success", null);
                }
                else
                {
                    List<string> errorList = new List<string>();
                    foreach (var error in result.Errors)
                    {
                        errorList.Add(error.Description);
                    }
                    return JsonResponse2(401, "Validation Error", errorList);
                }
            }
            catch (Exception ex)
            {
                return JsonResponse2(504, ex.Message, null);
            }
        }


        List<ClientProfileDtos> IAdminService.GetUsers()
        {
            try
            {
                var users = db.Users.Where(p => p.Type == UserType.Admin).OrderByDescending(p => p.CreatedAt).AsEnumerable().Select(n => new ClientProfileDtos
                {
                    CreatedAt = n.CreatedAt.ToString("dd-MMM-yyyy hh:mm:ss tt"),
                    Email = n.Email,
                    FullName = n.FullName,
                    ProfileImageUrl = n.ProfileImageUrl,
                    Status = n.Status,
                    Id = n.Id,
                    CompanyName = n.CompanyName,
                    PhoneNumber = n.PhoneNumber,
                    ApplicationAssigned = db.ClientApplications.Where(p=>p.UserId == n.Id).Select(p=>p.ApplicationObject.Name).ToList(),
                });
                return users.ToList();
            }
            catch (Exception es)
            {
                throw new ValidationException(es.GetBaseException().Message);
            }
        }

        List<UsersProfileDtos> IAdminService.GetAppUsers(string Id)
        {
            try
            {
                var users = db.Users.Where(p => p.Type == UserType.User && p.CompanyId == Id).OrderByDescending(p => p.CreatedAt).AsEnumerable().Select(n => new UsersProfileDtos
                {
                    CreatedAt = n.CreatedAt.ToString("dd-MMM-yyyy hh:mm:ss tt"),
                    Email = n.Email,
                    FullName = n.FullName,
                    ProfileImageUrl = n.ProfileImageUrl,
                    Status = n.Status,
                    Id = n.Id,
                    PhoneNumber = n.PhoneNumber,
                    MobileNumber = n.MobileNumber,
                    WorkNumber = n.WorkNumber,
                });
                return users.ToList();
            }
            catch (Exception es)
            {
                throw new ValidationException(es.GetBaseException().Message);
            }
        }

        async public Task<ResponseDto> CreateAdmin()
        {
            ApplicationUser newUser = new ApplicationUser
            {
                Email = "admin@dedramoon.com",
                UserName = "admin@dedramoon.com",
                RegisteredAt = currentTime,
                FullName = "Dedra Admin",
                ExternalType = LoginProvider.WithEmail,
                Status = EntityStatus.Active,
                EmailConfirmed = true,
                Type = UserType.Admin,
                ProfileImageUrl = "/mon.jpg",
            };
            var result = await userManager.CreateAsync(newUser, "1q2w3e4r");
            if (result.Succeeded)
            {
                if(!await roleManager.RoleExistsAsync("Admin"))
                {
                    await userManager.AddToRoleAsync(newUser, "Admin");
                    return JsonResponse2(200,"success",null);
                }
                else
                {
                    IdentityResult identityResult = await roleManager.CreateAsync(new IdentityRole("Admin"));
                    if (identityResult.Succeeded)
                    {
                        await userManager.AddToRoleAsync(newUser, "Admin");
                    }
                    return JsonResponse2(400, "role doesn't exist", null);
                }
            }
            else
            {
                return JsonResponse2(400, "error", result);
            }
        }
        public SuperAdminDashboard SuperDashboardStats()
        {
            SuperAdminDashboard a = new SuperAdminDashboard
            {
                TotalUsers = db.Users.Where(p => p.Status == EntityStatus.Active && p.Type == UserType.Admin).Count(),
                ApplicationTypes = db.Applications.Where(p => p.Status == EntityStatus.Active).Count(),
            };
            return a;
        }

        public AdminDashboard AdminDashboardStats(string Id)
        {
            AdminDashboard a = new AdminDashboard
            {
                TotalUsers = db.Users.Where(p => p.Status == EntityStatus.Active && p.Type == UserType.User && p.CompanyId == Id).Count(),
                CanceledApps = db.UserProjects.Where(p=>p.Status == ApplicationStatus.Canceled).Count(),
                CompletedApps = db.UserProjects.Where(p => p.Status == ApplicationStatus.Completed).Count(),
                InprogressApps = db.UserProjects.Where(p => p.Status == ApplicationStatus.InProgress).Count(),
                PendingApps = db.UserProjects.Where(p => p.Status == ApplicationStatus.Pending).Count(),
            };
            return a;
        }

        public ApplicationUser getLoginUser(string Id)
        {
            if (Id != null)
            {
                ApplicationUser LoginUser = db.Users.Find(Id);
                return LoginUser;
            }
            return null;
        }

        public ProfileDtos getLoginUser2(string Id)
        {
            if (Id != null)
            {
                var LoginUser = db.Users.Find(Id);
                var company = db.Users.Find(LoginUser.CompanyId);
                ProfileDtos profileDtos = new ProfileDtos
                {
                    FullName = LoginUser.FullName,
                    Email = LoginUser.Email,
                    ProfileImageUrl = LoginUser.ProfileImageUrl,
                    companyImage = company.ProfileImageUrl,
                };
                return profileDtos;
            }
            return null;
        }
        async Task<ResponseDto> IAdminService.Login(LoginViewModel LoginUser)
        {
            try
            {
                if (LoginUser.Email == null)
                {
                    return JsonResponse2(400, "Email is required!", null);
                }
                //Check Validations
                if (!ModelState.IsValid)
                {
                    var message = string.Join(" | ", ModelState.Values
                                    .SelectMany(v => v.Errors)
                                    .Select(e => e.ErrorMessage));
                    return JsonResponse2(400, message, null);
                }
                var RequestedUser = db.Users.Where(usr => usr.Email == LoginUser.Email).FirstOrDefault();
                if (RequestedUser != null)
                {
                    var result = await signInManager.PasswordSignInAsync(RequestedUser.UserName, LoginUser.Password, false, lockoutOnFailure: false);
                    if (result.Succeeded)
                    {
                        var user = db.Users.Where(p => p.Email == LoginUser.Email).FirstOrDefault();

                        var ident = new ClaimsIdentity(new Claim[]
                        {
                            new Claim("Id", RequestedUser.Id)
                        });
                        if (user.Type == UserType.SuperAdmin)
                        {
                            return JsonResponse2(200, "success", null);
                        }
                        else if(user.Type == UserType.Admin)
                        {
                            return JsonResponse2(200, "success", null);
                        }
                        else if (user.Type == UserType.User)
                        {
                            return JsonResponse2(200, "success", null);
                        }
                        else
                        {
                            return JsonResponse2(400, "Not allowed to sign in admin panel", null);
                        }
                    }
                    else if(result.IsNotAllowed)
                    {
                        return JsonResponse2(400, "not allowed to sign in", null);
                    }
                    else if (result.IsLockedOut)
                    {
                        return JsonResponse2(400, "user locked out", null);
                    }
                    else
                    {
                        return JsonResponse2(400, "incorrect password", null);
                    }
                }
                else
                {
                    return JsonResponse2(400, "Invalid user email or username!", null);
                }
            }
            catch (Exception ex)
            {
                return JsonResponse2(501, ex.GetBaseException().Message, null);
            }
        }

        async Task<ResponseDto> IAdminService.ForgotPassword([FromForm] ForgotPasswordVMS model)
        {
            if (ModelState.IsValid)
            {
                var user = db.Users.Where(p => p.Email == model.Email).FirstOrDefault();
                if (user == null)
                {
                    return JsonResponse2(401, "User doesn't exists", null);
                }
                var token = await userManager.GeneratePasswordResetTokenAsync(user);
                token = HttpUtility.UrlEncode(token);
                return JsonResponse2(200, token, null);
            }
            // If we got this far, something failed, redisplay form
            return JsonResponse2(400, "error", null);
        }
        async Task<ResponseDto> IAdminService.ResetPassword([FromForm] ResetPasswordVMS model)
        {
            if (ModelState.IsValid)
            {
                var user = db.Users.Where(p => p.Email == model.Email).FirstOrDefault();
                if (user == null)
                {
                    return JsonResponse2(401, "User doesn't exists", null);
                }
                //var token = await userManager.GeneratePasswordResetTokenAsync(user);
                var result = await userManager.ResetPasswordAsync(user, model.Code, model.NewPassword);
                if (result.Succeeded)
                {
                    return JsonResponse2(200, "Success , Password Changed Successfully", null);
                }
                else
                {
                    List<string> errorList = new List<string>();
                    foreach (var error in result.Errors)
                    {
                        errorList.Add(error.Description);
                    }
                    return JsonResponse2(401, "Validation Error", errorList);
                }
            }

            // If we got this far, something failed, redisplay form
            return JsonResponse2(400, "error", null);
        }
        public ApplicationUser UpdateProfileImage(string ImageUrl, string Id)
        {
            ApplicationUser LoginUser = db.Users.Find(Id);
            LoginUser.ProfileImageUrl = ImageUrl;
            db.Entry(LoginUser).State = EntityState.Modified;
            db.SaveChanges();
            return LoginUser;
        }

        async Task<ResponseDto> IAdminService.Logout()
        {
            try
            {
                await signInManager.SignOutAsync();
                return JsonResponse2(200, "success", null);
            }
            catch (Exception ex)
            {
                return JsonResponse2(501, ex.GetBaseException().Message, null);
            }
        }

        public ApplicationUser UpdateProfile(string name, string Id)
        {
            ApplicationUser LoginUser = db.Users.Find(Id);
            LoginUser.FullName = name;
            db.Entry(LoginUser).State = EntityState.Modified;
            db.SaveChanges();
            return LoginUser;
        }

        async Task<ResponseDto> IAdminService.ChangePassword(UpdatePasswordVms model, string Id)
        {
            if (!ModelState.IsValid)
            {
                var message = string.Join(" | ", ModelState.Values
                                .SelectMany(v => v.Errors)
                                .Select(e => e.ErrorMessage));

                return JsonResponse2(400, message, null);
            }
            try
            {
                var loginUser = db.Users.Find(Id);
                var result = await userManager.ChangePasswordAsync(loginUser, model.current_password, model.new_password);
                if (result.Succeeded)
                {
                    return JsonResponse2(200, "Success , Password Changed Successfully", null);
                }
                else
                {
                    List<string> errorList = new List<string>();
                    foreach (var error in result.Errors)
                    {
                        errorList.Add(error.Description);
                    }
                    return JsonResponse2(401, "Validation Error", errorList);
                }
            }
            catch (Exception ex)
            {
                return JsonResponse2(501, ex.GetBaseException().Message, null);
            }
        }

        public ResponseDto BlockUser(string Id)
        {
            try
            {
                var entity = db.Users.Find(Id);
                if (entity == null)
                {
                    return JsonResponse2(400, "entity not found", null);
                }
                else
                {
                    entity.Status = EntityStatus.Blocked;
                    db.Entry(entity).State = EntityState.Modified;
                    db.SaveChanges();
                    var title = "You are blocked";
                    var message = "Due to some reasons we have blocked your account.";
                    try
                    {
                        if (entity.FcmToken != "" && entity.FcmToken != null)
                        {
                            var sendNotification = SendMessageToClient(entity.FcmToken, message, title);
                        }
                    }
                    catch
                    {

                    }
                    return JsonResponse2(200, "success", null);
                }
            }
            catch (Exception es)
            {
                return JsonResponse2(504, es.GetBaseException().Message, null);
            }
        }
        public ResponseDto UnBlockUser(string Id)
        {
            try
            {
                var entity = db.Users.Find(Id);
                if (entity == null)
                {
                    return JsonResponse2(400, "entity not found", null);
                }
                else
                {
                    entity.Status = EntityStatus.Active;
                    db.Entry(entity).State = EntityState.Modified;
                    db.SaveChanges();
                    var title = "You are unblocked";
                    var message = "Congratulations! Our admin team has decided to unblock your account. Enjoy reading book.";
                    try
                    {
                        if (entity.FcmToken != "" && entity.FcmToken != null)
                        {
                            var sendNotification = SendMessageToClient(entity.FcmToken, message, title);
                        }
                    }
                    catch
                    {

                    }
                    return JsonResponse2(200, "success", null);
                }
            }
            catch (Exception es)
            {
                return JsonResponse2(504, es.GetBaseException().Message, null);
            }
        }

        public ResponseDto CreateApplications()
        {
            try
            {
                var applicaiton = new Applications
                {
                    CreatedAt = currentTime,
                    Status = EntityStatus.Active,
                    Name = "TMG Client Application",
                    UpdatedAt = currentTime,
                };
                db.Applications.Add(applicaiton);
                db.SaveChanges();

                var applicationsetting = new ApplicationSetting
                {
                    ApplicationId = applicaiton.Id,
                    ApplicationObject = applicaiton,
                    ApplicationType = ApplicationTypes.Form,
                    CreatedAt = currentTime,
                    Name = "Interview",
                    UpdatedAt = currentTime,
                };
                var applicationsetting2 = new ApplicationSetting
                {
                    ApplicationId = applicaiton.Id,
                    ApplicationObject = applicaiton,
                    ApplicationType = ApplicationTypes.Uploader,
                    CreatedAt = currentTime,
                    Name = "Upload Documents",
                    UpdatedAt = currentTime,
                };
                var applicationsetting3 = new ApplicationSetting
                {
                    ApplicationId = applicaiton.Id,
                    ApplicationObject = applicaiton,
                    ApplicationType = ApplicationTypes.Viewer,
                    CreatedAt = currentTime,
                    Name = "Invoices",
                    UpdatedAt = currentTime,
                };
                db.ApplicationSettings.Add(applicationsetting);
                db.ApplicationSettings.Add(applicationsetting2);
                db.ApplicationSettings.Add(applicationsetting3);
                db.SaveChanges();
                return JsonResponse2(200, "success", null);
            }
            catch(Exception ex)
            {
                return JsonResponse2(400, "error", ex.Message);
            }
        }

        List<ApplicaitionsVms> IAdminService.ShowApplications()
        {
            try
            {
                var apps = db.Applications.Where(p => p.Status == EntityStatus.Active).Select(n => new ApplicaitionsVms
                {
                    CreatedAt = n.CreatedAt.ToString("dd-MMM-yyyy hh:mm:ss tt"),
                    UpdatedAt = n.UpdatedAt != null? n.UpdatedAt.Value.ToString("dd-MMM-yyyy hh:mm:ss tt"):null,
                    Id  =n.Id,
                    Name = n.Name,
                    Types = db.ApplicationSettings.Where(p=>p.ApplicationId == n.Id).Select(p=>p.Name).ToList(),
                });
                return apps.ToList();
            }
            catch (Exception es)
            {
                throw new ValidationException(es.GetBaseException().Message);
            }
        }

        ResponseDto IAdminService.ShowApplicationsForCompany(string Id)
        {
            try
            {
                var user = db.Users.Where(p => p.UserName == Id).FirstOrDefault();
                if(user == null)
                {
                    return JsonResponse2(400, "company not found",null);
                }
                var apps = db.ClientApplications.Where(p => p.ApplicationObject.Status == EntityStatus.Active && p.UserId == user.Id).Select(p=>p.ApplicationObject).Select(n => new ApplicaitionsVms
                {
                    CreatedAt = n.CreatedAt.ToString("dd-MMM-yyyy hh:mm:ss tt"),
                    UpdatedAt = n.UpdatedAt != null ? n.UpdatedAt.Value.ToString("dd-MMM-yyyy hh:mm:ss tt") : null,
                    Id = n.Id,
                    Name = n.Name,
                    Types = db.ApplicationSettings.Where(p => p.ApplicationId == n.Id).Select(p => p.Name).ToList(),
                });
                return JsonResponse2(200,"success", apps.ToList());
            }
            catch (Exception es)
            {
                throw new ValidationException(es.GetBaseException().Message);
            }
        }

        

        public List<FormsSectionsVms> ShowForms(int Id)
        {
            try
            {
                List<FormsSectionsVms> formsSections1 = new List<FormsSectionsVms>();
                var formsSections = db.FormSections.Where(p => p.ApplicationSettingObject.ApplicationId == Id).OrderBy(p=>p.Order);
                foreach(var p in formsSections)
                {
                    FormsSectionsVms  forms1 = new FormsSectionsVms
                    {
                        CreatedAt = p.CreatedAt.ToString("dd-MMM-yyyy hh:mm:ss tt"),
                        Id = p.Id,
                        Name = p.Name,
                        IsRequired = p.IsRequired == true ?"Required" : "Not Required",
                        UpdatedAt = p.UpdatedAt != null ? p.UpdatedAt.Value.ToString("dd-MMM-yyyy hh:mm:ss tt") : null,
                        SectionFields = GetFormsFields(p.Id,1),
                    };
                    formsSections1.Add(forms1);
                }
                return formsSections1;
            }
            catch (Exception es)
            {
                throw new ValidationException(es.GetBaseException().Message);
            }
        }

        public List<FormsSectionFieldsVms> GetFormsFields(int sectionId,int type)
        {
            List<FormsSectionFieldsVms> sectionFieldsVms = new List<FormsSectionFieldsVms>();
            var fields = db.FormFields.AsQueryable();
            if(type == 1)
            {
                fields = fields.Where(p => p.FormSectionId == sectionId).OrderBy(p => p.Order);
            }
            else
            {
                fields = fields.Where(p => p.FormFieldId == sectionId).OrderBy(p => p.Order);
            }
            foreach(var f in fields)
            {
                FormsSectionFieldsVms formsSection = new FormsSectionFieldsVms
                {
                    FieldType = f.Type,
                    HelperText = f.HelperText,
                    Id = f.Id,
                    IsMultiple = f.IsMultiple,
                    IsRequired = f.IsRequired,
                    Max = f.Max,
                    Min = f.Min,
                    Name = f.Name,
                    Placeholder = f.Placeholder,
                    groupFields = GetFormsFields(f.Id,2),
                    formFieldsOptions = GetFormsFieldOptions(f.Id),
                };
                sectionFieldsVms.Add(formsSection);
            }
            return sectionFieldsVms;
        }

        public List<FormFieldsOptionsVms> GetFormsFieldOptions(int feildId)
        {
            var count = 0;
            List<FormFieldsOptionsVms> formFieldsOptionsVms = new List<FormFieldsOptionsVms>();
            var fields = db.FormFieldsOptions.Where(o => o.FormsFieldId == feildId).OrderBy(p => p.Order);
            foreach (var of in fields)
            {
                FormFieldsOptionsVms formsSection = new FormFieldsOptionsVms
                {
                    HelpText = of.HelpText,
                    Id = of.Id,
                    Type = of.Type,
                    Text = of.Text,
                    Value = of.Value,
                    SectionFields = of.Type == FormFieldOptionType.Expandable ? GetFormsFieldOptionsBindFields(of.Id): null,
                };
                formFieldsOptionsVms.Add(formsSection);
            }
            return formFieldsOptionsVms;
        }

        public List<FormsSectionFieldsVms> GetFormsFieldOptionsBindFields(int optionId)
        {
            List<FormsSectionFieldsVms> sectionFieldsVms = new List<FormsSectionFieldsVms>();
            var binders = db.FormFieldOptionBinders.Where(o => o.FormsFieldOptionId == optionId);
            if(binders.Count() > 0)
            {
                var Ids = binders.Select(p => p.FormsFieldId);
                var fields = db.FormFields.Where(p => Ids.Contains(p.Id));
                foreach (var f in fields)
                {
                    FormsSectionFieldsVms formsSection = new FormsSectionFieldsVms
                    {
                        FieldType = f.Type,
                        HelperText = f.HelperText,
                        Id = f.Id,
                        IsMultiple = f.IsMultiple,
                        IsRequired = f.IsRequired,
                        Max = f.Max,
                        Min = f.Min,
                        Name = f.Name,
                        Placeholder = f.Placeholder,
                        formFieldsOptions = GetFormsFieldOptions(f.Id),
                    };
                    sectionFieldsVms.Add(formsSection);
                }
            }
            return sectionFieldsVms;
        }

        public ResponseDto AddSection(AddSectionVms modal)
        {
            try
            {
                int? order = 0;
                if (modal.Type == 1)
                {
                    var applicationSetting = db.ApplicationSettings.Find(modal.Id);
                    if (applicationSetting == null)
                    {
                        return JsonResponse2(400, "applicaiton setting not found", modal);
                    }
                    var sectionLast = db.FormSections.Where(p => p.ApplicationSettingId == modal.Id).OrderBy(p => p.Order).LastOrDefault();
                    order = sectionLast == null ? 0 : sectionLast.Order + 1;
                    var section = new FormSections
                    {
                        CreatedAt = currentTime,
                        Name = modal.name,
                        Order = order,
                        ApplicationSettingId = modal.Id,
                        IsRequired = modal.isrequired,
                        UpdatedAt = currentTime,
                    };
                    db.FormSections.Add(section);
                    db.SaveChanges();
                }
                else
                {
                    var section = db.FormSections.Find(modal.Id);
                    section.IsRequired = modal.isrequired;
                    section.Name = modal.name;

                    db.Entry(section).State = EntityState.Modified;
                    db.SaveChanges();
                }
                
                return JsonResponse2(200, "success", null);
            }
            catch (Exception ex)
            {
                return JsonResponse2(400, "error", ex.Message);
            }
        }

        public ResponseDto DeleteSection(int Id)
        {
            try
            {
                var section = db.FormSections.Find(Id);
                if(section != null)
                {
                    //db.FormSections.Remove(section);
                    //db.SaveChanges();
                    return JsonResponse2(200, "success", null);
                }
                else
                {
                    return JsonResponse2(400, "section not found", null);
                }
            }
            catch (Exception ex)
            {
                return JsonResponse2(400, "error", ex.Message);
            }
        }

        public ResponseDto AddFields(AddFieldVms modal)
        {
            try
            {
                if(modal.Type == 1)
                {
                    int? sectionid = null;
                    int? feildid = null;
                    int? order = 0;
                    if (modal.IsChild == 1)
                    {
                        var section = db.FormSections.Find(modal.SectionId);
                        if (section == null)
                        {
                            return JsonResponse2(400, "section not found", modal);
                        }
                        sectionid = modal.SectionId;

                        var fieldLast = db.FormFields.Where(p => p.FormSectionId == modal.SectionId).OrderBy(p=>p.Order).LastOrDefault();
                        order = fieldLast==null?0: fieldLast.Order + 1;
                    }
                    else
                    {
                        var field2 = db.FormFields.Find(modal.SectionId);
                        if (field2 == null)
                        {
                            return JsonResponse2(400, "field not found", modal);
                        }
                        feildid = modal.SectionId;

                        var fieldLast = db.FormFields.Where(p => p.FormFieldId == modal.SectionId).OrderBy(p => p.Order).LastOrDefault();
                        order = fieldLast == null ?0: fieldLast.Order + 1;
                    }
                    
                    var field = new FormFields
                    {
                        CreatedAt = currentTime,
                        Name = modal.label,
                        HelperText = modal.helperText,
                        IsMultiple = modal.is_multiple,
                        Max = modal.maximum,
                        Min = modal.minimum,
                        Placeholder = modal.placeholder,
                        Type = modal.select_field_type,
                        FormSectionId = sectionid,
                        FormFieldId = feildid,
                        Order = order,
                        IsRequired = modal.isrequired,
                        UpdatedAt = currentTime,
                    };
                    db.FormFields.Add(field);
                    db.SaveChanges();
                }
                else
                {
                    var field = db.FormFields.Find(modal.FieldId);
                    if (field == null)
                    {
                        return JsonResponse2(400, "field not found", modal);
                    }
                    field.Name = modal.label;
                    field.UpdatedAt = currentTime;
                    field.HelperText = modal.helperText;
                    field.Placeholder = modal.placeholder;
                    field.Max = modal.maximum;
                    field.Min = modal.minimum;
                    field.IsRequired = modal.isrequired;
                    field.IsMultiple = modal.is_multiple;
                    db.Entry(field).State = EntityState.Modified;
                    db.SaveChanges();
                }
                return JsonResponse2(200, "success", null);
            }
            catch (Exception ex)
            {
                return JsonResponse2(400, "error", ex.Message);
            }
        }

        public ResponseDto AddOptionOfFieldVms(AddOptionOfFieldVms modal)
        {
            try
            {
                int? order = 0;
                if (modal.FormType == 1)
                {
                    var field = db.FormFields.Find(modal.FieldId);
                    if (field == null)
                    {
                        return JsonResponse2(400, "field not found", modal);
                    }
                    var optionLast = db.FormFieldsOptions.Where(p => p.FormsFieldId == modal.FieldId).OrderBy(p => p.Order).LastOrDefault();
                    order = optionLast == null ? 0 : optionLast.Order + 1;
                    var option = new FormFieldsOptions
                    {
                        CreatedAt = currentTime,
                        Type = FormFieldOptionType.Simple,
                        UpdatedAt = currentTime,
                        FormsFieldId = field.Id,
                        HelpText = modal.HelpText,
                        Text = modal.Text,
                        Value = "1",
                        Order = order,
                    };
                    db.FormFieldsOptions.Add(option);
                    db.SaveChanges();
                }
                else
                {
                    var option = db.FormFieldsOptions.Find(modal.OptionId);
                    if (option == null)
                    {
                        return JsonResponse2(400, "option not found", modal);
                    }
                    option.Text = modal.Text;
                    option.HelpText = modal.HelpText;
                    db.Entry(option).State = EntityState.Modified;
                    db.SaveChanges();
                }
                return JsonResponse2(200, "success", null);
            }
            catch (Exception ex)
            {
                return JsonResponse2(400, "error", ex.Message);
            }
        }

        public ResponseDto DeleteField(int Id)
        {
            try
            {
                var field = db.FormFields.Find(Id);
                if (field != null)
                {
                    db.FormFields.Remove(field);
                    db.SaveChanges();
                    return JsonResponse2(200, "success", null);
                }
                else
                {
                    return JsonResponse2(400, "field not found", null);
                }
            }
            catch (Exception ex)
            {
                return JsonResponse2(400, "error", ex.Message);
            }
        }

        public ResponseDto DeleteOption(int Id)
        {
            try
            {
                var option = db.FormFieldsOptions.Find(Id);
                if (option != null)
                {
                    db.FormFieldsOptions.Remove(option);
                    db.SaveChanges();
                    return JsonResponse2(200, "success", null);
                }
                else
                {
                    return JsonResponse2(400, "field not found", null);
                }
            }
            catch (Exception ex)
            {
                return JsonResponse2(400, "error", ex.Message);
            }
        }

        public List<ProjectsVms> UserProjects(string Id)
        {
            var user = db.Users.Find(Id);
            var company = db.Users.Find(user.CompanyId);
            if(company == null)
            {
                throw new ValidationException("Company Not Found");
            }
            var projects = db.UserProjects.Where(p => p.UserId == Id).Select(n=> new ProjectsVms { 
                ApplicaitonName = n.ApplicationObject.Name,
                Year = n.Year,
                Applicationid = n.Application,
                Status = n.Status,
                UserId = n.UserId,
                ProjectId = n.Id,
                ApplicationType = db.ProjectsSettings.Where(p=>p.ProjectId == n.Id).Select(p=>new ApplicationsettingVms { 
                    ApplicationType = p.Type,
                    Name = p.SettingName,
                    Status = p.Status,
                    ProjectSettingId = p.Id,
                }).ToList(),
                Email = company.Email,
                Image = company.ProfileImageUrl,
                PersonName = company.FullName,
                Phone = company.PhoneNumber,
                UserName = company.UserName,
                CompanyName = company.CompanyName,
            }).ToList();
            return projects;
        }

        public ResponseDto AddProject(AddNewProjectVms modal)
        {
            try
            {
                var applicaiton = db.Applications.Find(modal.Applicationid);
                if (applicaiton == null)
                {
                    return JsonResponse2(400, "application not found", null);
                }
                var projectFind = db.UserProjects.Where(p => p.UserId == modal.UserId && p.Application == modal.Applicationid && p.Year == modal.Year);
                if(projectFind.Count() > 0)
                {
                    return JsonResponse2(400, "Already created project with this year and application", null);
                }
                UserProjects userProjects = new UserProjects
                {
                    Status = ApplicationStatus.Pending,
                    Application = modal.Applicationid,
                    CreatedAt = currentTime,
                    UpdatedAt = currentTime,
                    UserId = modal.UserId,
                    Year = modal.Year,
                };
                db.UserProjects.Add(userProjects);

                var applicationSetting = db.ApplicationSettings.Where(p => p.ApplicationId == applicaiton.Id);
                foreach (var i in applicationSetting)
                {
                    ProjectsSettings projectsSettings = new ProjectsSettings
                    {
                        SettingName = i.Name,
                        Status = ProjectSettingStatus.InProgress,
                        CreatedAt = currentTime,
                        ProjectObject = userProjects,
                        Type = i.ApplicationType,
                        UpdatedAt = currentTime,
                    };
                    db.ProjectsSettings.Add(projectsSettings);
                }
                db.SaveChanges();
                return JsonResponse2(200, "success", null);
            }
            catch(Exception ex)
            {
                return JsonResponse2(504, ex.Message, null);
            }
        }

        public ResponseDto GetProjectsDocuments(int Id)
        {
            var projectSetting = db.ProjectsSettings.Find(Id);
            if (projectSetting == null)
            {
                return JsonResponse2(400, "error", null);
            }
            var documents = db.ProjectDocuments.Where(p => p.ProjectSettingId == Id).AsEnumerable().Select(n=>new ListofDocumentsVms
            { 
                DocumentUrl = n.DocumentUrl,
                FileSize = n.FileSize,
                Name = n.Name,
                Type = n.Type,
                CreatedAt = n.CreatedAt.ToString("dd-MMM-yyyy hh:mm:ss tt"),
                UserId = n.UserId,
                Id = n.Id,
            });

            DocumentsVms documentsVms = new DocumentsVms
            {
                ProjectSettingName = projectSetting.SettingName,
                ProjectSettingId = projectSetting.Id,
                ListofDocumentsVms = documents.ToList(),
            };

            return JsonResponse2(200, "success", documentsVms);
        }

        public ResponseDto GetAdminProjectsDocuments(int Id , ApplicationTypes s)
        {
            var project = db.UserProjects.Find(Id);
            if (project == null)
            {
                return JsonResponse2(400, "error", null);
            }
            var projectSetting = db.ProjectsSettings.Where(p => p.ProjectId == project.Id && p.Type == s).FirstOrDefault();
            var documents = db.ProjectDocuments.Where(p => p.ProjectSettingId == projectSetting.Id).AsEnumerable().Select(n => new ListofDocumentsVms
            {
                DocumentUrl = n.DocumentUrl,
                FileSize = n.FileSize,
                Name = n.Name,
                Type = n.Type,
                CreatedAt = n.CreatedAt.ToString("dd-MMM-yyyy hh:mm:ss tt"),
                UserId = n.UserId,
                Id = n.Id,
            });

            DocumentsVms documentsVms = new DocumentsVms
            {
                ProjectSettingName = projectSetting.SettingName,
                ProjectSettingId = projectSetting.Id,
                ListofDocumentsVms = documents.ToList(),
            };

            return JsonResponse2(200, "success", documentsVms);
        }

        public List<ClientFormsSectionsVms> GetProjectSettingForm(int Id)
        {
            var projectSetting = db.ProjectsSettings.Find(Id);
            var project = db.UserProjects.Find(projectSetting.ProjectId);
            var application = db.Applications.Find(project.Application);
            var applicaitonSetting = db.ApplicationSettings.Where(p => p.ApplicationType == ApplicationTypes.Form && p.ApplicationId == application.Id).FirstOrDefault();

            try
            {
                List<ClientFormsSectionsVms> formsSections1 = new List<ClientFormsSectionsVms>();
                var formsSections = db.FormSections.Where(p => p.ApplicationSettingObject.ApplicationId == applicaitonSetting.Id).OrderBy(p => p.Order);
                foreach (var p in formsSections)
                {
                    var checksubmission = db.ProjectFormSection.Where(a => a.SectionId == p.Id && a.ProjectSettingId == projectSetting.Id).FirstOrDefault();
                    var s = checksubmission == null ? FormSectionStatus.Incomplete : FormSectionStatus.Complete;
                    ClientFormsSectionsVms forms1 = new ClientFormsSectionsVms
                    {
                        CreatedAt = p.CreatedAt.ToString("dd-MMM-yyyy hh:mm:ss tt"),
                        Id = p.Id,
                        Name = p.Name,
                        IsRequired = p.IsRequired == true ? "Required" : "Not Required",
                        Status = s,
                        UpdatedAt = p.UpdatedAt != null ? p.UpdatedAt.Value.ToString("dd-MMM-yyyy hh:mm:ss tt") : null,
                    };
                    formsSections1.Add(forms1);
                }
                return formsSections1;
            }
            catch (Exception es)
            {
                throw new ValidationException(es.GetBaseException().Message);
            }
        }

        public ResponseDto DeleteDocument(int Id)
        {
            try
            {
                var document = db.ProjectDocuments.Find(Id);
                List<string> vs = new List<string>();
                if (document != null)
                {
                    vs.Add(document.Name);
                    db.ProjectDocuments.Remove(document);
                    db.SaveChanges();
                    return JsonResponse2(200, "success", vs);
                }
                else
                {
                    return JsonResponse2(400, "document not found", null);
                }
            }
            catch (Exception ex)
            {
                return JsonResponse2(400, "error", ex.Message);
            }
        }

        public ResponseDto UploadProjectDocuments(UploadDocuments modal)
        {
            try
            {
                var setting = db.ProjectsSettings.Find(modal.Id);
                if(setting == null)
                {
                    return JsonResponse2(400, "setting not found", null);
                }
                if (modal.files.Count() == 0)
                {
                    return JsonResponse2(400, "setting not found", null);
                }
                foreach (var i in modal.ListImages)
                {
                    ProjectDocuments projectDocuments = new ProjectDocuments
                    {
                        Status = EntityStatus.Active,
                        FileSize = i.FileSize,
                        ProjectSettingId = setting.Id,
                        DocumentUrl = i.Link,
                        Name = i.Name,
                        Type = DocumentType.AddedByUser,
                        UserId = modal.UserId,
                        CreatedAt = currentTime,
                        UpdatedAt = currentTime,
                    };
                    db.ProjectDocuments.Add(projectDocuments);
                }
                setting.Status = ProjectSettingStatus.Completed;
                db.Entry(setting).State = EntityState.Modified;
                db.SaveChanges();
                return JsonResponse2(200, "success", null);
            }
            catch (Exception ex)
            {
                return JsonResponse2(400, "error", ex.Message);
            }
        }

        public ResponseDto AdminUploadProjectDocuments(UploadDocuments modal)
        {
            try
            {
                var setting = db.ProjectsSettings.Find(modal.Id);
                if (setting == null)
                {
                    return JsonResponse2(400, "setting not found", null);
                }
                if (modal.files.Count() == 0)
                {
                    return JsonResponse2(400, "setting not found", null);
                }
                foreach (var i in modal.ListImages)
                {
                    ProjectDocuments projectDocuments = new ProjectDocuments
                    {
                        Status = EntityStatus.Active,
                        FileSize = i.FileSize,
                        ProjectSettingId = setting.Id,
                        DocumentUrl = i.Link,
                        Name = i.Name,
                        Type = DocumentType.AddedByAdmin,
                        UserId = modal.UserId,
                        CreatedAt = currentTime,
                        UpdatedAt = currentTime,
                    };
                    db.ProjectDocuments.Add(projectDocuments);
                }
                setting.Status = ProjectSettingStatus.Completed;
                db.Entry(setting).State = EntityState.Modified;
                db.SaveChanges();
                return JsonResponse2(200, "success", null);
            }
            catch (Exception ex)
            {
                return JsonResponse2(400, "error", ex.Message);
            }
        }

        public ResponseDto SaveFormInfo(List<SaveFormSection> vs, int Id, int SettingId)
        {
            try
            {
                var checkPrev = db.ProjectFormSection.Where(p => p.ProjectSettingId == SettingId && p.SectionId == Id && p.Status == FormSectionStatus.Complete).FirstOrDefault();
                if (checkPrev != null)
                {
                    var allAnswers = db.ProjectFormSubmissions.Where(p => p.ProjectSettingId == SettingId && p.SectionId == Id);
                    db.ProjectFormSubmissions.RemoveRange(allAnswers.ToList());
                    db.ProjectFormSection.Remove(checkPrev);
                }
                foreach (var i in vs)
                {
                    if (i.fieldtype == 8)
                    {
                        GetFieldEightValues(i);
                    }
                    else
                    {
                        ProjectFormSubmissions projectFormSubmissions = new ProjectFormSubmissions
                        {
                            SectionId = i.sectionId,
                            ProjectSettingId = i.settingId,
                            CreatedAt = currentTime,
                            FieldId = i.fieldId,
                            Label = i.label,
                            Value = i.value,
                            UpdatedAt = currentTime,
                        };
                        db.ProjectFormSubmissions.Add(projectFormSubmissions);
                        if (i.fieldtype == 4)
                        {
                            foreach (var a in i.values)
                            {
                                ProjectFormSubmissionValues projectFormSubmissionValues = new ProjectFormSubmissionValues
                                {
                                    Status = EntityStatus.Active,
                                    FieldObject = projectFormSubmissions,
                                    CreatedAt = currentTime,
                                    UpdatedAt = currentTime,
                                    Value = a
                                };
                                db.ProjectFormSubmissionValues.Add(projectFormSubmissionValues);
                            }
                        }
                    }
                }
                ProjectFormSection projectFormSection = new ProjectFormSection
                {
                    SectionId = Id,
                    Status = FormSectionStatus.Complete,
                    ProjectSettingId = SettingId,
                    CreatedAt = currentTime,
                    UpdatedAt = currentTime,
                };
                db.ProjectFormSection.Add(projectFormSection);
                db.SaveChanges();
                var projectSettingFormCheck = db.ProjectsSettings.Find(SettingId);
                var project = db.UserProjects.Find(projectSettingFormCheck.ProjectId);
                var application = db.Applications.Find(project.Application);
                var applicationSetting = db.ApplicationSettings.Find(application.Id);
                var Allsections = db.FormSections.Where(p => p.ApplicationSettingId == applicationSetting.Id).Select(p=>p.Id).ToList();
                var projectformSection = db.ProjectFormSection.Where(p => p.ProjectSettingId == projectSettingFormCheck.Id && p.Status == FormSectionStatus.Complete).Select(p=>p.SectionId).ToList();

                var firstNotSecond = projectformSection.Except(Allsections).ToList();
                var secondNotFirst = Allsections.Except(projectformSection).ToList();
                if (!firstNotSecond.Any() && !secondNotFirst.Any()) 
                {
                    projectSettingFormCheck.Status = ProjectSettingStatus.Completed;
                    db.Entry(projectSettingFormCheck).State = EntityState.Modified;
                    db.SaveChanges();
                }
                return JsonResponse2(200, "success", null);
            }
            catch(Exception ex)
            {
                return JsonResponse2(504, ex.Message, null);
            }
        }
        
        public void GetFieldEightValues(SaveFormSection form)
        {
            ProjectFormSubmissions projectFormSubmissions = new ProjectFormSubmissions
            {
                SectionId = form.sectionId,
                ProjectSettingId = form.settingId,
                CreatedAt = currentTime,
                FieldId = form.fieldId,
                Label = form.label,
                Value = form.value,
                UpdatedAt = currentTime,
            };
            db.ProjectFormSubmissions.Add(projectFormSubmissions);
            foreach (var i in form.fieldValues)
            {
                if (i.fieldtype == 8)
                {
                    GetFieldEightValues(i);
                }
                else
                {
                    ProjectFormSubmissions projectFormSubmissions2 = new ProjectFormSubmissions
                    {
                        SectionId = i.sectionId,
                        ProjectSettingId = i.settingId,
                        CreatedAt = currentTime,
                        FieldId = i.fieldId,
                        Label = i.label,
                        Value = i.value,
                        UpdatedAt = currentTime,
                        SubmissionObject = projectFormSubmissions,
                    };
                    db.ProjectFormSubmissions.Add(projectFormSubmissions2);

                    if (i.fieldtype == 4)
                    {
                        foreach (var a in i.values)
                        {
                            ProjectFormSubmissionValues projectFormSubmissionValues = new ProjectFormSubmissionValues
                            {
                                Status = EntityStatus.Active,
                                FieldObject = projectFormSubmissions,
                                CreatedAt = currentTime,
                                UpdatedAt = currentTime,
                                Value = a
                            };
                            db.ProjectFormSubmissionValues.Add(projectFormSubmissionValues);
                        }
                    }
                }
            }
        }

        public List<ProjectsVms> GetUserProjects(string Id)
        {
            var company = db.Users.Find(Id);
            if (company == null)
            {
                throw new ValidationException("Company Not Found");
            }
            var companyClients = db.Users.Where(p => p.CompanyId == company.Id).Select(p=>p.Id).ToList();
            
            var projects = db.UserProjects.Where(p => companyClients.Contains(p.UserId)).Select(n => new ProjectsVms
            {
                CustomerEmail = n.UserObject.Email,
                CustomerName = n.UserObject.FirstName + " " + n.UserObject.LastName,
                CustomerProfileImageUrl = n.UserObject.ProfileImageUrl == ""? "/mon.jpg": n.UserObject.ProfileImageUrl,
                CustomerUsername = n.UserObject.UserName,
                ApplicaitonName = n.ApplicationObject.Name,
                Year = n.Year,
                Applicationid = n.Application,
                Status = n.Status,
                CreatedAt = n.CreatedAt.ToString(),
                UserId = n.UserId,
                ProjectId = n.Id,
                ApplicationType = db.ProjectsSettings.Where(p => p.ProjectId == n.Id).Select(p => new ApplicationsettingVms
                {
                    ApplicationType = p.Type,
                    Name = p.SettingName,
                    Status = p.Status,
                    ProjectSettingId = p.Id,
                }).ToList(),
            }).ToList();
            return projects;
        }

        public ResponseDto MarkInProgress(int Id)
        {
            try
            {
                var entity = db.UserProjects.Find(Id);
                if (entity == null)
                {
                    return JsonResponse2(400, "entity not found", null);
                }
                else
                {
                    entity.Status = ApplicationStatus.InProgress;
                    db.Entry(entity).State = EntityState.Modified;
                    db.SaveChanges();
                    return JsonResponse2(200, "success", null);
                }
            }
            catch (Exception es)
            {
                return JsonResponse2(504, es.GetBaseException().Message, null);
            }
        }
        public ResponseDto CompleteProject(int Id)
        {
            try
            {
                var entity = db.UserProjects.Find(Id);
                if (entity == null)
                {
                    return JsonResponse2(400, "entity not found", null);
                }
                else
                {
                    entity.Status = ApplicationStatus.Completed;
                    db.Entry(entity).State = EntityState.Modified;
                    db.SaveChanges();
                    return JsonResponse2(200, "success", null);
                }
            }
            catch (Exception es)
            {
                return JsonResponse2(504, es.GetBaseException().Message, null);
            }
        }
        public ResponseDto RejectProject(int Id)
        {
            try
            {
                var entity = db.UserProjects.Find(Id);
                if (entity == null)
                {
                    return JsonResponse2(400, "entity not found", null);
                }
                else
                {
                    entity.Status = ApplicationStatus.Canceled;
                    db.Entry(entity).State = EntityState.Modified;
                    db.SaveChanges();
                    return JsonResponse2(200, "success", null);
                }
            }
            catch (Exception es)
            {
                return JsonResponse2(504, es.GetBaseException().Message, null);
            }
        }

        public AnswerApplicationInfoVms GetAnswersOfProjects(int Id)
        {
            try
            {
                List<AnswerSectionVms> vs = new List<AnswerSectionVms>();
                var entity = db.UserProjects.Find(Id);
                var name = "";
                if (entity == null)
                {
                    throw new ValidationException("entity not found");
                }
                else
                {
                    var getAnswerSetting = db.ProjectsSettings.Where(p => p.ProjectId == Id && p.Type == ApplicationTypes.Form).FirstOrDefault();
                    var submision = db.ProjectFormSection.Where(p => p.ProjectSettingId == getAnswerSetting.Id);
                    foreach(var i in submision)
                    {
                        var section = db.FormSections.Find(i.SectionId);
                        AnswerSectionVms answerSectionVms = new AnswerSectionVms
                        {
                            sectionId = i.SectionId,
                            sectionName = section.Name,
                            fieldValues = db.ProjectFormSubmissions.Where(p=>p.SectionId == i.SectionId).Select(p=>new AnswerFormSectionVms {
                                fieldId = p.FieldId,
                                fieldtype = (int)p.FieldObject.Type,
                                label = p.Label,
                                value = p.Value,
                                values = db.ProjectFormSubmissionValues.Where(a =>a.SubmissionId == p.Id).Select(p=>p.Value).ToList(),
                            }).ToList(),
                        };
                        vs.Add(answerSectionVms);
                    }
                }
                var application = db.Applications.Find(entity.Application);
                var user = db.Users.Find(entity.UserId);
                AnswerApplicationInfoVms answerApplicationInfoVms = new AnswerApplicationInfoVms
                {
                    AnswerSectionVms = vs,
                    ApplicationId = entity.Application,
                    ApplicationName = application.Name,
                    UserEmail = user.Email,
                    Year = entity.Year,
                };
                return answerApplicationInfoVms;
            }
            catch (Exception es)
            {
                throw new ValidationException(es.GetBaseException().Message);
            }
        }
        FormsSectionsVms IAdminService.GetClientSectionDetail(int Id,int SettingId)
        {
            try
            {
                var p = db.FormSections.Find(Id);
                if (p == null)
                {
                    return new FormsSectionsVms();
                }
                var applicationSetting = db.ApplicationSettings.Find(p.ApplicationSettingId);
                if (applicationSetting == null)
                {
                    return new FormsSectionsVms();
                }
                FormsSectionsVms forms1 = new FormsSectionsVms
                {
                    CreatedAt = p.CreatedAt.ToString("dd-MMM-yyyy hh:mm:ss tt"),
                    Id = p.Id,
                    Name = p.Name,
                    IsRequired = p.IsRequired == true ? "Required" : "Not Required",
                    UpdatedAt = p.UpdatedAt != null ? p.UpdatedAt.Value.ToString("dd-MMM-yyyy hh:mm:ss tt") : null,
                    SectionFields = GetFormsFields(p.Id, 1),
                    ApplicationId = applicationSetting.ApplicationId,
                };
                return forms1;
            }
            catch (Exception es)
            {
                throw new ValidationException(es.GetBaseException().Message);
            }
        }
        FormsSectionsWithAnswerVms IAdminService.GetClientSectionDetailWithAnswer(int Id, int SettingId)
        {
            try
            {
                var p = db.FormSections.Find(Id);
                if (p == null)
                {
                    return new FormsSectionsWithAnswerVms();
                }
                var applicationSetting = db.ApplicationSettings.Find(p.ApplicationSettingId);
                if (applicationSetting == null)
                {
                    return new FormsSectionsWithAnswerVms();
                }
                FormsSectionsWithAnswerVms forms1 = new FormsSectionsWithAnswerVms
                {
                    CreatedAt = p.CreatedAt.ToString("dd-MMM-yyyy hh:mm:ss tt"),
                    Id = p.Id,
                    Name = p.Name,
                    IsRequired = p.IsRequired == true ? "Required" : "Not Required",
                    UpdatedAt = p.UpdatedAt != null ? p.UpdatedAt.Value.ToString("dd-MMM-yyyy hh:mm:ss tt") : null,
                    SectionFields = GetFormsFieldsWithAnswers(p.Id, 1, SettingId,0),
                    ApplicationId = applicationSetting.ApplicationId,
                };
                return forms1;
            }
            catch (Exception es)
            {
                throw new ValidationException(es.GetBaseException().Message);
            }
        }
        public List<FormsSectionFieldsWithAnswerVms> GetFormsFieldsWithAnswers(int sectionId, int type,int SettingId,int level)
        {
            List<FormsSectionFieldsWithAnswerVms> sectionFieldsVms = new List<FormsSectionFieldsWithAnswerVms>();
            var fields = db.FormFields.AsQueryable();
            if (type == 1)
            {
                fields = fields.Where(p => p.FormSectionId == sectionId).OrderBy(p => p.Order);
            }
            else
            {
                fields = fields.Where(p => p.FormFieldId == sectionId).OrderBy(p => p.Order);
            }
            foreach (var f in fields)
            {
                List<string> checkBoxAnswers = new List<string>();
                List<List<FormsSectionFieldsWithAnswerVms>> vs = new List<List<FormsSectionFieldsWithAnswerVms>>();
                var answerValue = "";
                if(f.Type == FormFeildType.Checkbox)
                {
                    if (level == 0)
                    {
                        var answer = db.ProjectFormSubmissions.Where(p => p.FieldId == f.Id && p.ProjectSettingId == SettingId).FirstOrDefault();
                        if (answer != null)
                        {
                            checkBoxAnswers = db.ProjectFormSubmissionValues.Where(p => p.SubmissionId == answer.Id).Select(p => p.Value).ToList();
                        }
                    }
                    else
                    {
                        var answer = db.ProjectFormSubmissions.Where(p => p.FieldId == f.Id && p.ProjectSettingId == SettingId && p.SubmissionId == level).FirstOrDefault();
                        if (answer != null)
                        {
                            checkBoxAnswers = db.ProjectFormSubmissionValues.Where(p => p.SubmissionId == answer.Id).Select(p => p.Value).ToList();
                        }
                    }
                }
                else if (f.Type == FormFeildType.IsMultipleSection)
                {
                    var answer = db.ProjectFormSubmissions.Where(p => p.FieldId == f.Id && p.ProjectSettingId == SettingId);
                    foreach(var i in answer)
                    {
                        vs.Add(GetFormsFieldsWithAnswers(f.Id, 2, SettingId,i.Id));
                    }
                }
                else
                {
                    if(level == 0)
                    {
                        var answer = db.ProjectFormSubmissions.Where(p => p.FieldId == f.Id && p.ProjectSettingId == SettingId).FirstOrDefault();
                        answerValue = answer != null ? answer.Value : "";
                    }
                    else
                    {
                        var answer = db.ProjectFormSubmissions.Where(p => p.FieldId == f.Id && p.ProjectSettingId == SettingId && p.SubmissionId == level).FirstOrDefault();
                        answerValue = answer != null ? answer.Value : "";
                    }
                }

                FormsSectionFieldsWithAnswerVms formsSection = new FormsSectionFieldsWithAnswerVms
                {
                    FieldType = f.Type,
                    HelperText = f.HelperText,
                    Id = f.Id,
                    IsMultiple = f.IsMultiple,
                    IsRequired = f.IsRequired,
                    Max = f.Max,
                    Min = f.Min,
                    MultitpleAnswers = checkBoxAnswers,
                    Answer = answerValue,
                    Name = f.Name,
                    MultiFieldQuestionsAndAnswers = vs,
                    Placeholder = f.Placeholder,
                    groupFields = GetFormsFieldsWithAnswers(f.Id, 2, SettingId,0),
                    formFieldsOptions = GetFormsFieldOptionsWithAnswers(f.Id),
                };
                sectionFieldsVms.Add(formsSection);
            }
            return sectionFieldsVms;
        }
        public List<FormFieldsOptionsWithAnswerVms> GetFormsFieldOptionsWithAnswers(int feildId)
        {
            var count = 0;
            List<FormFieldsOptionsWithAnswerVms> formFieldsOptionsVms = new List<FormFieldsOptionsWithAnswerVms>();
            var fields = db.FormFieldsOptions.Where(o => o.FormsFieldId == feildId).OrderBy(p => p.Order);
            foreach (var of in fields)
            {
                FormFieldsOptionsWithAnswerVms formsSection = new FormFieldsOptionsWithAnswerVms
                {
                    HelpText = of.HelpText,
                    Id = of.Id,
                    Type = of.Type,
                    Text = of.Text,
                    Value = of.Value,
                    SectionFields = of.Type == FormFieldOptionType.Expandable ? GetFormsFieldOptionsBindFieldsAnswers(of.Id) : null,
                };
                formFieldsOptionsVms.Add(formsSection);
            }
            return formFieldsOptionsVms;
        }
        public List<FormsSectionFieldsWithAnswerVms> GetFormsFieldOptionsBindFieldsAnswers(int optionId)
        {
            List<FormsSectionFieldsWithAnswerVms> sectionFieldsVms = new List<FormsSectionFieldsWithAnswerVms>();
            var binders = db.FormFieldOptionBinders.Where(o => o.FormsFieldOptionId == optionId);
            if (binders.Count() > 0)
            {
                var Ids = binders.Select(p => p.FormsFieldId);
                var fields = db.FormFields.Where(p => Ids.Contains(p.Id));
                foreach (var f in fields)
                {
                    FormsSectionFieldsWithAnswerVms formsSection = new FormsSectionFieldsWithAnswerVms
                    {
                        FieldType = f.Type,
                        HelperText = f.HelperText,
                        Id = f.Id,
                        IsMultiple = f.IsMultiple,
                        IsRequired = f.IsRequired,
                        Max = f.Max,
                        Min = f.Min,
                        Name = f.Name,
                        Placeholder = f.Placeholder,
                        formFieldsOptions = GetFormsFieldOptionsWithAnswers(f.Id),
                    };
                    sectionFieldsVms.Add(formsSection);
                }
            }
            return sectionFieldsVms;
        }
    }
}
