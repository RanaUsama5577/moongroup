using DAL.Applications;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Entities.Enum;

namespace DAL.Projects
{
    public class ProjectFormSection:Base2
    {
        public FormSectionStatus Status { get; set; }

        [ForeignKey("SettingObject")]
        public int ProjectSettingId { get; set; }
        public virtual ProjectsSettings SettingObject { get; set; }

        [ForeignKey("SectionObject")]
        public int SectionId { get; set; }
        public virtual FormSections SectionObject { get; set; }
    }
}
