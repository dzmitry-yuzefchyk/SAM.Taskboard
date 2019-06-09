using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace SAM.Taskboard.Model.Task
{
    public class CreateTaskViewModel
    {
        public int ProjectId { get; set; }

        public int BoardId { get; set; }

        public int ColumnId { get; set; }

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

        public List<HttpPostedFileBase> Attachments { get; set; }

        public string AssigneeId { get; set; }

        public CreateTaskViewModel()
        {
            Attachments = new List<HttpPostedFileBase>();
        }
    }
}
