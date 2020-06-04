using System;
using NUnit.Framework;

namespace Ebisu.MinimizeGolden
{
    public class MinTest
    {
        public MinTest()
        {
            //Below code is to find the ULP in C#.
            
        }

        private readonly double EPS = 0.0000001;  // minimize-golden-section-1d tests use 32-bit float's ulp

        [Test]
        public void Basic()
        {
            Status status = MinimizeGolden.Min(x =>x * x, -1.0, 1.0, 1e-3, 100);
            double res = status.argmin;
            Assert.AreEqual(0, res, 1e-3);
            Assert.True(status.converged);
        }

        [Test]
        public void Hyperbola()
        {
            // Minimizes 1 / (x - 1) in [0, 2]
            Status status = MinimizeGolden.Min(x => 1.0 / (x - 1.0), 0, 2);
            Assert.AreEqual(1.0d, status.argmin, EPS);
            Assert.True(status.converged);
        }

        [Test]
        public void NegHyperbola()
        {
            // Minimizes -1 / (x - 1) in [0, 2]
            Status status = MinimizeGolden.Min(x => -1.0f / (x - 1.0f), 0, 2);
            Assert.AreEqual(1.0, status.argmin, EPS);
            Assert.True(status.converged);
            Assert.AreEqual(41, status.iterations);
        }

        [Test]
        public void Parabola()
        {
            // Succeeds out on bounded minimization of -x^2
            Status status = MinimizeGolden.Min(x => -x * x, -1, 2);
            Assert.AreEqual(2, status.argmin, EPS);
            Assert.AreEqual(-4, status.minimum, EPS);
            Assert.True(status.converged);
        }

        [Test]
        public void Sqrt()
        {
            // Minimizes sqrt(x) in [0, inf)
            Status status = MinimizeGolden.Min(x => Math.Sqrt(x), 0, 300);
            Assert.True(status.converged);
            Assert.AreEqual(0.0, status.argmin, EPS);
        }

        [Test]
        public void SqrtAbs()
        {
            // Minimizes sqrt(|x|)
            Status status = MinimizeGolden.Min(x => Math.Sqrt(Math.Abs(x)), -3, 3);
            Assert.True(status.converged);
            Assert.AreEqual(0.0, status.argmin, EPS);
            Assert.AreEqual(0.0, status.minimum, 1e-3);
        }

        [Test]
        public void Tol()
        {
            // returns answer if tolerance not met
            Status status = MinimizeGolden.Min(x => x * (x - 2.0), 0, 3, 0, 200);
            Assert.AreEqual(200, status.iterations);
            Assert.AreEqual(-1.0, status.minimum, EPS);
            Assert.AreEqual(1.0, status.argmin, EPS);
            Assert.False(status.converged);
        }

        [Test]
        public void ParabolaEdge()
        {
            // minimizes x(x-2) in [5, 6]
            Status status = MinimizeGolden.Min(x => x * (x - 2.0), 5, 6);
            Assert.True(status.converged);
            Assert.AreEqual(5.0, status.argmin, EPS);
        }

        [Test]
        public void Cubic()
        {
            // minimizes a cubic
            Status status = MinimizeGolden.Min(x => x * (x - 2) * (x - 1), -3, 3);
            Assert.True(status.converged);
            Assert.AreEqual(-3, status.argmin, EPS);
        }

        [Test]
        public void NegCubic()
        {
            // maximizes a cubic
            Status status = MinimizeGolden.Min(x => -x * (x - 2) * (x - 1), 0, 3);
            Assert.True(status.converged);
            Assert.AreEqual(3, status.argmin, EPS);
        }

        [Test]
        public void BoundedCubic()
        {
            // minimizes a cubic against bounds
            Status status = MinimizeGolden.Min(x => x * (x - 2) * (x - 1), 5, 6);
            Assert.True(status.converged);
            Assert.AreEqual(5, status.argmin, EPS);
        }

        [Test]
        public void Cos()
        {
            // minimizes cosine
            Status status = MinimizeGolden.Min(x => Math.Cos(x), -10, 10);
            Assert.True(status.converged);
            Assert.AreEqual(-1, Math.Cos(status.argmin), EPS);
        }

        [Test]
        public void Cusp()
        {
            // minimizes a cusp
            Status status = MinimizeGolden.Min(x => Math.Sqrt(Math.Abs(x - 5)), 0, 10);
            Assert.True(status.converged);
            Assert.AreEqual(5, status.argmin, EPS);
        }
    }
}
