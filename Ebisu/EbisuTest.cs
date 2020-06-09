using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace Ebisu
{
    public class EbisuTest
    {

        private readonly double EPS = 0.0000001;

        private static double Relerr(double dirt, double gold)
        {
            return (dirt == gold) ? 0 : Math.Abs(dirt - gold) / Math.Abs(gold);
        }

        public void TestAgainstReference()
        {
            try
            {
                // All this boilerplate is just to load JSON
                
                double maxTol = 5e-3;
                string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Ebisu/Ebisu_test.json");
                string[] testData = File.ReadAllLines(path);
                JArray expectedResult = (JArray)JsonConvert.DeserializeObject(testData[0]);

                foreach (var child in expectedResult)
                {
                    //subtest might be either
                    // a) ["update", [3.3, 4.4, 1.0], [0, 5, 0.1], {"post": [7.333641958415551, 8.949256654818793,
                    // 0.4148304099305316]}] or b) ["predict", [34.4, 34.4, 1.0], [5.5], {"mean": 0.026134289032202798}]
                    //
                    // In both cases, the first two elements are a string and an array of numbers. Then the remaining vary depend on
                    // what that string is. where the numbers are arbitrary. So here we go...
                    String operation = child[0].ToString();

                    JArray second = (JArray)child[1];
                    EbisuModel ebisu = new EbisuModel(double.Parse(second[2].ToString()), double.Parse(second[0].ToString()), double.Parse(second[1].ToString()));

                    if (operation.Equals("update"))
                    {
                        int successes = Convert.ToInt32(child[2][0].ToString());
                        int total = Convert.ToInt32(child[2][1].ToString());
                        double t = Convert.ToDouble(child[2][2].ToString());
                        JArray third = (JArray)child[3].Last.Last;//subtest.get(3).get("post");
                        EbisuModel expected = new EbisuModel(double.Parse(third[2].ToString()), double.Parse(third[0].ToString()), double.Parse(third[1].ToString()));

                        IEbisu actual = Ebisu.UpdateRecall(ebisu, successes, total, t);

                        Assert.AreEqual(expected.getAlpha(), actual.getAlpha(), maxTol);
                        Assert.AreEqual(expected.getBeta(), actual.getBeta(), maxTol);
                        Assert.AreEqual(expected.getTime(), actual.getTime(), maxTol);
                    }
                    else if (operation.Equals("predict"))
                    {
                        double t = Convert.ToDouble(child[2][0].ToString());
                        double expected = Convert.ToDouble(child[3].First.Last.ToString());
                        double actual = Ebisu.PredictRecall(ebisu, t, true);
                        Assert.AreEqual(expected, actual, maxTol);
                    }
                    else
                    {
                        throw new Exception("unknown operation");
                    }
                }
            }
            catch (Exception ex)
            {
                ex.StackTrace.ToString();
                Console.WriteLine("¡¡¡OOOPS SOMETHING BAD HAPPENED!!!");
                Assert.IsTrue(false);
            }
        }

        [Test]
        public void TestHalflife()
        {
            double hl = 20.0;
            EbisuModel m = new EbisuModel(hl, 2, 2);
            Assert.True(Math.Abs(Ebisu.ModelToPercentileDecay(m, .5, true) - hl) > 1e-2);
            Assert.True(Relerr(Ebisu.ModelToPercentileDecay(m, .5, 1e-6), hl) < 1e-3);
            //Assert.Throws<Exception>(() => Ebisu.ModelToPercentileDecay(m, 0.5, 1e-150));
        }

        [Test]
        public void Predict()
        {
            EbisuModel m = new EbisuModel(2, 2, 2);
            double p = Ebisu.PredictRecall(m, 2, true);
            Assert.AreEqual(0.5, p, EPS, "1 + 1 should equal 2");
        }

        [Test]
        public void Update()
        {
            IEbisu m = new EbisuModel(2, 2, 2);
            IEbisu success = Ebisu.UpdateRecall(m, 1, 1, 2.0);
            IEbisu failure = Ebisu.UpdateRecall(m, 0, 1, 2.0);

            Assert.AreEqual(3.0, success.getAlpha(), 500 * EPS, "success/alpha");
            Assert.AreEqual(2.0, success.getBeta(), 500 * EPS, "success/beta");

            Assert.AreEqual(2.0, failure.getAlpha(), 500 * EPS, "failure/alpha");
            Assert.AreEqual(3.0, failure.getBeta(), 500 * EPS, "failure/beta");
        }

        [Test]
        public void CheckLogSumExp()
        {
            double expected = Math.Exp(3.3) + Math.Exp(4.4) - Math.Exp(5.5);
            double[] actual = Ebisu.LogSumExp(new List<double>() { 3.3, 4.4, 5.5 }, new List<double>() { 1.0, 1.0, -1.0 });

            double EPSilon = ULP(actual[0]);
            Assert.AreEqual(Math.Log(Math.Abs(expected)), actual[0], EPSilon, "Magnitude of logSumExp");

            Assert.AreEqual(Math.Sign(expected), actual[1], "Sign of logSumExp");
        }

        double ULP(double value)
        {
            long bits = BitConverter.DoubleToInt64Bits(value);
            double nextValue = BitConverter.Int64BitsToDouble(bits + 1);
            double result = nextValue - value;
            return result;
        }
    }
}