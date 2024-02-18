using EloSwissMatchMaking.DBContext.DTO;
using EloSwissMatchMaking.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EloSwissMatchMaking.DBContext
{
    public class PlayerDBContext : DbContext
    {
        public PlayerDBContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<PlayerDTO> Players { get; set; }
    }
}
