using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Projects
{
    public class ProjectFormSubmissionValues:Base
    {
        public string Value { get; set; }

        [ForeignKey("FieldObject")]
        public int SubmissionId { get; set; }
        public virtual ProjectFormSubmissions FieldObject { get; set; }
    }
}
