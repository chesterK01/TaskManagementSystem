using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagementSystem.Service.DTOs.Common
{
    public enum ProjectStatus : byte
    {
        NotStarted = 0,
        InProgress = 1,
        Completed = 2,
        OnHold = 3
    }

    public enum TaskPriority : byte
    {
        Low = 0,
        Medium = 1,
        High = 2,
        Urgent = 3
    }

    public enum ProjectTaskStatus : byte
    {
        ToDo = 0,
        InProgress = 1,
        InReview = 2,
        Done = 3
    }

    public static class StatusLabels
    {
        public static string ProjectStatusName(ProjectStatus status) => status switch
        {
            ProjectStatus.NotStarted => "Chưa bắt đầu",
            ProjectStatus.InProgress => "Đang thực hiện",
            ProjectStatus.Completed => "Hoàn thành",
            ProjectStatus.OnHold => "Tạm dừng",
            _ => status.ToString()
        };

        public static string TaskPriorityName(TaskPriority priority) => priority switch
        {
            TaskPriority.Low => "Thấp",
            TaskPriority.Medium => "Trung bình",
            TaskPriority.High => "Cao",
            TaskPriority.Urgent => "Khẩn cấp",
            _ => priority.ToString()
        };

        public static string TaskStatusName(ProjectTaskStatus status) => status switch
        {
            ProjectTaskStatus.ToDo => "Chưa làm",
            ProjectTaskStatus.InProgress => "Đang làm",
            ProjectTaskStatus.InReview => "Đang review",
            ProjectTaskStatus.Done => "Hoàn thành",
            _ => status.ToString()
        };
    }
}
