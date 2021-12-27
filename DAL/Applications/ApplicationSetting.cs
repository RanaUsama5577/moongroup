using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Entities.Enum;

namespace DAL.Applications
{
    public class ApplicationSetting:Base2
    {
        public string Name { get; set; }
        public ApplicationTypes ApplicationType { get; set; }

        [ForeignKey("ApplicationObject")]
        public int ApplicationId { get; set; }
        public virtual Applications ApplicationObject { get; set; }
    }
}
