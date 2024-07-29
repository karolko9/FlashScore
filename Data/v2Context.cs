using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using v2.Models;

namespace v2.Data
{
    public class v2Context : DbContext
    {
        public v2Context (DbContextOptions<v2Context> options)
            : base(options)
        {
        }

        public DbSet<v2.Models.Team> Team { get; set; } = default!;
        public DbSet<v2.Models.Coach> Coaches { get; set; } = default!;
        public DbSet<v2.Models.Player> Players { get; set; } = default!;
        public DbSet<v2.Models.Match> Matches { get; set; } = default!;


        
    }
}
