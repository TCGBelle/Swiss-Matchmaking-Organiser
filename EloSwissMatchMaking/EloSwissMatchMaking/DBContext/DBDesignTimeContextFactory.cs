using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EloSwissMatchMaking.DBContext
{
    public class DBDesignTimeContextFactory : IDesignTimeDbContextFactory<PlayerDBContext>
    {
        public PlayerDBContext CreateDbContext(string[] args)
        {
            DbContextOptions options = new DbContextOptionsBuilder().UseSqlite("Data Source=ELO.db").Options;
            return new PlayerDBContext(options);
        }


    }
}
