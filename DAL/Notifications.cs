using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using static Entities.Enum;

namespace DAL
{
    public class Notifications : Base2
    {
        [ForeignKey("NotifyByObject")]
        public string notifyBy { get; set; }
        [ForeignKey("NotitfyToObject")]
        public string notifyTo { get; set; }
        public string title { get; set; }
        public string body { get; set; }
        public bool isSeen { get; set; }
        public bool toALl { get; set; }
        public NotificationType type { get; set; }

        public virtual ApplicationUser NotifyByObject { get; set; }
        public virtual ApplicationUser NotitfyToObject { get; set; }

    }
}
