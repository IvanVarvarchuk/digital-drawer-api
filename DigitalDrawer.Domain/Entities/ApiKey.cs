using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalDrawer.Domain.Entities
{
    public class UsersApiKey: BaseAuditableEntity
    {
        public string Name { get; set; }
        public string Key { get; set; }
        public string UserId { get; set; }
    }
}
