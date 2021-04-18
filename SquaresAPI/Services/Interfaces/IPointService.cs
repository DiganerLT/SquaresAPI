using SquaresAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SquaresAPI.Services.Interfaces
{
    public interface IPointService
    {
        int Add(Point point);

        void AddRange(Point[] points);

        Point Get(int id);

        IEnumerable<Point> GetAll();

        bool Delete(int id);

        void Delete();

        IEnumerable<Square> GetSquares();

        int GetSquaresCount();
    }
}
