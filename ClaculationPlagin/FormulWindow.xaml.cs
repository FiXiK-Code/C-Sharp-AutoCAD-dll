using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ClaculationPlagin
{
    /// <summary>
    /// Логика взаимодействия для FormulWindow.xaml
    /// </summary>
    public partial class FormulWindow : Window
    {
        public FormulWindow()
        {
            InitializeComponent();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            BufferClass.formul = ((Button)sender).Content.ToString().Remove(((Button)sender).Content.ToString().Length - 1);
            this.Close();
        }
    }
}
