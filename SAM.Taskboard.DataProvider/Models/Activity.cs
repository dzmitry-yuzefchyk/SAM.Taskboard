using System;
using System.ComponentModel.DataAnnotations;

namespace SAM.Taskboard.DataProvider.Models
{
    public class Activity
    {
        public int Id { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public int ProjectId { get; set; }

        public Project Project { get; set; }
    }
}
