using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalDrawer.Domain.Entities
{
    public class FileConversion : BaseAuditableEntity
    {
        public string UserId { get; set; } = String.Empty;
        public string OriginalFileName { get; set; } = String.Empty;
        public string ConvertedFileName { get; set; } = String.Empty;
        public byte[] ConvertedFileContent { get; set; } = Array.Empty<byte>();
        public string? FileLink { get; set; }
        public TargetFileFormat FileFormat { get; set; }
        public string? DeletionJobId { get; set; }
        public DateTime? Deletiondate { get; set; }
    }

}
