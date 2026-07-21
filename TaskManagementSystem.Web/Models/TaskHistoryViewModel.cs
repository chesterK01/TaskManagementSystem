namespace TaskManagementSystem.Web.Models
{
    public class TaskHistoryViewModel
    {
        public string OldStatusName { get; set; } = null!;
        public string NewStatusName { get; set; } = null!;
        public string ChangedByName { get; set; } = null!;
        public DateTime ChangedDate { get; set; }
    }
}