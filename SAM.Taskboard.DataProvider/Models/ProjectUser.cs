namespace SAM.Taskboard.DataProvider.Models
{
    public class ProjectUser
    {
        public int Id { get; set; }

        public int ActivityId { get; set; }

        public Activity Activity { get; set; }

        public int RoleId { get; set; }

        public Role Rolde { get; set; }

        public Project Project { get; set; }

        public User User { get; set; }
    }
}
