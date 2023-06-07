using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FittingsCalculation
{
    /// <summary>
    /// Класс для математических рассетов.
    /// </summary>
    public static class CalculationClass
    {
        /// <summary>
        /// Метод для получения площади сечения арматуры
        /// </summary>
        /// <param name="d">Диаметр стержней</param>
        /// <param name="n">Количество стержней</param>
        /// <param name="p">Шаг стержней(расстояние между центрами стержней) в миллиметрах</param>
        /// <returns>Площадь сечения арматуры</returns>
        public static double GetPlosh(double d, double n, int p = 0)
        {
            //S = (πd²/4) * n
            //n = L / p
           
            if (p != 0)
            {
                n = 1 / p / 1000;
            }

            return n * (Math.PI * Math.Pow(d, 2) / 4);
        }

        /// <summary>
        /// Метод для получения массы арматуры
        /// </summary>
        /// <param name="D">Диаметр арматуры</param>
        /// <param name="L">Длинна арматуры в мм</param>
        /// <returns></returns>
        public static double GetMass(string D, string L, int gost)
        {
            if(gost == 0)//ГОСТ 5781-82
            {
                return Math.Round(Math.PI * Math.Pow(Convert.ToDouble(D), 2) * Convert.ToDouble(L) * 1.05 * 0.006162, 3) * Convert.ToDouble(BufferClass.countFitting);

            }
            else//ГОСТ 34028-2016
            {
                return Math.Round((Math.PI * Math.Pow(Convert.ToDouble(D), 2) * Convert.ToDouble(L) / 4) * 0.785 , 3) * Convert.ToDouble(BufferClass.countFitting);
            }

            //return Math.Round( Math.PI * Math.Pow( Convert.ToDouble(D), 2) / 4 * 0.7850 * Convert.ToDouble(L) / 100000, 3) * Convert.ToDouble(BufferClass.countFitting);
        }

        /// <summary>
        /// Метод получения площади сечения
        /// </summary>
        /// <param name="D"> Диаметр арматуры</param>
        /// <returns></returns>
        public static double GetPloshSech(string D)
        {
            return Math.Round( Math.PI * Math.Pow( Convert.ToDouble(D), 2 ) / 4 , 1 )  / 100;
        }

    }
}
