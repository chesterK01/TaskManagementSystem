using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagementSystem.Service.DTOs.TaskHistory
{
    public class TaskHistoryDto
    {
        public byte OldStatus { get; set; }
        public string OldStatusName { get; set; } = null!;
        public byte NewStatus { get; set; }
        public string NewStatusName { get; set; } = null!;
        public string ChangedByName { get; set; } = null!;
        public DateTime ChangedDate { get; set; }
    }
}
