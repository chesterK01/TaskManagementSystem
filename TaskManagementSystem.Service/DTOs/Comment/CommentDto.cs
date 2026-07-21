using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagementSystem.Service.DTOs.Comment
{
    public class CommentDto
    {
        public int CommentId { get; set; }
        public int TaskId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; } = null!;
        public string Content { get; set; } = null!;
        public DateTime CreatedDate { get; set; }
    }
}
