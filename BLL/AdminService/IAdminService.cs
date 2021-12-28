using DAL;
using Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Entities.Enum;

namespace BLL.AdminService
{
    public interface IAdminService
    {
        List<ClientProfileDtos> GetUsers();
        Task<ResponseDto> SignUpUser(RegisterUser SignUpUser);
        Task<ResponseDto> ForgotPassword([FromForm] ForgotPasswordVMS model);
        Task<ResponseDto> ResetPassword([FromForm] ResetPasswordVMS model);
        List<UsersProfileDtos> GetAppUsers(string Id);

        List<ProjectsVms> UserProjects(string Id);
        List<ProjectsVms> GetUserProjects(string Id);
        ResponseDto AddProject(AddNewProjectVms modal);
        Task<ResponseDto> CreateAdmin();
        ResponseDto CreateApplications();
        List<ApplicaitionsVms> ShowApplications();
        ResponseDto ShowApplicationsForCompany(string Id);
        List<ClientFormsSectionsVms> GetProjectSettingForm(int Id);
        List<FormsSectionsVms> ShowForms(int Id);
        FormsSectionsVms GetClientSectionDetail(int Id, int SettingId);
        FormsSectionsWithAnswerVms GetClientSectionDetailWithAnswer(int Id, int SettingId);
        List<FormsSectionFieldsVms> GetFormsFields(int sectionId, int type);
        ResponseDto AddSection(AddSectionVms modal);
        ResponseDto DeleteSection(int Id);
        ResponseDto AddFields(AddFieldVms modal);
        ResponseDto AddOptionOfFieldVms(AddOptionOfFieldVms modal);
        ResponseDto DeleteField(int Id);
        ResponseDto DeleteOption(int Id);
        List<FormFieldsOptionsVms> GetFormsFieldOptions(int Id);

        ResponseDto GetProjectsDocuments(int Id);
        ResponseDto GetAdminProjectsDocuments(int Id, ApplicationTypes applicationTypes);
        ResponseDto DeleteDocument(int Id);
        ResponseDto UploadProjectDocuments(UploadDocuments modal);
        ResponseDto AdminUploadProjectDocuments(UploadDocuments modal);
        ResponseDto SaveFormInfo(List<SaveFormSection> vs, int Id, int SettingId);
        Task<ResponseDto> Login(LoginViewModel LoginUser);
        SuperAdminDashboard SuperDashboardStats();
        AdminDashboard AdminDashboardStats(string Id);

        ApplicationUser getLoginUser(string Id);
        ProfileDtos getLoginUser2(string Id);
        ApplicationUser UpdateProfileImage(string ImageUrl, string Id);
        ApplicationUser UpdateProfile(string Name, string Id);
        Task<ResponseDto> ChangePassword(UpdatePasswordVms model, string Id);
        Task<ResponseDto> Logout();

        ResponseDto BlockUser(string Id);
        ResponseDto UnBlockUser(string Id);


        ResponseDto MarkInProgress(int Id);
        ResponseDto CompleteProject(int Id);
        ResponseDto RejectProject(int Id);

        AnswerApplicationInfoVms GetAnswersOfProjects(int Id);

        
    }
}
