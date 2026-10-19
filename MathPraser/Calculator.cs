using System;
using System.Collections.Generic;
using System.Globalization;

namespace ExpressionCalculator
{
    internal class Calculator
    {
        public static double Calculate(List<string> listexpression)
        {
            while (listexpression.Contains("("))
            {
                int firstIndex = listexpression.LastIndexOf("(");
                int lastIndex = listexpression.IndexOf(")", firstIndex);

                if (lastIndex == -1)
                {
                    throw new ArgumentException("Brak zamykającego nawiasu");
                }

                var inside = new List<string>();
                for (int i = 0; i < lastIndex - firstIndex - 1; i++)
                {
                    inside.Add(listexpression[firstIndex + i + 1]);
                }

                double value = CalculateFlat(inside);

                listexpression.RemoveRange(firstIndex, lastIndex - firstIndex + 1);   
                listexpression.Insert(firstIndex, value.ToString(CultureInfo.InvariantCulture));
            }

            if (listexpression.Contains(")"))                                         
            {
                throw new ArgumentException("Brak otwierającego nawiasu");
            }

            return CalculateFlat(listexpression);                                     
        }

        private static double CalculateFlat(List<string> list)
        {
            // potęgi – od prawej
            while (list.Contains("^"))
            {
                int index = list.LastIndexOf("^");

                if (index == 0 || index == list.Count - 1)
                {
                    throw new ArgumentException("Brak liczby przy ^");
                }

                double x = double.Parse(list[index + 1], CultureInfo.InvariantCulture);
                double z = double.Parse(list[index - 1], CultureInfo.InvariantCulture);

                double score = Math.Pow(z, x);

                list.RemoveRange(index - 1, 3);
                list.Insert(index - 1, score.ToString(CultureInfo.InvariantCulture));
            }

            // mnożenie i dzielenie – od lewej
            while (list.Contains("*") || list.Contains("/"))
            {
                int index = list.FindIndex(t => t == "*" || t == "/");

                if (index == 0 || index == list.Count - 1)
                {
                    throw new ArgumentException($"Brak liczby przy {list[index]}");
                }

                double x = double.Parse(list[index + 1], CultureInfo.InvariantCulture);
                double z = double.Parse(list[index - 1], CultureInfo.InvariantCulture);

                double score;
                if (list[index] == "*")
                {
                    score = z * x;
                }
                else
                {
                    if (x == 0)
                    {
                        throw new DivideByZeroException("Dzielenie przez zero");
                    }
                    score = z / x;
                }

                list.RemoveRange(index - 1, 3);
                list.Insert(index - 1, score.ToString(CultureInfo.InvariantCulture));
            }

            // dodawanie i odejmowanie – od lewej
            while (list.Contains("+") || list.Contains("-"))
            {
                int index = list.FindIndex(t => t == "+" || t == "-");

                if (index == 0 || index == list.Count - 1)
                {
                    throw new ArgumentException($"Brak liczby przy {list[index]}");
                }

                double x = double.Parse(list[index + 1], CultureInfo.InvariantCulture);
                double z = double.Parse(list[index - 1], CultureInfo.InvariantCulture);

                double score;
                if (list[index] == "+")
                {
                    score = z + x;
                }
                else
                {
                    score = z - x;
                }

                list.RemoveRange(index - 1, 3);
                list.Insert(index - 1, score.ToString(CultureInfo.InvariantCulture));
            }

            if (list.Count == 0)
            {
                throw new ArgumentException($"Pusty nawias");
            }

            if (list.Count > 1)
            {
                throw new ArgumentException($"Brak operatora między liczbami");
            }

            return double.Parse(list[0], CultureInfo.InvariantCulture);
        }
    }
}