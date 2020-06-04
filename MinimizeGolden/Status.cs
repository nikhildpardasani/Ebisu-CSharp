using System;
namespace Ebisu.MinimizeGolden
{
    public class Status
    {
        public int iterations;
        public double argmin;
        public double minimum;
        public bool converged;

        public Status(int iterations, double argmin, double minimum, bool converged)
        {
            this.iterations = iterations;
            this.argmin = argmin;
            this.minimum = minimum;
            this.converged = converged;
        }
    }
}
