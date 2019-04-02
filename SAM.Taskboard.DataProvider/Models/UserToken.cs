using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAM.Taskboard.DataProvider.Models
{
    class UserToken
    {
        [Key]
        [ForeignKey("User")]
        public int Id { get; set; }

        [Required]
        public string Token { get; set; }

        public DateTime Created { get; set; }

        public int LifeTime { get; set; } = 30;

        public User User { get; set; }
    }
}
