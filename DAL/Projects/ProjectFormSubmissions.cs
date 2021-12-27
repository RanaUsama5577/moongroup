using DAL.Applications;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Projects
{
    public class ProjectFormSubmissions:Base2
    {
        public string Label { get; set; }
        public string Value { get; set; }

        [ForeignKey("SettingObject")]
        public int ProjectSettingId { get; set; }
        public virtual ProjectsSettings SettingObject { get; set; }

        [ForeignKey("FieldObject")]
        public int FieldId { get; set; }
        public virtual FormFields FieldObject { get; set; }

        [ForeignKey("SubmissionObject")]
        public int? SubmissionId { get; set; }
        public virtual ProjectFormSubmissions SubmissionObject { get; set; }

        [ForeignKey("SectionObject")]
        public int SectionId { get; set; }
        public virtual FormSections SectionObject { get; set; }

    }
}
