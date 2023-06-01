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
        /// <param name="L"> длина участка, на который требуется расчитать количество стержней</param>
        /// <returns>Площадь сечения арматуры</returns>
        public static double GetPlosh(double d, double n, int p = 0, double L = 1)
        {
            //S = (πd²/4) * n
            //n = L / p
           
            if (p != 0)
            {
                n = L / p / 1000;
            }

            return n * (Math.PI * Math.Pow(d, 2) / 4);
        }
    }
}
