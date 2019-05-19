using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SAM.Taskboard.DataProvider.Models
{
    public class Column
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; } = "Column";

        public ICollection<Task> Tasks { get; set; }
        public int Position { get; set; }

        public int BoardId { get; set; }

        public Board Board { get; set; }

        public Column()
        {
            Tasks = new List<Task>();
        }
    }
}
