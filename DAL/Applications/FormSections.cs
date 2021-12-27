using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Applications
{
    public class FormSections:Base2
    {
        public string Name { get; set; }
        public bool IsRequired { get; set; }
        public int? Order { get; set; }

        [ForeignKey("ApplicationSettingObject")]
        public int ApplicationSettingId { get; set; }
        public virtual ApplicationSetting ApplicationSettingObject { get; set; }
    }
}
