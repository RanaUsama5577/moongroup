using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using static Entities.Enum;

namespace DAL
{
    public class ApplicationUser : IdentityUser
    {
        public UserType Type { get; set; }
        public EntityStatus Status { get; set; }
        public DateTime RegisteredAt { get; set; }
        public string ApiToken { get; set; }
        public LoginProvider ExternalType { get; set; }
        public string ExternalId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string CompanyName { get; set; }
        public string MobileNumber { get; set; }
        public string WorkNumber { get; set; }
        public string ProfileImageUrl { get; set; }
        public bool IsNotificationDisabled { get; set; }
        public string Address { get; set; }
        public string FcmToken { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public UserType UserType { get; set; }

        [ForeignKey("CompanyObject")]
        public string CompanyId { get; set; }

        public virtual ApplicationUser CompanyObject { get; set; }

    }
}
