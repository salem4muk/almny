using almny.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace almny.Repository
{
    public class DatabaseContext : IdentityDbContext<ApplicationUser>
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {

        }

        public virtual DbSet<Subject> Subjects { get; set; }
        public virtual DbSet<Level> Levels { get; set; }
        public virtual DbSet<Department> Departments { get; set; }
        public virtual DbSet<Semester> Semesters { get; set; }
        public virtual DbSet<Video> Videos { get; set; }
        public virtual DbSet<Book> Books { get; set; }
        public virtual DbSet<Like> Likes { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        //private void SeedData(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<Level>().HasData(
        //        new Level { Id = 1, Name = "المستوى الاول" },
        //        new Level { Id = 2, Name = "المستوى الثاني" },
        //        new Level { Id = 3, Name = "المستوى الاول + الثاني" }
        //    );

        //    modelBuilder.Entity<Department>().HasData(
        //        new Department { Id = 1, Name = "القسم العلمي" },
        //        new Department { Id = 2, Name = "القسم الادبي" },
        //        new Department { Id = 3, Name = "القسم العلمي + الادبي" }
        //    );

        //    modelBuilder.Entity<Semester>().HasData(
        //        new Semester { Id = 1, Name = "الفصل الاول" },
        //        new Semester { Id = 2, Name = "الفصل الثاني" },
        //        new Semester { Id = 3, Name = "الفصل الاول + الثاني" }
        //    );
        //}

    }
}