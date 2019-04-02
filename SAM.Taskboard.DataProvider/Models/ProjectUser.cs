namespace SAM.Taskboard.DataProvider.Models
{
    class ProjectUser
    {
        public int Id { get; set; }

        public int ActivityId { get; set; }

        public Activity Activity { get; set; }

        public Project Project { get; set; }

        public User User { get; set; }
    }
}
