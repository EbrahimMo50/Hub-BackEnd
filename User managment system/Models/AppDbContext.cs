using Microsoft.EntityFrameworkCore;

namespace User_managment_system.Models
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Group> Groups { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>().ToTable("users").HasKey(x => x.Id);
            modelBuilder.Entity<Group>().ToTable("groups").HasKey(x => x.Id);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\Ebrahim.Mohsen\\Documents\\UserDb.mdf;Integrated Security=True;Connect Timeout=30;Encrypt=True");
        }
    }
}
