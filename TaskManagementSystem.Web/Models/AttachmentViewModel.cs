namespace TaskManagementSystem.Web.Models
{
    public class AttachmentViewModel
    {
        public int AttachmentId { get; set; }
        public string FileName { get; set; } = null!;
        public string FilePath { get; set; } = null!;
        public string UploadedByName { get; set; } = null!;
        public DateTime UploadedDate { get; set; }
    }
}
