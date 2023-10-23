using System.Data.Entity;

namespace Helper
{
    public class LibraryContext : DbContext
    {
        // Your context has been configured to use a 'LibraryContext' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'MusicWinForms.LibraryContext' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'LibraryContext' 
        // connection string in the application configuration file.
        public LibraryContext()
            : base("name=LibraryContext")
        {
        }

        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

        public virtual DbSet<Library> Libraries { get; set; }
        public virtual DbSet<Track> Tracks { get; set; }
        public virtual DbSet<Playlist> Playlists { get; set; }

        static LibraryContext()
        {
            //Database.SetInitializer(new DropCreateDatabaseIfModelChanges<LibraryContext>());
            //if (drop)
            Database.SetInitializer(new DropCreateDatabaseAlways<LibraryContext>());
        }

    }

    //public class MyEntity
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //}
}