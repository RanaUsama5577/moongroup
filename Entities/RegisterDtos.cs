using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using static Entities.Enum;

namespace Entities
{
    public class LoginDtos
    {
        [Required(ErrorMessage = "Email is required.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Email is required.")]
        public string Password { get; set; }
    }
    public class RegisterExternalUserDtos
    {
        [Required(ErrorMessage = "Email is required.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "ExternalType is required.")]
        public LoginProvider ExternalType { get; set; }
        [Required(ErrorMessage = "ExternalId is required.")]
        public string ExternalId { get; set; }
        [Required(ErrorMessage = "Username is required.")]
        public string Name { get; set; }
        public string ProfilePic { get; set; }
    }
    public class RegisterUserDtos
    {
        [Required(ErrorMessage = "Email is required.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "First Name is required.")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Sur Name is required.")]
        public string SurName { get; set; }
        [Required(ErrorMessage = "Username is required.")]
        public string Password { get; set; }
    }
    public class UpdateProfileDtos
    {
        [Required(ErrorMessage = "FirstName is required.")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "LastName is required.")]
        public string LastName { get; set; }
    }
    public class SendOtp
    {
        public string PhoneNumber { get; set; }
        public string Username { get; set; }
    }
    public class VerifyOtp
    {
        public string Email { get; set; }
        public string code { get; set; }
        public string lan { get; set; }
    }
}