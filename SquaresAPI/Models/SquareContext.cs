using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SquaresAPI.Models
{
    public class SquareContext : DbContext
    {
        public SquareContext(DbContextOptions<SquareContext> options)
            : base(options)
        {

        }

        public DbSet<Point> Points { get; set; }
    }
}
