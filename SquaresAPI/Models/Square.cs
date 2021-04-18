using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SquaresAPI.Models
{
    public class Square
    {
        public Square()
        {
            Points = new List<Point>();
        }

        public List<Point> Points { get; set; }
    }
}
