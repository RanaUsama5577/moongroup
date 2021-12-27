using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Applications
{
    public class FormFieldOptionBinder:Base2
    {
        [ForeignKey("FormsFieldObject")]
        public int FormsFieldId { get; set; }
        public virtual FormFields FormsFieldObject { get; set; }

        [ForeignKey("FormsFieldOptionObject")]
        public int FormsFieldOptionId { get; set; }
        public virtual FormFieldsOptions FormsFieldOptionObject { get; set; }
    }
}
