using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SAM.Taskboard.Model.Task
{
    public class CreateTaskViewModel
    {
        public int BoardId { get; set; }

        public int ColumnId { get; set; }

        [Required(ErrorMessage = "Please specify this field")]
        [MaxLength(32, ErrorMessage = "Maximum length is 32 symbols")]
        public string Title { get; set; }

        [MaxLength(1024, ErrorMessage = "Maximum length is 1024 symbols")]
        public string Content { get; set; }

        [Required]
        public TaskType Type { get; set; }

        [Required]
        public Severity Severity { get; set; }

        [Required]
        public Priority Priority { get; set; }

        public List<byte[]> Attachments { get; set; }

        public IEnumerable<SelectListItem> TeamMembers { get; set; } = new SelectList(new[]
            {
                new { AssigneeId="1", Name="name1" },
                new { AssigneeId="2", Name="name2" },
                new { AssigneeId="3", Name="name3" },
            }, "AssigneeId", "Name", 1);

        public string AssigneeEmail { get; set; }
    }
}
