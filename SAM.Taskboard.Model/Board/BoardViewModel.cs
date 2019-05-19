using System;
using System.Collections.Generic;

namespace SAM.Taskboard.Model.Board
{
    public class BoardViewModel
    {
        public string Title { get; set; }
        public List<Column> Columns { get; set; }
    }

    public class Column
    {
        public string Title { get; set; }
        public List<Task> Tasks { get; set; }
    }

    public class Task
    {
        public string Title { get; set; }
        public DateTime StartTime { get; set; }
    }
}
