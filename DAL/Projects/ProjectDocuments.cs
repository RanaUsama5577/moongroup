using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Entities.Enum;

namespace DAL.Projects
{
    public class ProjectDocuments:Base
    {
        public string DocumentUrl { get; set; }
        public string Name { get; set; }
        public string FileSize { get; set; }
        public DocumentType Type { get; set; }

        [ForeignKey("SettingObject")]
        public int ProjectSettingId { get; set; }
        public virtual ProjectsSettings SettingObject { get; set; }

        [ForeignKey("UserObject")]
        public string UserId { get; set; }
        public virtual ApplicationUser UserObject { get; set; }
    }
}
