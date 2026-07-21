namespace TaskManagementSystem.Web.Models
{
    public class NotificationViewModel
    {
        public int NotificationId { get; set; }
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
        public bool IsRead { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}