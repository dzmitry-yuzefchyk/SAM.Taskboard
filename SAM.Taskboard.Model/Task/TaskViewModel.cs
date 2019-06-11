using SAM.Taskboard.Model.Project;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace SAM.Taskboard.Model.Task
{
    public class TaskViewModel
    {
        public string ProjectTitle { get; set; }

        public int ProjectId { get; set; }

        public string BoardTitle { get; set; }

        public int BoardId { get; set; }

        public int ColumnId { get; set; }

        public int TaskId { get; set; }

        public List<BoardInfo> Boards { get; set; }

        [Required(ErrorMessage = "Please specify this field")]
        [MaxLength(64, ErrorMessage = "Maximum length is 64 symbols")]
        public string Title { get; set; }

        [MaxLength(1024, ErrorMessage = "Maximum length is 1024 symbols")]
        public string Content { get; set; }

        [Required]
        public TaskType Type { get; set; }

        [Required]
        public Severity Severity { get; set; }

        [Required]
        public Priority Priority { get; set; }

        public List<AttachmentInfo> Attachments { get; set; }

        public string AssigneeId { get; set; }

        public string AssigneeEmail { get; set; }

        public string AssigneeIcon { get; set; }

        public string CreatorEmail { get; set; }

        public string CreatorIcon { get; set; }

        public bool CanUserDeleteTask { get; set; }

        public bool CanUserChangeTask { get; set; }

        public TaskViewModel()
        {
            Attachments = new List<AttachmentInfo>();
        }
    }

    public class AttachmentInfo
    {
        public int Id { get; set; }
        public byte[] Document { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public string FileExtension { get; set; }
    }
}
