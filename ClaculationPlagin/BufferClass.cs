using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ClaculationPlagin
{
    public static class BufferClass
    {
        public static bool multyItem { get; set; } = false;
        public static bool round { get; set; } = false;
        public static string znac { get; set; } = "+";
        public static int roundItem { get; set; } = 3;
        public static string pref { get; set; } = null;
        public static string suff { get; set; } = null;
        public static string formul { get; set; } = null;
        public static StackPanel SetTemlate(StackPanel value, string text1, string text2)
        {
            if (text1 != null && text2 != null)
            {
                ((TextBlock)(((Border)value.Children[0]).Child)).Text = text1;
                ((TextBlock)(((Border)value.Children[1]).Child)).Text = text2;
            }
            if(text1 == null)
            {
                ((TextBlock)(((Border)value.Children[1]).Child)).Text = text2;
                value.Children.Remove(value.Children[0]);
            }
            if (text2 == null)
            {
                ((TextBlock)(((Border)value.Children[0]).Child)).Text = text1;
                value.Children.Remove(value.Children[1]);
            }

            return value;
        }
    }
}
