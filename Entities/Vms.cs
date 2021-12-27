using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static Entities.Enum;

namespace Entities
{
    public class ProjectsVms
    {
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerUsername { get; set; }
        public string CustomerProfileImageUrl { get; set; }
        public string UserId { get; set; }
        public int Applicationid { get; set; }
        public int ProjectId { get; set; }
        public string CreatedAt { get; set; }
        public string ApplicaitonName { get; set; }
        public ApplicationStatus Status { get; set; }
        public string Status2 { get; set; }
        public string Year { get; set; }

        public string Image { get; set; }
        public string PersonName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string UserName { get; set; }
        public string CompanyName { get; set; }
        public List<ApplicationsettingVms> ApplicationType { get; set; }
    }

    public class AnswerApplicationInfoVms
    {
        public int ApplicationId { get; set; }
        public string ApplicationName { get; set; }
        public string Year { get; set; }
        public string UserEmail { get; set; }
        public List<AnswerSectionVms> AnswerSectionVms { get; set; }
    }

    public class AnswerSectionVms
    {
        public int sectionId { get; set; }
        public string sectionName { get; set; }
        public List<AnswerFormSectionVms> fieldValues { get; set; }
    }

    public class AnswerFormSectionVms
    {
        public int fieldId { get; set; }
        public string label { get; set; }
        public string value { get; set; }
        public int fieldtype { get; set; }
        public List<string> values { get; set; }
    }


    public class SaveFormSection
    {
        public int fieldId { get; set; }
        public string label { get; set; }
        public string value { get; set; }
        public int fieldtype { get; set; }
        public int settingId { get; set; }
        public int sectionId { get; set; }
        public List<string> values { get; set; }
        public List<SaveFormSection> fieldValues { get; set; }
    }

    public class DocumentsVms
    {
        public string ProjectSettingName { get; set; }
        public int ProjectSettingId { get; set; }
        public List<ListofDocumentsVms> ListofDocumentsVms { get; set; }
    }

