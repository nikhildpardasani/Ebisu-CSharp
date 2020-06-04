using System;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Ebisu
{
    public class GammaTest
    {
        private static double logTol = 1e-12;
        private static double tol = 1e-12;

        private static double Relerr(double expected, double actual)
        {
            return (actual == expected) ? 0 : Math.Abs(actual - expected) / Math.Abs(expected);
        }

        [Test]
        public static void LoadAndCompare()
        {
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Gamma_test.json");
            string[] testData = File.ReadAllLines(path);
            double[][] expected = (double[][])JsonConvert.DeserializeObject(testData[0],typeof(double[][]));

            foreach (double[] expectedPair in expected)
            {
                double x = expectedPair[0];
                String msg = "x=" + x.ToString();
                {
                    double y = expectedPair[1];
                    double yActual = Gamma.gammaln(x);
                    Relerr(y, yActual);
                    Assert.AreEqual(0, Relerr(y, yActual), logTol);
                }
                {
                    double z = expectedPair[2];
                    double zActual = Gamma.gamma(x);
                    if (Double.IsFinite(z))
                    {
                        Relerr(z, zActual);
                        Assert.AreEqual(0, Relerr(z, zActual), tol);
                    }
                    else
                    {
                        Assert.True(z == zActual);
                    }
                }
            }
        }
    }
}
