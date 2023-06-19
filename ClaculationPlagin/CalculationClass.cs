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
    }
}