    public class UploadDocuments
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public IFormFile[] files { get; set; }
        public List<AddListofDocumentsVms> ListImages { get; set; }
    }

    public class AddListofDocumentsVms
    {
        public string Name { get; set; }
        public string Link { get; set; }
        public string FileSize { get; set; }
    }

    public class ListofDocumentsVms
    {
        public string UserId { get; set; }
        public int Id { get; set; }
        public string DocumentUrl { get; set; }
        public string Name { get; set; }
        public string FileSize { get; set; }
        public string CreatedAt { get; set; }
        public DocumentType Type { get; set; }
    }
    public class ApplicationsettingVms
    {
        public string Name { get; set; }
        public int ProjectSettingId { get; set; }
        public ProjectSettingStatus Status { get; set; }
        public ApplicationTypes ApplicationType { get; set; }
    }
    public class AddNewProjectVms
    {
        public int Applicationid { get; set; }
        public string Year { get; set; }
        public string UserId { get; set; }
    }

    

    public class SuperAdminDashboard
    {
        public int TotalUsers { get; set; }
        public int ApplicationTypes { get; set; }
    }


    public class RegisterUser
    {
        [Required(ErrorMessage ="Email is required")]
        public string Email { get; set; }
        [Required(ErrorMessage = "First Name is required")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Last Name is required")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Mobile Number is required")]
        public string MobileNumber { get; set; }
        [Required(ErrorMessage = "Work Number is required")]
        public string WorkNumber { get; set; }
        [Required(ErrorMessage = "Phone Number is required")]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Confirm Password is required")]
        [Compare(nameof(Password), ErrorMessage = "Password and confirm password should be equal")]
        public string ConfirmPassword { get; set; }
        [Required(ErrorMessage = "Year is required")]
        public string Year { get; set; }
        [Required(ErrorMessage = "Application Id is required")]
        public int ApplicationId { get; set; }
    }

    public class AdminDashboard
    {
        public int TotalUsers { get; set; }
        public int PendingApps { get; set; }
        public int InprogressApps { get; set; }
        public int CompletedApps { get; set; }
        public int CanceledApps { get; set; }
    }


    public class ApplicaitionsVms
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }
        public List<string> Types { get; set; }
    }

    


    public class ProfileDtos
    {
        public string Id { get; set; }
        public string Phone { get; set; }
        public string ProfileImageUrl { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string FullName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string companyImage { get; set; }
        public bool PackagePurchased { get; set; }
        public string ExpiryDate { get; set; }
        public UserType Role { get; set; }
        public string Token { get; set; }
        public LoginProvider ExternalType { get; set; }
        public string ExternalId { get; set; }
        public string DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }
        public string FcmToken { get; set; }
        public bool NeedsVerification { get; set; }
        public bool ExternalSignInVerification { get; set; }
    }
    public class ChangePasswordVMS
    {
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Old Password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }
    }
    public class ForgotPasswordVMS
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }

    public class ForgotPasswordLink
    {
        [Required]
        public string Link { get; set; }
    }

    public class ClientProfileDtos
    {
        public string Id { get; set; }
        public string ProfileImageUrl { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string CompanyName { get; set; }
        public string PhoneNumber { get; set; }
        public List<string> ApplicationAssigned { get; set; }
        public EntityStatus Status { get; set; }
        public string CreatedAt { get; set; }
    }

    public class UsersProfileDtos
    {
        public string Id { get; set; }
        public string ProfileImageUrl { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string MobileNumber { get; set; }
        public string WorkNumber { get; set; }
        public EntityStatus Status { get; set; }
        public string CreatedAt { get; set; }
    }

    public class AddSectionVms
    {
        public int Id { get; set; }
        public int Type { get; set; }
        public string name { get; set; }
        public bool isrequired { get; set; }
    }

    public class AddFieldVms
    {
        public int? SectionId { get; set; }
        public int? FieldId { get; set; }
        public int Type { get; set; }
        public string label { get; set; }
        public string helperText { get; set; }
        public int? minimum { get; set; }
        public int? maximum { get; set; }
        public string placeholder { get; set; }
        public int IsChild { get; set; }
        public FormFeildType select_field_type { get; set; }
        public bool isrequired { get; set; }
        public bool is_multiple { get; set; }
    }

    public class AddOptionOfFieldVms
    {
        public int? FieldId { get; set; }
        public int? OptionId { get; set; }
        public string Text { get; set; }
        public string HelpText { get; set; }
        public int FormType { get; set; }
    }


    public class FormsSectionsVms
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string IsRequired { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }
        public int ApplicationId { get; set; }
        public List<FormsSectionFieldsVms> SectionFields{ get; set; }
    }

    public class FormsSectionsWithAnswerVms
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string IsRequired { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }
        public int ApplicationId { get; set; }
        public List<FormsSectionFieldsWithAnswerVms> SectionFields { get; set; }
    }

    public class ClientFormsSectionsVms
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public FormSectionStatus Status { get; set; }
        public string IsRequired { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }
        public int ApplicationId { get; set; }
    }

    public class FormsSectionFieldsVms
    {
        public int Id { get; set; }
        public FormFeildType FieldType { get; set; }
        public bool IsRequired { get; set; }
        public bool IsMultiple { get; set; }
        public string Placeholder { get; set; }
        public string Name { get; set; }
        public string HelperText { get; set; }
        public string Answer { get; set; }
        public int? Min { get; set; }
        public int? Max { get; set; }
        public List<FormsSectionFieldsVms> groupFields { get; set; }
        public List<FormFieldsOptionsVms> formFieldsOptions { get; set; }
    }

    public class FormFieldsOptionsVms
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string Value { get; set; }
        public string Answer { get; set; }
        public string HelpText { get; set; }
        public FormFieldOptionType Type { get; set; }

        public List<FormsSectionFieldsVms> SectionFields { get; set; }
    }

    public class FormsSectionFieldsWithAnswerVms
    {
        public int Id { get; set; }
        public FormFeildType FieldType { get; set; }
        public bool IsRequired { get; set; }
        public bool IsMultiple { get; set; }
        public string Placeholder { get; set; }
        public string Name { get; set; }
        public string HelperText { get; set; }
        public string Answer { get; set; }
        public List<string> MultitpleAnswers { get; set; }
        public List<List<FormsSectionFieldsWithAnswerVms>> MultiFieldQuestionsAndAnswers { get; set; }
        public int? Min { get; set; }
        public int? Max { get; set; }
        public List<FormsSectionFieldsWithAnswerVms> groupFields { get; set; }
        public List<FormFieldsOptionsWithAnswerVms> formFieldsOptions { get; set; }
    }

    public class FormFieldsOptionsWithAnswerVms
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string Value { get; set; }
        public string Answer { get; set; }
        public string HelpText { get; set; }
        public FormFieldOptionType Type { get; set; }

        public List<FormsSectionFieldsWithAnswerVms> SectionFields { get; set; }
    }
}
