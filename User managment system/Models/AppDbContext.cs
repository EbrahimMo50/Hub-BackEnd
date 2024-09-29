using Microsoft.EntityFrameworkCore;

namespace User_managment_system.Models
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<UserTask> Tasks { get; set; }

        public AppDbContext(DbContextOptions options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>()
                .HasOne(u => u.Group) // A User can have one Group
                .WithMany(g => g.Users) // A Group can have many Users
                .HasForeignKey(u => u.GroupId) // Foreign key property
                .OnDelete(DeleteBehavior.SetNull); // Allow deleting a Group without deleting Users

            modelBuilder.Entity<Group>().ToTable("groups").HasKey(x => x.Id);
            modelBuilder.Entity<UserTask>().ToTable("tasks").HasKey(x => x.Id);
        }
    }
}
