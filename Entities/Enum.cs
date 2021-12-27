using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Entities
{
    public class Enum
    {
        public enum FormFeildType
        {
            Text,
            Number,
            TextArea,
            SelectBox,
            Checkbox,
            Radio,
            Password,
            GroupSection,
            IsMultipleSection,
            DateField,
            MaskField,
            DollarField,
        }

        public enum FormFieldOptionType
        {
            Simple,
            Expandable
        }

        public enum FormSectionStatus
        {
            Incomplete,
            Complete
        }

        public enum ApplicationTypes
        {
            Form,
            Uploader,
            Viewer,
        }

        public enum DocumentType
        {
            AddedByUser,
            AddedByAdmin,
        }

        public enum ReadingTimeType
        {
            Hours,
            Days,
            Weeks,
            Months,
        }
        public enum PackageStatus
        {
            Active,
            Expired,
        }

        public enum TagType
        {
            TwCw,
            Tags,
        }
        
        public enum ContentType
        {
            TermsAndConditions,
            AboutUs,
            AboutApp,
            PrivacyPolicy,
            Information,
        }

        public enum EntityStatus
        {
            Active,
            Blocked,
        }

        public enum ApplicationStatus
        {
            Pending,
            InProgress,
            Completed,
            Canceled,
        }

        public enum ProjectSettingStatus
        {
            InProgress,
            Completed,
        }

        public enum PaymentMethod
        {
            Paypal,
            Ideal,
            BankAccount,
        }

        public enum UserType
        {
            SuperAdmin,
            Admin,
            User,
        }

        public enum LoginProvider
        {
            WithEmail,
            WithGmail,
            WithFacebook,
        }

        public enum DurationType
        {
            Days,
            Weeks,
            Months,
            Years,
        }

        public enum PaymentStatus
        {
            UnPaid,
            Paid,
        }

        public enum QueryStatus
        {
            Resolved,
            UnResolved,
        }

        public enum GenderType
        {
            Male,
            Female,
        }

        public enum NotificationType
        {
            Check,
            QueryResolved
        }
    }
}
