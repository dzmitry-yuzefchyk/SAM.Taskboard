using System;
using System.ComponentModel.DataAnnotations;

namespace SAM.Taskboard.DataProvider.Models
{
    class Activity
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(256)]
        public string Description { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public int ProjectId { get; set; }

        public Project Project { get; set; }
    }
}
