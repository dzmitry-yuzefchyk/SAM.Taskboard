using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SAM.Taskboard.Model.Task
{
    public class CreateTaskViewModel
    {
        [Required]
        [MaxLength(32, ErrorMessage = "Please specify this field")]
        public string Title { get; set; }

        [MaxLength(1024, ErrorMessage = "Please specify this field")]
        public string Content { get; set; }

        [Required]
        public int Type { get; set; }

        [Required]
        public int Severity { get; set; }

        [Required]
        public int Priority { get; set; }

        public byte[] Attachments { get; set; }

        public IEnumerable<SelectListItem> TeamMembers { get; set; } = new SelectList(new[]
    {
        new { AssigneeId="1", Name="name1" },
        new { AssigneeId="2", Name="name2" },
        new { AssigneeId="3", Name="name3" },
    }, "AssigneeId", "Name", 1);

        public SelectListItem AssigneeId { get; set; }

        public DateTime TimToComplete { get; set; }
    }
}
