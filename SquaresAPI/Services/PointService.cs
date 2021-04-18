using SquaresAPI.Models;
using SquaresAPI.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SquaresAPI.Services
{
    public class PointService : IPointService
    {
        private readonly SquareContext context;

        public PointService(SquareContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Adding Point to context
        /// </summary>
        /// <param name="point">Point object</param>
        /// <returns>Returns inserted Point identifier</returns>
        public int Add(Point point)
        {
            context.Add(point);
            context.SaveChanges();

            return point.Id;
        }

        /// <summary>
        /// Adding a list of points to context
        /// </summary>
        /// <param name="points">List of points</param>
        public void AddRange(Point[] points)
        {
            context.AddRange(points);
            context.SaveChanges();
        }

        /// <summary>
        /// Deleting point from context
        /// </summary>
        /// <param name="id">Point identifier</param>
        /// <returns>Returns "False" if point was not found, "True" otherwise</returns>
        public bool Delete(int id)
        {
            var point = context.Points.Find(id);

            if (point == null)
                return false;

            context.Remove(point);
            context.SaveChanges();

            return true;
        }

        /// <summary>
        /// Removing all points from context
        /// </summary>
        public void Delete()
        {
            var points = context.Points;
            context.RemoveRange(points);
            context.SaveChanges();
        }

        /// <summary>
        /// Getting point from context
        /// </summary>
        /// <param name="id">Point identifier</param>
        /// <returns>Returns Point</returns>
        public Point Get(int id)
        {
            var point = context.Points.Find(id);

            return point;
        }

        /// <summary>
        /// Getting all points from contet
        /// </summary>
        /// <returns>Returns all Points</returns>
        public IEnumerable<Point> GetAll()
        {
            var points = context.Points;

            return points;
        }

        /// <summary>
        /// Getting all squares from list of points
        /// </summary>
        /// <returns>Returns all squares from list of points</returns>
        public IEnumerable<Square> GetSquares()
        {
            var points = context.Points.ToList();
            var squares = new List<Square>();

            if (points.Count < 4)
                return null;

            var potentialSquares = new List<Square>();

            potentialSquares = GetCombinations(points.ToArray(), points.Count, 4, potentialSquares);

            foreach (var potentialSquare in potentialSquares)
            {
                var square = GetSquare(potentialSquare);

                if (square != null)
                    squares.Add(square);
            }

            return squares;
        }

        /// <summary>
        /// Getting all squares from list of points
        /// </summary>
        /// <returns>Returns all squares from list of points</returns>
        public int GetSquaresCount()
        {
            var count = 0;

            var points = context.Points.ToList().GroupBy(x => new {x.CoordinateX, x.CoordinateY}).Select(x => x.First()).ToList();
            var squares = new List<Square>();

            if (points.Count < 4)
                return count;

            var potentialSquares = new List<Square>();

            potentialSquares = GetCombinations(points.ToArray(), points.Count, 4, potentialSquares);

            foreach (var potentialSquare in potentialSquares)
            {
                var square = GetSquare(potentialSquare);

                if (square != null)
                    squares.Add(square);
            }

            return squares.Count;
        }

        /// <summary>
        /// Gets dinstance between two Points
        /// </summary>
        /// <param name="p">Point 1 object</param>
        /// <param name="q">Point 2 object</param>
        /// <returns>Returns distance between two Points</returns>
        private int GetDistance(Point p, Point q)
        {
            return (p.CoordinateX - q.CoordinateX) * (p.CoordinateX - q.CoordinateX) + (p.CoordinateY - q.CoordinateY) * (p.CoordinateY - q.CoordinateY);
        }

        /// <summary>
        /// Gets square object if potential square object is a square
        /// </summary>
        /// <param name="potentialSquare">Potential square object</param>
        /// <returns>Returns square object if potential square object is square, null otherwise</returns>
        private Square GetSquare(Square potentialSquare)
        {
            Point p1 = potentialSquare.Points[0];
            Point p2 = potentialSquare.Points[1];
            Point p3 = potentialSquare.Points[2];
            Point p4 = potentialSquare.Points[3];

            int d2 = GetDistance(p1, p2); // from p1 to p2
            int d3 = GetDistance(p1, p3); // from p1 to p3
            int d4 = GetDistance(p1, p4); // from p1 to p4

            if (d2 == 0 || d3 == 0 || d4 == 0)
                return null;

            // If lengths if (p1, p2) and (p1, p3) are same, then
            // following conditions must met to form a square.
            // 1) Square of length of (p1, p4) is same as twice
            // the square of (p1, p2)
            // 2) Square of length of (p2, p3) is same
            // as twice the square of (p2, p4)
            if (d2 == d3 && 2 * d2 == d4
                && 2 * GetDistance(p2, p4) == GetDistance(p2, p3))
            {
                return new Square()
                {
                    Points = new List<Point>()
                    {
                        p1, p2, p3, p4
                    }
                };
            }

            // The below two cases are similar to above case
            if (d3 == d4 && 2 * d3 == d2
                && 2 * GetDistance(p3, p2) == GetDistance(p3, p4))
            {
                return new Square()
                {
                    Points = new List<Point>()
                    {
                        p1, p2, p3, p4
                    }
                };
            }
            if (d2 == d4 && 2 * d2 == d3
                && 2 * GetDistance(p2, p3) == GetDistance(p2, p4))
            {
                return new Square() { Points = new List<Point>() { p1, p2, p3, p4 }};
            }

            return null;
        }

        /// <summary>
        /// Getting all possible combinations of squares from list of points
        /// </summary>
        /// <param name="arr">Array of points</param>
        /// <param name="data">Temporary array of points</param>
        /// <param name="start">Start index</param>
        /// <param name="end">End index</param>
        /// <param name="index">Current index</param>
        /// <param name="r">Number of items in combination </param>
        /// <param name="potentialSquares">Potential square objects</param>
        /// <returns>Returns all possible combinations of squares from list of points</returns>
        private List<Square> CombinationUtil(Point[] arr, Point[] data,
                                    int start, int end,
                                    int index, int r, List<Square> potentialSquares)
        {
            //Combination found
            if (index == r)
            {
                var potentialSquare = new Square();

                for (int j = 0; j < r; j++)
                    potentialSquare.Points.Add(data[j]);

                potentialSquares.Add(potentialSquare);

                return potentialSquares;
            }

            // replace index with all
            // possible elements. The
            // condition "end-i+1 >=
            // r-index" makes sure that
            // including one element
            // at index will make a
            // combination with remaining
            // elements at remaining positions
            for (int i = start; i <= end &&
                      end - i + 1 >= r - index; i++)
            {
                data[index] = arr[i];
                 potentialSquares = CombinationUtil(arr, data, i + 1,
                                end, index + 1, r, potentialSquares);
            }

            return potentialSquares;
        }

        /// <summary>
        /// Getting all possible combinations of squares from list of points
        /// </summary>
        /// <param name="arr">Array of points</param>
        /// <param name="n">Array length</param>
        /// <param name="r">Number of items in combination</param>
        /// <param name="potentialSquares">Potential square objects</param>
        /// <returns>Returns all possible combinations of squares from list of points</returns>
        private List<Square> GetCombinations(Point[] arr,
                                     int n, int r, List<Square> potentialSquares)
        {
            // A temporary array to store
            // all combination one by one
            Point[] data = new Point[r];

            // Getl all combinations
            // using temprary array 'data[]'
            return CombinationUtil(arr, data, 0,
                            n - 1, 0, r, potentialSquares);
        }
    }
}
