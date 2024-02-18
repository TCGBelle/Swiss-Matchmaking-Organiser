using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EloSwissMatchMaking.DBContext.DTO
{
    public class PlayerDTO
    {

        [Key] public int Id { get; set; }

        public string Name { get; set; }

        public int ELO { get; set; }
    }
}
