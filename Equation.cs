using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightIntensityAnalyzer
{
    public class Equation
    {
        public static void Process()
        {
            while (true)
            {
                Console.WriteLine("Input OM in px:");
                var OM = double.Parse(Console.ReadLine());
                Console.WriteLine("Input CM in px:");
                var CM = double.Parse(Console.ReadLine());
                Console.WriteLine("Input L in cm:");
                var L = double.Parse(Console.ReadLine());
                Console.WriteLine("Input R in cm:");
                var R = double.Parse(Console.ReadLine());
                Console.WriteLine("------------------------------:");

                Count(OM, CM, L, R);
                Console.ReadKey();
                Console.WriteLine("\n===============================");
            }
        }

        static void Count(double OM, double CM_in_px, double L_in_cm, double R_in_cm)
        {
            var rate = CM_in_px / R_in_cm;
            Console.WriteLine($"Rate = {Math.Round(rate, 3)} px/cm");

            var L = rate * L_in_cm;
            Console.WriteLine($"L = {Math.Round(L, 3)} px");

            var a = Math.Acos(OM / CM_in_px);
            Console.WriteLine($"alfa = {ToDeg(a)} deg");

            var CO = CM_in_px * Math.Sin(a);
            Console.WriteLine($"CO = {Math.Round(CO, 3)}");

            var y = Math.Atan((L + CM_in_px - CO) / OM);
            Console.WriteLine($"gamma = {ToDeg(y)} deg");

            var b = Math.PI - a - y;
            Console.WriteLine($"beta = {ToDeg(b)} deg");
        }

        static double ToDeg(double rad) => Math.Round((180 / Math.PI) * rad, 3);
    }
}
