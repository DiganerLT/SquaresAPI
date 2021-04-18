using Microsoft.AspNetCore.Mvc;
using SquaresAPI.Models;
using SquaresAPI.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace SquaresAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SquareController : ControllerBase
    {
        private readonly IPointService pointService;

        public SquareController(IPointService pointService)
        {
            this.pointService = pointService;
        }

        // GET: api/Square/GetPoints
        [HttpGet]
        public IActionResult GetPoints()
        {
            var points = pointService.GetAll().ToList();

            if (!points.Any())
                return NoContent();

            return Ok(points);
        }

        // GET api/Square/GetPoint/1
        [HttpGet("{id}")]
        public ActionResult<Point> GetPoint(int id)
        {
            var point = pointService.Get(id);

            if (point == null)
                return NotFound();

            return point;
        }

        // POST api/Square/CreatePoint
        [HttpPost]
        public ActionResult<Point> CreatePoint([FromBody] Point point)
        {
            point.Id = pointService.Add(point);

            return CreatedAtAction(nameof(GetPoints), new { id = point.Id}, point);
        }

        // POST api/Square/CreatePoints
        [HttpPost]
        public ActionResult<Point> CreatePoints([FromBody] Point[] points)
        {
            pointService.AddRange(points);

            return CreatedAtAction(nameof(GetPoints), points);
        }

        // DELETE api/Square/Delete/1
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var success = pointService.Delete(id);

            if (success)
                return Ok();
            else
                return NotFound();
        }

        // GET: api/Square/GetSquares
        [HttpGet]
        public ActionResult<List<Square>> GetSquares()
        {
            var squares = pointService.GetSquares();

            if (squares == null || !squares.Any())
                return NoContent();

            return squares.ToList();
        }

        // GET: api/Square/GetSquaresCount
        [HttpGet]
        public ActionResult<int> GetSquaresCount()
        {
            var count = pointService.GetSquaresCount();

            return count;
        }
    }
}
