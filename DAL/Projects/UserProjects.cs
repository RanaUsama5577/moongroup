using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Entities.Enum;

namespace DAL.Projects
{
    public class UserProjects:Base2
    {
        public string Year { get; set; }
        public ApplicationStatus Status { get; set; }


        [ForeignKey("ApplicationObject")]
        public int Application { get; set; }
        public virtual Applications.Applications ApplicationObject { get; set; }


        [ForeignKey("UserObject")]
        public string UserId { get; set; }
        public virtual ApplicationUser UserObject { get; set; }
    }
}
