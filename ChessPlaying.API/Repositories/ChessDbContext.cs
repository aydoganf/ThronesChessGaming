using ChessPlaying.API.Model.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChessPlaying.API.Repositories
{
    public class ChessDbContext : DbContext
    {
        public DbSet<Session> Sessions { get; set; }

        public ChessDbContext(DbContextOptions<ChessDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Session>().HasKey(s => s.Id);
        }
    }
}
