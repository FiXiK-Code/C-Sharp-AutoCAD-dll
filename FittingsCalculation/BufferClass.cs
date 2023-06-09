using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FittingsCalculation
{
    /// <summary>
    /// Класс для передачи информации между формами
    /// </summary>
    public static class BufferClass
    {
        /// <summary>
        /// Длинна арматуры
        /// </summary>
        public static string lengthFitting { get; set; }

        /// <summary>
        /// Масса арматуры
        /// </summary>
        public static string massFitting { get; set; } = null;

        /// <summary>
        /// Наименования арматуры согласно нормативного документа
        /// </summary>
        public static string nameFitting { get; set; } = null;

        /// <summary>
        /// Количество арматур
        /// </summary>
        public static string countFitting { get; set; }

        /// <summary>
        /// Символ диаметра для вставки
        /// </summary>
        public static bool synbol { get; set; }

        /// <summary>
        /// Площадь сечения арматуры
        /// </summary>
        public static string ploshadFitting { get; set; }

        /// <summary>
        /// Площадь сечения 1 п.м.
        /// </summary>
        public static string ploshadFitting_On_1M { get; set; }
    }
}
