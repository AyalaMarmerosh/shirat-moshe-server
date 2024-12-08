using Microsoft.EntityFrameworkCore;
using MonthlyDataApi.Models;
using MonthlyDataApi.Models.MonthlyDataApi.Models;
using System.Collections.Generic;

namespace MonthlyDataApi.Services
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Person> Persons { get; set; }
        public DbSet<MonthlyRecord> MonthlyRecords { get; set; }
    }

}
