using System;
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

        public byte[] Attachments { get; set; }

        [Required]
        public string Type { get; set; } = "BUG";

        [Required]
        public string Priority { get; set; } = "LOW";

        public string Severity { get; set; } = "LOW";

        public string Asignee { get; set; }

        public TimeSpan TimeToComplete { get; set; }

        public DateTime StartTime { get; set; }

        public int ColumnId { get; set; }

        public Column Column { get; set; }
    }
}
