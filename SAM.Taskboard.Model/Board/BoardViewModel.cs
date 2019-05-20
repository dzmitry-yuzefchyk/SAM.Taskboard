using System;
using System.Collections.Generic;

namespace SAM.Taskboard.Model.Board
{
    public class BoardViewModel
    {
        public int BoardId { get; set; }
        public string Title { get; set; }
        public List<ColumnInfo> Columns { get; set; }
        public bool CanUserChangeProject { get; set; }
        public bool CanUserCreateTask { get; set; }
    }

    public class ColumnInfo
    {
        public string Title { get; set; }
        public List<TaskInfo> Tasks { get; set; }
    }

    public class TaskInfo
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime CreationTime { get; set; }
    }
}
