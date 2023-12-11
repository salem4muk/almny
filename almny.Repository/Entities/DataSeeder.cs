using almny.Models;
using almny.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3almny.Repository.Entities
{
    public class DataSeeder
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var databaseContext = new DatabaseContext(
                serviceProvider.GetRequiredService<DbContextOptions<DatabaseContext>>()))
            {
                SeedData(databaseContext);
            }
        }

        private static void SeedData(DatabaseContext databaseContext)
        {
            SeedLevels(databaseContext);
            SeedDepartments(databaseContext);
            SeedSemesters(databaseContext);
        }

        private static void SeedLevels(DatabaseContext databaseContext)
        {
            if (!databaseContext.Levels.Any())
            {
                var levels = new List<Level>()
            {
                new Level { Name = "المستوى الاول" },
                new Level { Name = "المستوى الثاني" },
                new Level { Name = "المستوى الاول + الثاني" }
            };

                databaseContext.Levels.AddRange(levels);
                databaseContext.SaveChanges();
            }
        }

        private static void SeedDepartments(DatabaseContext databaseContext)
        {
            if (!databaseContext.Departments.Any())
            {
                var departments = new List<Department>()
            {
                new Department { Name = "القسم العلمي" },
                new Department { Name = "القسم الادبي" },
                new Department { Name = "القسم العلمي + الادبي" }
            };

                databaseContext.Departments.AddRange(departments);
                databaseContext.SaveChanges();
            }
        }

        private static void SeedSemesters(DatabaseContext databaseContext)
        {
            if (!databaseContext.Semesters.Any())
            {
                var semesters = new List<Semester>()
            {
                new Semester { Name = "الفصل الاول" },
                new Semester { Name = "الفصل الثاني" },
                new Semester { Name = "الفصل الاول + الثاني" }
            };

                databaseContext.Semesters.AddRange(semesters);
                databaseContext.SaveChanges();
            }
        }
    }

}
