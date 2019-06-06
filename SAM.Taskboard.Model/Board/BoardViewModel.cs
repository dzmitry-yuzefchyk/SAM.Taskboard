using System;
using System.Collections.Generic;

namespace SAM.Taskboard.Model.Board
{
    public class BoardViewModel
    {
        public int ProjectId { get; set; }
        public string ProjectTitle { get; set; }
        public int BoardId { get; set; }
        public string Title { get; set; }
        public List<ColumnInfo> Columns { get; set; }
        public bool CanUserChangeBoard { get; set; }
        public bool CanUserCreateTask { get; set; }
    }

    public class ColumnInfo
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public List<TaskInfo> Tasks { get; set; }
    }

    public class TaskInfo
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string AssigneeEmail { get; set; }
        public string AssigneeIcon { get; set; }
        public string CreatorEmail { get; set; }
        public string CreatorIcon { get; set; }
        public bool CanUserChangeTask { get; set; }
        public Priority Priority { get; set; }
        public Severity Severity { get; set; }
        public TaskType Type { get; set; }
        public DateTime CreationTime { get; set; }
    }
}
