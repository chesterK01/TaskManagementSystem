using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagementSystem.Service.DTOs.Attachment
{
    public class AttachmentDto
    {
        public int AttachmentId { get; set; }
        public int TaskId { get; set; }
        public string FileName { get; set; } = null!;
        public string FilePath { get; set; } = null!;
        public string UploadedByName { get; set; } = null!;
        public DateTime UploadedDate { get; set; }
    }
}
