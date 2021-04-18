using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SquaresAPI.Controllers;
using SquaresAPI.Models;
using SquaresAPI.Services;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SquaresAPI.Test
{
    public class SquaresAPITest
    {
        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void TestGetPoints_WithNoRecords_ReturnNoContent()
        {
            //Arrange
            DbContextOptionsBuilder<SquareContext> optionsBuilder = new();
            optionsBuilder.UseInMemoryDatabase(MethodBase.GetCurrentMethod().Name);

            IActionResult result;

            using (SquareContext ctx = new(optionsBuilder.Options))
            {
                var pointService = new PointService(ctx);

                var controller = new SquareController(pointService);

                //Act
                result = controller.GetPoints();
            }

            var noContentResult = result as NoContentResult;

            //Assert
            Assert.IsNotNull(noContentResult);
        }

        [Test]
        public void TestGetPoints_WithSingleRecord_ReturnOk()
        {
            //Arrange
            DbContextOptionsBuilder<SquareContext> optionsBuilder = new();
            optionsBuilder.UseInMemoryDatabase(MethodBase.GetCurrentMethod().Name);

            using (SquareContext ctx = new(optionsBuilder.Options))
            {
                ctx.Add(new Point { CoordinateX = 10, CoordinateY = 20 });
                ctx.SaveChanges();
            }

            IActionResult result;

            using (SquareContext ctx = new(optionsBuilder.Options))
            {
                var pointService = new PointService(ctx);

                var controller = new SquareController(pointService);

                //Act
                result = controller.GetPoints() ;
            }

            var okResult = result as OkObjectResult;
            var points = (List<Point>)okResult.Value;
            var singlePoint = points.Single();

            //Assert
            Assert.IsNotNull(okResult);
            Assert.AreEqual(10, singlePoint.CoordinateX);
            Assert.AreEqual(20, singlePoint.CoordinateY);
        }

        [Test]
        public void TestGetSquaresCount_WhenSingleSquare_Return1()
        {
            //Arrange
            DbContextOptionsBuilder<SquareContext> optionsBuilder = new();
            optionsBuilder.UseInMemoryDatabase(MethodBase.GetCurrentMethod().Name);

            using (SquareContext ctx = new(optionsBuilder.Options))
            {
                var point1 = new Point { CoordinateX = -1, CoordinateY = 1 };
                var point2 = new Point { CoordinateX = 1, CoordinateY = 1 };
                var point3 = new Point { CoordinateX = 1, CoordinateY = -1 };
                var point4 = new Point { CoordinateX = -1, CoordinateY = -1 };

                ctx.AddRange(new Point[] { point1, point2, point3, point4 });
                ctx.SaveChanges();
            }

            ActionResult<int> result;

            using (SquareContext ctx = new(optionsBuilder.Options))
            {
                var pointService = new PointService(ctx);

                var controller = new SquareController(pointService);

                //Act
                result = controller.GetSquaresCount();
            }

            var count = result.Value;

            //Assert
            Assert.AreEqual(1, count);
        }

        [Test]
        public void TestGetSquaresCount_When2Squares_Return2()
        {
            //Arrange
            DbContextOptionsBuilder<SquareContext> optionsBuilder = new();
            optionsBuilder.UseInMemoryDatabase(MethodBase.GetCurrentMethod().Name);

            using (SquareContext ctx = new(optionsBuilder.Options))
            {
                var s1_point1 = new Point { CoordinateX = -1, CoordinateY = 1 };
                var s1_point2 = new Point { CoordinateX = 1, CoordinateY = 1 };
                var s1_point3 = new Point { CoordinateX = 1, CoordinateY = -1 };
                var s1_point4 = new Point { CoordinateX = -1, CoordinateY = -1 };

                var s2_point1 = new Point { CoordinateX = 0, CoordinateY = 1 };
                var s2_point2 = new Point { CoordinateX = 1, CoordinateY = 1 };
                var s2_point3 = new Point { CoordinateX = 1, CoordinateY = 0 };
                var s2_point4 = new Point { CoordinateX = 0, CoordinateY = 0 };


                ctx.AddRange(new Point[] { s1_point1, s1_point2, s1_point3, s1_point4 });
                ctx.AddRange(new Point[] { s2_point1, s2_point2, s2_point3, s2_point4 });
                ctx.SaveChanges();
            }

            ActionResult<int> result;

            using (SquareContext ctx = new(optionsBuilder.Options))
            {
                var pointService = new PointService(ctx);

                var controller = new SquareController(pointService);

                //Act
                result = controller.GetSquaresCount();
            }

            var count = result.Value;

            //Assert
            Assert.AreEqual(2, count);
        }

        [Test]
        public void TestGetSquaresCount_WhenNoSquares_Return0()
        {
            //Arrange
            DbContextOptionsBuilder<SquareContext> optionsBuilder = new();
            optionsBuilder.UseInMemoryDatabase(MethodBase.GetCurrentMethod().Name);

            using (SquareContext ctx = new(optionsBuilder.Options))
            {
                var s1_point1 = new Point { CoordinateX = 0, CoordinateY = 3 };
                var s1_point2 = new Point { CoordinateX = 1, CoordinateY = 2 };
                var s1_point3 = new Point { CoordinateX = 3, CoordinateY = 0 };
                var s1_point4 = new Point { CoordinateX = 0, CoordinateY = 0 };

                var s2_point1 = new Point { CoordinateX = -1, CoordinateY = 1 };
                var s2_point2 = new Point { CoordinateX = 1, CoordinateY = 1 };
                var s2_point3 = new Point { CoordinateX = 1, CoordinateY = -2 };
                var s2_point4 = new Point { CoordinateX = -1, CoordinateY = -1 };


                ctx.AddRange(new Point[] { s1_point1, s1_point2, s1_point3, s1_point4 });
                ctx.AddRange(new Point[] { s2_point1, s2_point2, s2_point3, s2_point4 });
                ctx.SaveChanges();
            }

            ActionResult<int> result;

            using (SquareContext ctx = new(optionsBuilder.Options))
            {
                var pointService = new PointService(ctx);

                var controller = new SquareController(pointService);

                //Act
                result = controller.GetSquaresCount();
            }

            var count = result.Value;

            //Assert
            Assert.AreEqual(0, count);
        }
    }
}