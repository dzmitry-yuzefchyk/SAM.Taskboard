using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAM.Taskboard.DataProvider.Models
{
    public class Task
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(64)]
        public string Title { get; set; } = "Task";

        [MaxLength(1024)]
        public string Content { get; set; }

        [MaxLength(3096)]
        public byte[] Attachments { get; set; }

        [Required]
        [MaxLength(11)]
        [RegularExpression(@"BUG|FEATURE|IMPROVEMENT")]
        public string Type { get; set; } = "BUG";

        [Required]
        [MaxLength(6)]
        [RegularExpression(@"LOW|MEDIUM|HIGH|URGENT")]
        public string Priority { get; set; } = "LOW";

        [MaxLength(8)]
        [RegularExpression(@"BLOCKER|CRITICAL|NORMAL|LOW")]
        public string Severity { get; set; } = "LOW";

        [MaxLength(64)]
        public string Asignee { get; set; }

        [Column(TypeName = "Time")]
        public DateTime TimeToComplete { get; set; }

        [Column(TypeName = "Timestamp")]
        public DateTime StartTime { get; set; }

        public int ColumnId { get; set; }

        public Column Column { get; set; }
    }
}
