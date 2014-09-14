using System.Data.Entity;

namespace WolfTheatres.Database
{
    public class WolfTheatresContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, add the following
        // code to the Application_Start method in your Global.asax file.
        // Note: this will destroy and re-create your database with every model change.
        // 
        // System.Data.Entity.Database.SetInitializer(new System.Data.Entity.DropCreateDatabaseIfModelChanges<AngularTutorial.Models.ToDoContext>());

        public WolfTheatresContext()
            : base("name=WolfTheatresContext")
        {
            System.Data.Entity.Database.SetInitializer<WolfTheatresContext>(null);
        }

        public DbSet<Movie> Movies { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<EmployeeShift> EmployeeShifts { get; set; }
        public DbSet<MovieShowtime> MovieShowtimes { get; set; }
        public DbSet<Poster> Posters { get; set; }
        public DbSet<Trailer> Trailers { get; set; }
        public DbSet<Image> Images { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

        }
    }
}
