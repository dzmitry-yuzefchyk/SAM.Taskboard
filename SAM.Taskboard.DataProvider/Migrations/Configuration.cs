namespace SAM.Taskboard.DataProvider.Migrations
{
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<SAM.Taskboard.DataProvider.TaskboardContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(SAM.Taskboard.DataProvider.TaskboardContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
            //Role administrator = new Role { Id = 0, RoleType = "ADMINISTRATOR" };
            //Role creator = new Role { Id = 1, RoleType = "CREATOR" };
            //Role user = new Role { Id = 2, RoleType = "USER" };
            //Role viewer = new Role { Id = 3, RoleType = "VIEWER" };

            //context.Roles.AddOrUpdate(administrator);
            //context.Roles.AddOrUpdate(creator);
            //context.Roles.AddOrUpdate(user);
            //context.Roles.AddOrUpdate(viewer);
        }
    }
}
