using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagementSystem.Service.DTOs.Notification
{
    public class NotificationDto
    {
        public int NotificationId { get; set; }
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
        public bool IsRead { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
