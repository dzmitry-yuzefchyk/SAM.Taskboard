using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAM.Taskboard.DataProvider.Models
{
    public class Task
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; } = "Task";

        public string Content { get; set; }

        [Required]
        public int Type { get; set; }
        [Required]
        public int Priority { get; set; }

        public int Severity { get; set; }

        public string AssigneeId { get; set; }

        public User Assignee { get; set; }

        public string CreatorId { get; set; }

        public User Creator { get; set; }

        public TimeSpan TimeToComplete { get; set; }

        public DateTime StartTime { get; set; }

        public int ColumnId { get; set; }

        public Column Column { get; set; }

        public ICollection<Attachment> Attachments { get; set; }

        public ICollection<Comment> Comments { get; set; }

        public Task()
        {
            Attachments = new List<Attachment>();
            Comments = new List<Comment>();
        }
    }
}
