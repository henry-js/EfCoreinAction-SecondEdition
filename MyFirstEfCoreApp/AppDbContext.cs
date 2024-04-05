using Microsoft.EntityFrameworkCore;

namespace MyFirstEfCoreApp
{
    public class AppDbContext : DbContext
    {
        private const string ConnectionString = //#A
            @"Data Source=MyFirstEfCoreApp.db";

        public DbSet<Book> Books { get; set; }

        protected override void OnConfiguring(
            DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(ConnectionString); //#B
        }
    }

    /********************************************************
    #A The connection string is used by the SQL Server database provider to find the database
    #B Using the SQL Server database provider’s UseSqlServer command sets up the options ready for creating the applications’s DBContext
     ********************************************************/
}