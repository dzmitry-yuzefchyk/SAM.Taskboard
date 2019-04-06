using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SAM.Taskboard.DataProvider.Models
{
    public class Board
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; } = "Boad";

        public ICollection<Column> Columns { get; set; }

        public int ProjectId { get; set; }

        public Project Project { get; set; }

        public BoardSettings Settings { get; set; }

        public ICollection<BoardUser> BoardUser { get; set; }

        public Board()
        {
            Columns = new List<Column>();
            BoardUser = new List<BoardUser>();
        }
    }
}
