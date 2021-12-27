using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Entities.Enum;

namespace DAL.Applications
{
    public class FormFieldsOptions:Base2
    {
        public string Text { get; set; }
        public string Value { get; set; }
        public string HelpText { get; set; }
        public int? Order { get; set; }
        public FormFieldOptionType Type { get; set; }

        [ForeignKey("FormsFieldObject")]
        public int FormsFieldId { get; set; }
        public virtual FormFields FormsFieldObject { get; set; }
    }
}
