using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Entities.Enum;

namespace DAL.Projects
{
    public class ProjectsSettings:Base2
    {
        public ProjectSettingStatus Status { get; set; }
        public ApplicationTypes Type { get; set; }
        public string SettingName { get; set; }

        [ForeignKey("ProjectObject")]
        public int ProjectId { get; set; }
        public virtual UserProjects ProjectObject { get; set; }
    }
}
