namespace SAM.Taskboard.DataProvider.Models
{
    public class TeamUser
    {
        public int Id { get; set; }

        public int RoleId { get; set; }

        public Role Rolde { get; set; }

        public Team Team { get; set; }

        public User User { get; set; }
    }
}
