using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static Entities.Enum;

namespace Entities
{
    public class ProfileDtosUser
    {
        public string Id { get; set; }
        public string Phone { get; set; }
        public string ProfileImageUrl { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public UserType Role { get; set; }
        public string Token { get; set; }
        public LoginProvider ExternalType { get; set; }
        public string ExternalId { get; set; }
        public string DateOfBirth { get; set; }
        public string Gender { get; set; }
        public int AvailedOffers { get; set; }
        public int SavedMoney { get; set; }
    }

    public class NotificationsVms
    {
        public int Id { get; set; }
        public int? RedirectId { get; set; }
        public string NotifyById { get; set; }
        public string NotifyToId { get; set; }
        public string NotifyToUserName { get; set; }
        public string NotifyToEmail { get; set; }
        public string NotifyToImage { get; set; }
        public string NotifyByUserName { get; set; }
        public string NotifyByEmail { get; set; }
        public string NotifyByImage { get; set; }

        public string Body { get; set; }
        public string Title { get; set; }
        public string CreatedAt { get; set; }
        public NotificationType Type { get; set; }
        public bool IsSeen { get; set; }

    }

    public class ratingVms
    {
        public int Id { get; set; }
        public int rating { get; set; }
        public string review { get; set; }
        public string reviewByEmail { get; set; }
        public string createdAt { get; set; }
        public string reviewByImage { get; set; }
    }

}