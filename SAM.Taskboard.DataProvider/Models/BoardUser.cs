namespace SAM.Taskboard.DataProvider.Models
{
    public class BoardUser
    {
        public int Id { get; set; }

        public int RoleId { get; set; }

        public Role Role { get; set; }

        public User User { get; set; }

        public Board Board { get; set; }
    }
}
