using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalDrawer.Domain.Entities
{
    public class FileConversion : BaseAuditableEntity
    {
        public string UserId { get; set; }
        public string OriginalFileName { get; set; }
        public string ConvertedFileName { get; set; }
        public byte[] ConvertedFileContent { get; set; }
        public string? FileLink { get; set; }
        public TargetFileFormat FileFormat { get; set; }
        public string? DeletionJobId { get; set; }
        public DateTime? DeletionDate { get; set; }
    }

}
