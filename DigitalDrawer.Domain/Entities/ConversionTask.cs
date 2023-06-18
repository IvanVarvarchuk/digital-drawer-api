using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalDrawer.Domain.Entities
{
    public class ConversionTask: BaseAuditableEntity
    {
        public ConversionTask()
        {
            Files = new HashSet<ConversionFile>();
        }

        public ICollection<ConversionFile> Files { get; set; }
    }
}
