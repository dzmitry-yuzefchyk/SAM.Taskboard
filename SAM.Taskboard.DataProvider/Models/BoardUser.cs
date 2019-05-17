namespace SAM.Taskboard.DataProvider.Models
{
    public class BoardUser
    {
        public int Id { get; set; }

        public int Role { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }

        public int BoardId { get; set; }
        public Board Board { get; set; }
    }
}
