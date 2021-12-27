using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Entities.Enum;

namespace DAL.Applications
{
    public class FormFields:Base2
    {
        public FormFeildType Type { get; set; }
        public bool IsRequired { get; set; }
        public bool IsMultiple { get; set; }
        public string Placeholder { get; set; }
        public string Name { get; set; }
        public string HelperText { get; set; }
        public int? Min { get; set; }
        public int? Max { get; set; }
        public int? Order { get; set; }

        [ForeignKey("FormsSectionObject")]
        public int? FormSectionId { get; set; }
        public virtual FormSections FormsSectionObject { get; set; }

        [ForeignKey("FormsFieldObject")]
        public int? FormFieldId { get; set; }
        public virtual FormFields FormsFieldObject { get; set; }
    }
}
