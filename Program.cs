using System;
using Ebisu.Gamma;
using Ebisu.MinimizeGolden;

namespace Ebisu
{
    class Program
    {
        static void Main(string[] args)
        {
            
            Console.WriteLine("========== Starting Tests for Gamma ==========");

            GammaTest.LoadAndCompare();

            Console.WriteLine("========== Tests for Gamma Successful!!! ==========\n");

            Console.WriteLine("========== Starting Tests for Minimize Golden ==========");
            MinTest minTest = new MinTest();
            minTest.Basic();
            Console.WriteLine("Test for min Basic Successful!!!");

            minTest.Hyperbola();
            Console.WriteLine("Test for min Hyperbola Successful!!!");

            minTest.NegHyperbola();
            Console.WriteLine("Test for min NegHyperbola Successful!!!");

            minTest.Parabola();
            Console.WriteLine("Test for min Parabola Successful!!!");

            minTest.Sqrt();
            Console.WriteLine("Test for min Sqrt Successful!!!");

            minTest.SqrtAbs();
            Console.WriteLine("Test for min SqrtAbs Successful!!!");

            minTest.Tol();
            Console.WriteLine("Test for min Tol Successful!!!");

            minTest.ParabolaEdge();
            Console.WriteLine("Test for min ParabolaEdge Successful!!!");

            minTest.Cubic();
            Console.WriteLine("Test for min Cubic Successful!!!");

            minTest.NegCubic();
            Console.WriteLine("Test for min NegCubic Successful!!!");

            minTest.BoundedCubic();
            Console.WriteLine("Test for min BoundedCubic Successful!!!");

            minTest.Cos();
            Console.WriteLine("Test for min Cos Successful!!!");

            minTest.Cusp();
            Console.WriteLine("Test for min Cusp Successful!!!");

            Console.WriteLine("========== Tests for Minimize Golden Successful!!! ==========\n");

            Console.WriteLine("========== Starting Tests for Ebisu 2.0 ==========");

            EbisuTest ebisuTest = new EbisuTest();

            ebisuTest.TestAgainstReference();
            Console.WriteLine("Test for Ebisu TestAgainstReference Successful!!!");

            ebisuTest.TestHalflife();
            Console.WriteLine("Test for Ebisu TestHalflife Successful!!!");

            ebisuTest.Predict();
            Console.WriteLine("Test for Ebisu Predict Successful!!!");

            ebisuTest.Update();
            Console.WriteLine("Test for Ebisu Update Successful!!!");

            ebisuTest.CheckLogSumExp();
            Console.WriteLine("Test for Ebisu CheckLogSumExp Successful!!!");


            Console.WriteLine("========== Tests for Ebisu 2.0 Successful!!! ==========\n");

        }
    }
}
