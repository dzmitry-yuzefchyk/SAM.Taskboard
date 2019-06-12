using System;

namespace SAM.Taskboard.DataProvider.Models
{
    public class Comment
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public int TaskId { get; set; }

        public Task Task { get; set; }

        public string CreatorId { get; set; }

        public User Creator { get; set; }

        public DateTime CreationDate { get; set; }
    }
}
