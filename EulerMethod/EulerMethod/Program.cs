using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EulerMethod
{
    using System.Security.Cryptography;

    public class Program
    {
        private static int tableWidth = 77;

        public static void Main(string[] args)
        {
            var n = GetN();
            var a = GetA();
            var h = GetH(a, n);
            var xArray = GenerateXArray(n, h);
            var yArray = GenerateYArray(xArray, h);
            var yArrayModified = GenerateYArrayModified(xArray, h);
            var yArrayPrecise = GenerateYArrayPrecise(xArray);
            var maxError = FindMaxError(yArray, yArrayPrecise);
            var maxErrorModified = FindMaxError(yArrayModified, yArrayPrecise);

            var showStep = Math.Max(xArray.Length / 10, 1);
            var current = 0;
            Console.WriteLine();
            PrintBoldLine();
            PrintBoldRow(new[] { "x", "y Euler", "y Zmodyfikowany Euler" });
            PrintBoldLine();
            for (int i = 0; i <= Math.Min(10, n); i++)
            {
                PrintRow(new[] { xArray[current].ToString(), yArray[current].ToString(), yArrayModified[current].ToString() });
                PrintLine();
                current += showStep;
            }

            Console.WriteLine("Błąd Euler: " + maxError);
            Console.WriteLine("Błąd Zmodyfikowany Euler: " + maxErrorModified);
            Console.ReadLine();
        }

        #region Printing

        private static void PrintLine()
        {
            Console.WriteLine(new string('-', tableWidth - 1));
        }

        private static void PrintBoldLine()
        {
            Console.WriteLine(new string('#', tableWidth - 1));
        }

        private static void PrintBoldRow(string[] columns)
        {
            int width = (tableWidth - columns.Length) / columns.Length;
            string row = "#";

            foreach (string column in columns)
            {
                row += AlignCentre(column, width) + "#";
            }

            Console.WriteLine(row);
        }

        private static void PrintRow(string[] columns)
        {
            int width = (tableWidth - columns.Length) / columns.Length;
            string row = "|";

            foreach (string column in columns)
            {
                row += AlignCentre(column, width) + "|";
            }

            Console.WriteLine(row);
        }

        private static string AlignCentre(string text, int width)
        {
            text = text.Length > width ? text.Substring(0, width - 3) + "..." : text;

            if (string.IsNullOrEmpty(text))
            {
                return new string(' ', width);
            }
            else
            {
                return text.PadRight(width - (width - text.Length) / 2).PadLeft(width);
            }
        }

        #endregion

        private static double FormulaF(double x, double y)
        {
            return Math.Pow(x + y, 2) - Math.Pow(x, 4) + (2 * x) - 1;
        }

        private static double FormulaY(double x)
        {
            return Math.Pow(x, 2) - x;
        }

        private static double GetA()
        {
            Console.Write("Podaj a:");
            var s = Console.ReadLine();
            double a;
            if (double.TryParse(s, out a))
            {
                if (a > 0)
                {
                    return a;
                }
                
                throw new Exception("A musi być większa od 0");
            }

            throw new Exception("Niepoprawna liczba.");
        }

        private static uint GetN()
        {
            Console.Write("Podaj n:");
            var s = Console.ReadLine();
            uint n;
            if (uint.TryParse(s, out n))
            {
                if (n >= 1)
                {
                    return n;
                }

                throw new Exception("N musi być większa lub równa 1");
            }

            throw new Exception("Niepoprawna liczba.");
        }

        private static double GetH(double a, uint n)
        {
            return a / n;
        }

        private static double[] GenerateXArray(uint n, double h)
        {
            var temp = new Queue<double>();
            temp.Enqueue(0);
            double tempX = 0;
            for (var i = 1; i <= n; i++)
            {
                temp.Enqueue(tempX + h);
                tempX += h;
            }

            return temp.ToArray();
        }

        private static double[] GenerateYArray(double[] xArray, double h)
        {
            var temp = new Queue<double>();
            temp.Enqueue(0);
            double tempY = 0;
            for (var i = 1; i < xArray.Length; i++)
            {
                var y = tempY + (h * FormulaF(xArray[i - 1], tempY));
                temp.Enqueue(y);
                tempY = y;
            }

            return temp.ToArray();
        }

        private static double[] GenerateYArrayModified(double[] xArray, double h)
        {
            var temp = new Queue<double>();
            temp.Enqueue(0);
            double tempY = 0;
            for (var i = 1; i < xArray.Length; i++)
            {
                var y = tempY + (FormulaF(xArray[i - 1] + (h / 2), tempY + (FormulaF(xArray[i - 1], tempY) * (h / 2))) * h);
                temp.Enqueue(y);
                tempY = y;
            }

            return temp.ToArray();
        }

        private static double[] GenerateYArrayPrecise(double[] xArray)
        {
            var temp = new Queue<double>();
            foreach (var x in xArray)
            {
                temp.Enqueue(FormulaY(x));
            }

            return temp.ToArray();
        }

        private static double FindMaxError(double[] yArray, double[] yArrayPrecise)
        {
            var temp = new Queue<double>();
            var max = Math.Abs(yArray[0] - yArrayPrecise[0]);
            for (var i = 1; i < yArray.Length; i++)
            {
                var sub = Math.Abs(yArray[i] - yArrayPrecise[i]);
                if (sub > max)
                {
                    max = sub;
                }
            }

            return max;
        }
    }
}
