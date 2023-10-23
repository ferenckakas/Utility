using System.Data.Entity;

namespace Helper
{
    public class MusicModel : DbContext
    {
        // Your context has been configured to use a 'MusicModel' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'MusicWinForms.MusicModel' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'MusicModel' 
        // connection string in the application configuration file.
        public MusicModel()
            : base("name=MusicModel")
        {
        }

        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

        public virtual DbSet<Library> Libraries { get; set; }
        public virtual DbSet<Track> Tracks { get; set; }
        public virtual DbSet<Playlist> Playlists { get; set; }

        static MusicModel()
        {
            //Database.SetInitializer(new DropCreateDatabaseIfModelChanges<MusicModel>());
            //if (drop)
            //Database.SetInitializer(new DropCreateDatabaseAlways<MusicModel>());
        }

    }

    //public class MyEntity
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //}
}