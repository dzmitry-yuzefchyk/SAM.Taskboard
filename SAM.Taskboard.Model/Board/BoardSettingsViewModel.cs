using SAM.Taskboard.DataProvider.Models;
using SAM.Taskboard.Model.Project;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SAM.Taskboard.Model.Board
{
    public class BoardSettingsViewModel
    {
        public int ProjectId { get; set; }

        public string ProjectTitle { get; set; }

        public int BoardId { get; set; }

        [MaxLength(64, ErrorMessage = "Maximum 64 symbols")]
        [Required(ErrorMessage = "Please specify field")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Please specify field")]
        public TaskSettingsRole AccessToDeleteTask { get; set; }

        [Required(ErrorMessage = "Please specify field")]
        public TaskSettingsRole AccessToChangeTask { get; set; }

        [Required(ErrorMessage = "Please specify field")]
        public TaskSettingsRole AccessToCreateTask { get; set; }

        [Required(ErrorMessage = "Please specify field")]
        public TaskSettingsRole AccessToChangeBoard { get; set; }

        public List<BoardInfo> Boards { get; set; }

        public List<ColumnInfo> Columns { get; set; }

        public BoardSettingsViewModel()
        {
            Columns = new List<ColumnInfo>();
            Boards = new List<BoardInfo>();
        }
    }
}
