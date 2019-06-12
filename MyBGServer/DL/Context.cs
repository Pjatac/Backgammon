using Microsoft.EntityFrameworkCore;
using MyBGServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBGServer.DL
{
	public class Context : DbContext
	{
		public DbSet<User> Users { get; set; }
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlite("Data Source=usersdb.db");
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<User>();
		}
	}
}
