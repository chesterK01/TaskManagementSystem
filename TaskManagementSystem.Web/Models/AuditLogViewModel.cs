namespace TaskManagementSystem.Web.Models
{
    public class AuditLogViewModel
    {
        public string? UserName { get; set; }
        public string TableName { get; set; } = null!;
        public int? RecordId { get; set; }
        public string Action { get; set; } = null!;
        public DateTime CreatedDate { get; set; }
    }
}
