using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SAM.Taskboard.DataProvider.Models
{
    class Column
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(64)]
        public string Title { get; set; } = "Column";

        public ICollection<Task> Tasks { get; set; }

        public int BoardId { get; set; }

        public Board Board { get; set; }

        public Column()
        {
            Tasks = new List<Task>();
        }
    }
}
