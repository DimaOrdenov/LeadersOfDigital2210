using System;
using Dal.Entities;
using Microsoft.EntityFrameworkCore;

namespace Dal
{
    public class AppDbContext : DbContext
    {
        public DbSet<Weather> Weathers { get; set; }

        public AppDbContext(DbContextOptions options) 
            : base(options)
        {
        }
    }
}
