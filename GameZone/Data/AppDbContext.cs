


namespace GameZone.Data
{
    public class AppDbContext : IdentityDbContext<IdentityUser>
    {
        public DbSet<Game> Games { get; set; }
        public DbSet<Device> Devices { get; set; }
        public DbSet<GameDevice> GameDevices { get; set; }
        public DbSet<Category> Categories { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Category>()
                .HasData(new Category[]
                {
                    new Category { Id = 1, Name = "Action" },
                    new Category { Id = 2, Name = "Adventure" },
                    new Category { Id = 3, Name = "RPG" },
                    new Category { Id = 4, Name = "Simulation" },
                    new Category { Id = 5, Name = "Strategy" }
                });

            modelBuilder.Entity<Device>()
                .HasData(new Device[]
                {
                    new Device { Id = 1, Name = "PC", Icon = "fas fa-desktop" },
                    new Device { Id = 2, Name = "PlayStation", Icon = "fab fa-playstation" },
                    new Device { Id = 3, Name = "Xbox", Icon = "fab fa-xbox" },
                    new Device { Id = 4, Name = "Nintendo", Icon = "fas fa-gamepad" }
                });

            


            modelBuilder.Entity<GameDevice>()
                .HasKey(gd => new { gd.GameId, gd.DeviceId });
        }
    }
}
