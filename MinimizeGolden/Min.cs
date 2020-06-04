using System;
namespace Ebisu.MinimizeGolden
{
    public class MinimizeGolden
    {
        private static double PHI_RATIO = Math.Round(2 / (1 + Math.Sqrt(5)), 6);
        public MinimizeGolden()
        {
        }

        public static Status Min(Func<Double, Double> f, double xL, double xU, int maxIterations)
        {
            return Min(f, xL, xU, 1e-8, maxIterations);
        }

        public static Status Min(Func<Double, Double> f, double xL, double xU)
        {
            return Min(f, xL, xU, 1e-8);
        }

        public static Status Min(Func<Double, Double> f, double xL, double xU, double tol)
        {
            return Min(f, xL, xU, tol, 100);
        }

        public static Status Min(Func<Double, Double> f, double xL, double xU, double tol, int maxIterations)
        {
            double xF;
            double fF;
            int iteration = 0;
            double x1 = xU - PHI_RATIO * (xU - xL);
            double x2 = xL + PHI_RATIO * (xU - xL);
            // Initial bounds:
            double f1 = Math.Round(f(x1), 6);
            double f2 = Math.Round(f(x2), 6);

            // Store these values so that we can return these if they're better.
            // This happens when the minimization falls *approaches* but never
            // actually reaches one of the bounds
            double f10 = Math.Round(f(xL), 6);
            double f20 = Math.Round(f(xU), 6);
            double xL0 = xL;
            double xU0 = xU;

            // Simple, robust golden section minimization:
            while (++iteration < maxIterations && Math.Abs(xU - xL) > tol)
            {
                if (f2 > f1)
                {
                    xU = x2;
                    x2 = x1;
                    f2 = f1;
                    x1 = xU - PHI_RATIO * (xU - xL);
                    f1 = f(x1);
                }
                else
                {
                    xL = x1;
                    x1 = x2;
                    f1 = f2;
                    x2 = xL + PHI_RATIO * (xU - xL);
                    f2 = f(x2);
                }
            }

            xF = Math.Round(0.5 * (xU + xL), 6);
            fF = Math.Round(0.5 * (f1 + f2), 6);

            Status status = new Status(iteration, xF, fF, true);
            if (Double.IsNaN(f2) || Double.IsNaN(f1) || iteration == maxIterations)
            {
                status.converged = false;
            }
            if (f10 < fF)
            {
                status.argmin = xL0;
            }
            else if (f20 < fF)
            {
                status.argmin = xU0;
            }
            else
            {
                status.argmin = xF;
            }
            return status;
        }
    }
}
