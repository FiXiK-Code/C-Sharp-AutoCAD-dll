using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClaculationPlagin
{
    /// <summary>
    /// Класс для математических рассетов.
    /// </summary>
    public static class CalculationClass
    {
        /// <summary>
        /// Метод для рассчета результата функций из дополнительно окна
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static double FunctionCalculation(string value)
        {
            var nameFunction = value.Split('(')[0];
            var content = value.Split('(')[1];
            content = content.Remove(content.Length - 1);
            switch (nameFunction)
            {
                case "Abs":
                    return Math.Abs(Convert.ToDouble(content));
                case "Acos":
                    return Math.Acos(Convert.ToDouble(content));
                case "Asin":
                    return Math.Asin(Convert.ToDouble(content));
                case "Atan":
                    return Math.Atan(Convert.ToDouble(content));
                case "Ceiling":
                    return Math.Ceiling(Convert.ToDouble(content));
                case "Exp":
                    return Math.Exp(Convert.ToDouble(content));
                case "Floor":
                    return Math.Floor(Convert.ToDouble(content));
                case "IEEERemainder":
                    return Math.IEEERemainder(Convert.ToDouble(content.Split(',')[0]), Convert.ToDouble(content.Split(',')[1]));
                case "Log":
                    return Math.Log(Convert.ToDouble(content.Split(',')[0]), Convert.ToDouble(content.Split(',')[1]));
                case "Log10":
                    return Math.Log10(Convert.ToDouble(content));
                case "Max":
                    return Math.Max(Convert.ToDouble(content.Split(',')[0]), Convert.ToDouble(content.Split(',')[1]));
                case "Min":
                    return Math.Min(Convert.ToDouble(content.Split(',')[0]), Convert.ToDouble(content.Split(',')[1]));
                case "Pow":
                    return Math.Pow(Convert.ToDouble(content.Split(',')[0]), Convert.ToDouble(content.Split(',')[1]));
                case "Round":
                    return Math.Round(Convert.ToDouble(content.Split(',')[0]), Convert.ToInt32(content.Split(',')[1]));
                case "Sign":
                    return Math.Sign(Convert.ToDouble(content));
                case "Truncate":
                    return Math.Truncate(Convert.ToDouble(content));
            }
            return 0.0;
        }

        /// <summary>
        /// Метод для математического рассчета входной строки
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static double Calculate(string value)
        {
            value = value.Replace(")","");
            value = value.Replace(" ","");

            value = UmnFunc(value);
            value = DelFunc(value);
            value = PlusFunc(value);
            value = MinFunc(value);

            return Convert.ToDouble(value);
        }

        /// <summary>
        /// Метод, рассчитывающий элементы произведения в входной строке
        /// </summary>
        /// <param name="value">входная строка</param>
        /// <returns>Строку в которой элементы произведения заменены на результат</returns>
        private static string UmnFunc(string value)
        {
            while(true)
            {
                var indexZnac = value.IndexOf('*');
                if (indexZnac == -1) break;
                var num1 = "";
                var num2 = "";

                bool num1_ok = false;
                bool num2_ok = false;

                int j = indexZnac - 1;
                int k = indexZnac + 1;

                while (true)
                {
                    if (j > 0)
                    {
                        if (value[j] == '*' || value[j] == '-' || value[j] == '/' || value[j] == '+' && !num1_ok)
                        {
                            if (j - 1 != 0  && value[j - 1] == '*' || value[j - 1] == '/' || value[j - 1] != '+')
                            {
                                num1_ok = true;
                            }
                            if(value[j] == '-')
                            {
                                num1_ok = true;
                                num1 = value[j] + num1;
                                j--;
                            }
                        }
                    }
                    if (j == -1) num1_ok = true;
                    if (k < value.Length)
                    {
                        if (value[k] == '*' || value[k] == '-' || value[k] == '/' || value[k] == '+' && !num2_ok)
                        {
                            if (num2 != "")
                            {
                                num2_ok = true;
                            }
                        }
                    }
                    if (k == value.Length) num2_ok = true;

                    if (!num1_ok)
                    {
                        num1 = value[j] + num1;
                        j--;
                    }

                    if (!num2_ok)
                    {
                        num2 += value[k];
                        k++;
                    }

                    if (num1_ok && num2_ok) break;
                }

                var res = Convert.ToDouble(num1) * Convert.ToDouble(num2);
                value = value.Replace(num1 + "*" + num2, j>=0 && value[j] != '*' && value[j] != '-' && value[j] != '/' && value[j] != '+' && res >= 0 ? "+"+res.ToString() : res.ToString());
            }
            return value;
        }

        /// <summary>
        /// Метод, рассчитывающий элементы деления в входной строке
        /// </summary>
        /// <param name="value">входная строка</param>
        /// <returns>Строку в которой элементы деления заменены на результат</returns>
        private static string DelFunc(string value)
        {
            while(true)
            {
                var indexZnac = value.IndexOf('/');
                if (indexZnac == -1) break;
                var num1 = "";
                var num2 = "";

                bool num1_ok = false;
                bool num2_ok = false;

                int j = indexZnac - 1;
                int k = indexZnac + 1;

                while (true)
                {
                    if (j > 0)
                    {
                        if (value[j] == '*' || value[j] == '-' || value[j] == '/' || value[j] == '+' && !num1_ok)
                        {
                            if (j - 1 != 0 && value[j - 1] == '*' || value[j - 1] == '/' || value[j - 1] != '+')
                            {
                                num1_ok = true;
                            }
                            if (value[j] == '-')
                            {
                                num1_ok = true;
                                num1 = value[j] + num1;
                                j--;
                            }
                        }
                    }
                    if (j == -1) num1_ok = true;
                    if (k < value.Length)
                    {
                        if (value[k] == '*' || value[k] == '-' || value[k] == '/' || value[k] == '+' && !num2_ok)
                        {
                            if (num2 != "")
                            {
                                num2_ok = true;
                            }

                        }
                    }
                    if (k == value.Length) num2_ok = true;

                    if (!num1_ok)
                    {
                        num1 = value[j] + num1;
                        j--;
                    }

                    if (!num2_ok)
                    {
                        num2 += value[k];
                        k++;
                    }

                    if (num1_ok && num2_ok) break;
                }

                var res = Convert.ToDouble(num1) / Convert.ToDouble(num2);
                value = value.Replace(num1 + "/" + num2, j >= 0 && value[j] != '*' && value[j] != '-' && value[j] != '/' && value[j] != '+' && res >= 0 ? "+" + res.ToString() : res.ToString());

            }
            return value;
        }

        /// <summary>
        /// Метод, рассчитывающий элементы сложения в входной строке
        /// </summary>
        /// <param name="value">входная строка</param>
        /// <returns>Строку в которой элементы сложения заменены на результат</returns>
        private static string PlusFunc(string value)
        {
            while(true)
            {
                var indexZnac = value.IndexOf('+');
                if (indexZnac == -1) break;
                var num1 = "";
                var num2 = "";

                bool num1_ok = false;
                bool num2_ok = false;

                int j = indexZnac - 1;
                int k = indexZnac + 1;

                while (true)
                {
                    if (j > 0)
                    {
                        if (value[j] == '*' || value[j] == '-' || value[j] == '/' || value[j] == '+' && !num1_ok)
                        {
                            if (j - 1 != 0 && value[j - 1] == '*' || value[j - 1] == '/' || value[j - 1] != '+')
                            {
                                num1_ok = true;
                            }
                            if (value[j] == '-')
                            {
                                num1_ok = true;
                                num1 = value[j] + num1;
                                j--;
                            }
                        }
                    }
                    if(j==-1) num1_ok = true;
                    if (k < value.Length)
                    {
                        if (value[k] == '*' || value[k] == '-' || value[k] == '/' || value[k] == '+' && !num2_ok)
                        {
                            if ( num2 != "")
                            {
                                num2_ok = true;
                            }

                        }
                    }
                    if(k==value.Length) num2_ok = true;

                    if (!num1_ok)
                    {
                        num1 = value[j] + num1;
                        j--;
                    }

                    if (!num2_ok)
                    {
                        num2 += value[k];
                        k++;
                    }

                    if (num1_ok && num2_ok) break;
                }

                var res = Convert.ToDouble(num1) + Convert.ToDouble(num2);
                value = value.Replace(num1 + "+" + num2, j >= 0 && value[j] != '*' && value[j] != '-' && value[j] != '/' && value[j] != '+' && res >= 0 ? "+" + res.ToString() : res.ToString());

            }
            return value;
        }

        /// <summary>
        /// Метод, рассчитывающий элементы вычитания в входной строке
        /// </summary>
        /// <param name="value">входная строка</param>
        /// <returns>Строку в которой элементы вычитания заменены на результат</returns>
        private static string MinFunc(string value)
        {
            while(true)
            {
                var indexZnac = value.IndexOf('-',1);
                var num1 = "";
                if (indexZnac == -1) break;
                if (indexZnac == 0) num1 = "0";
                var num2 = "";

                bool num1_ok = false;
                bool num2_ok = false;

                int j = indexZnac - 1;
                int k = indexZnac + 1;

                while (true)
                {
                    if (j > 0)
                    {
                        if (value[j] == '*' || value[j] == '-' || value[j] == '/' || value[j] == '+' && !num1_ok)
                        {
                            if (j - 1 != 0 && value[j - 1] == '*' || value[j - 1] == '/' || value[j - 1] != '+')
                            {
                                num1_ok = true;
                            }
                            if (value[j] == '-')
                            {
                                num1_ok = true;
                                num1 = value[j] + num1;
                                j--;
                            }
                        }
                    }
                    if (j == -1) num1_ok = true;
                    if (k < value.Length)
                    {
                        if (value[k] == '*' || value[k] == '-' || value[k] == '/' || value[k] == '+' && !num2_ok)
                        {
                            if (num2 != "")
                            {
                                num2_ok = true;
                            }

                        }
                    }
                    if (k == value.Length) num2_ok = true;

                    if (!num1_ok)
                    {
                        num1 = value[j] + num1;
                        j--;
                    }

                    if (!num2_ok)
                    {
                        num2 += value[k];
                        k++;
                    }

                    if (num1_ok && num2_ok) break;
                }

                var res = Convert.ToDouble(num1) - Convert.ToDouble(num2);
                value = value.Replace(num1 + "-" + num2, j >= 0 && value[j] != '*' && value[j] != '-' && value[j] != '/' && value[j] != '+' && res >= 0 ? "+" + res.ToString() : res.ToString());

            }
            return value;
        }
    }
}
