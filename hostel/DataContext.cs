using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Hostel
{
    class DataContext : DbContext 
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) 
        {
            optionsBuilder.UseSqlite("Data Source = UserData.db");
        }
        
        public DbSet<Student> Students { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Privilege> Privileges { get; set; }
    }
}
