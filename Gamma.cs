using System;
namespace Ebisu
{
    public class Gamma
    {
        private static double g = 7;
        private static double[] p = {0.99999999999980993,  676.5203681218851,     -1259.1392167224028,
                               771.32342877765313,   -176.61502916214059,   12.507343278686905,
                               -0.13857109526572012, 9.9843695780195716e-6, 1.5056327351493116e-7};

        private static double g_ln = 607.0 / 128.0;
        private static double[] p_ln = {0.99999999999999709182,     57.156235665862923517,      -59.597960355475491248,
                                  14.136097974741747174,      -0.49191381609762019978,    0.33994649984811888699e-4,
                                  0.46523628927048575665e-4,  -0.98374475304879564677e-4, 0.15808870322491248884e-3,
                                  -0.21026444172410488319e-3, 0.21743961811521264320e-3,  -0.16431810653676389022e-3,
                                  0.84418223983852743293e-4,  -0.26190838401581408670e-4, 0.36899182659531622704e-5};

        private static double log2π = Math.Log(2 * Math.PI);
        private static double sqrt2π = Math.Sqrt(2 * Math.PI);

        public Gamma()
        {
        }

        public static double gammaln(double z)
        {
            if (z < 0) { return Double.NaN; }
            double x = p_ln[0];
            for (int i = p_ln.Length - 1; i > 0; --i) { x += p_ln[i] / (z + i); }
            double t = z + g_ln + 0.5;
            return .5 * log2π + (z + .5) * Math.Log(t) - t + Math.Log(x) - Math.Log(z);
        }

        public static double gamma(double z)
        {
            if (z < 0.5)
            {
                return Math.PI / (Math.Sin(Math.PI * z) * gamma(1 - z));
            }
            else if (z > 100)
                return Math.Exp(gammaln(z));
            else
            {
                z -= 1;
                double x = p[0];
                for (int i = 1; i < (int)g + 2; i++) { x += p[i] / (z + i); }
                double t = z + g + 0.5;
                return sqrt2π * Math.Pow(t, z + 0.5) * Math.Exp(-t) * x;
            }
        }
    }
}
