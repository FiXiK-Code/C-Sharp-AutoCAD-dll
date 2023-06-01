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
        public static string lengthFitting { get; set; }
        public static string massFitting { get; set; }
        public static string nameFitting { get; set; }
        public static string countFitting { get; set; }
        public static bool synbol { get; set; }

        public static string ploshadFitting { get; set; }
        public static string ploshadFitting_On_1M { get; set; }
    }
}
