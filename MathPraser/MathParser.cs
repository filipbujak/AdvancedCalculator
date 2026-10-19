using System;
using System.Collections.Generic;
using System.Text;

namespace ExpressionCalculator
{
    public class MathParser
    {
        public static double Evaluate(string expression)
        {
            expression = expression.Replace(" ", "");
            expression = expression.Replace(",", ".");

            var listexpression = new List<string>();

            var operators = new List<string> { "+", "-", "*", "/", "(", ")", "^" };
            var numbers = new List<string> { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "." };
            // operatory działań bez nawiasów – tylko one nie mogą stać obok siebie
            var checker = new List<string> { "+", "-", "*", "/", "^" };

            // czy następna liczba ma być ujemna
            bool negative = false;

            for (int i = 0; i < expression.Length; i++)
            {
                string c = expression[i].ToString();

                if (operators.Contains(c))
                {
                    // minus liczby ujemnej: na początku albo po operatorze / "(" (ale nie po ")")
                    if (c == "-" && (i == 0 || (operators.Contains(expression[i - 1].ToString()) && expression[i - 1] != ')')))
                    {
                        negative = !negative; // "--2" = 2
                        continue;
                    }

                    // mnożenie domyślne: 
                    if (c == "(" && listexpression.Count > 0)
                    {
                        string last = listexpression[listexpression.Count - 1];

                        if (last == ")" || !operators.Contains(last))  
                        {
                            listexpression.Add("*");
                        }
                    }

                    if (negative && c == "(")
                    {
                        listexpression.Add("-1");
                        listexpression.Add("*");
                        negative = false;
                    }
                    else if (negative)
                    {
                        throw new ArgumentException("Brak liczby po minusie");
                    }

                    if (checker.Contains(c) && (i == 0 || expression[i - 1] == '('))
                    {
                        throw new ArgumentException($"Operator '{c}' w złym miejscu");
                    }

                    // dwa operatory obok siebie z wyjatkiem -
                    if (checker.Contains(c)
                        && i + 1 < expression.Length
                        && checker.Contains(expression[i + 1].ToString())
                        && expression[i + 1] != '-')
                    {
                        throw new ArgumentException("Dwa operatory obok siebie");
                    }

                    listexpression.Add(c);
                }
                else if (numbers.Contains(c))
                {
                    if (listexpression.Count > 0 && listexpression[listexpression.Count - 1] == ")")
                    {
                        listexpression.Add("*");
                    }

                    StringBuilder numberBuilder = new StringBuilder();

                    if (negative)
                    {
                        numberBuilder.Append('-');
                        negative = false;
                    }

                    while (i < expression.Length && numbers.Contains(expression[i].ToString()))
                    {
                        numberBuilder.Append(expression[i]);
                        i++;
                    }
                    i--; // aby nie pominac
                    listexpression.Add(numberBuilder.ToString());
                }
                else
                {
                    throw new ArgumentException($"Niedozwolony znak: '{c}'");
                }
            }

            if (negative || checker.Contains(listexpression[listexpression.Count - 1]))
            {
                throw new ArgumentException("Wyrażenie nie może kończyć się operatorem");
            }

            return Calculator.Calculate(listexpression);
        }
    }
}