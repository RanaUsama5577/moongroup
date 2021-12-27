using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Applications
{
    public class ClientApplications:Base2
    {
        [ForeignKey("ApplicationObject")]
        public int Application { get; set; }
        public virtual Applications ApplicationObject { get; set; }


        [ForeignKey("UserObject")]
        public string UserId { get; set; }
        public virtual ApplicationUser UserObject { get; set; }
    }
}
