using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EloSwissMatchMaking.DBContext
{
    public class DBContextFactory
    {
        private readonly string _connectionString;

        public DBContextFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public PlayerDBContext CreateDbContext()
        {
            DbContextOptions options = new DbContextOptionsBuilder().UseSqlite(_connectionString).Options;
            return new PlayerDBContext(options);
        }
    }
}
