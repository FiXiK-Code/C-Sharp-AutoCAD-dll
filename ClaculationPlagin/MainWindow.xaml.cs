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
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void PointSizeBtn_Click(object sender, RoutedEventArgs e) // получене расстояния между точками
        {
            var result = CommanClass.GetSize();


        }

        private void PolySizeBtn_Click(object sender, RoutedEventArgs e) // получение длянны объктов
        {
            var result = CommanClass.GetPolySize();


        }

        private void PointCoordinateBtn_Click(object sender, RoutedEventArgs e) // получение координаты точки (выбор - XYZ)
        {
            string nap = "x";

            var result = CommanClass.GetCoordinate(nap);


        }

        private void PlBtn_Click(object sender, RoutedEventArgs e) // получение площади
        {



        }

        private void InputTextBtn_Click(object sender, RoutedEventArgs e) // получение значения из текста
        {

        }

        private void InputToolsBtn_Click(object sender, RoutedEventArgs e) // получение значения из выноски
        {

        }

        private void TableInputBtn_Click(object sender, RoutedEventArgs e) // получение значения из таблицы (яч)
        {

        }

        private void SizeInputBtn_Click(object sender, RoutedEventArgs e) // получение размера
        {
            string result = CommanClass.GetDimension();
            if(result == "Комманда была завершена!")
            {
                MessageBox.Show(result);
                return;
            }
            if(result == null)
            {
                MessageBox.Show("Error!");
                return;
            }
        }


        /////////

        private void OutputOneTextBtn_Click(object sender, RoutedEventArgs e) // вставка однострочного текста
        {

        }

        private void OutputAddetTextBtn_Click(object sender, RoutedEventArgs e) // заменить текст в существующе (одстр)
        {

        }

        private void OutputInTableBtn_Click(object sender, RoutedEventArgs e) // вставить в таблицу
        {

        }

        private void OutputPolyTextBtn_Click(object sender, RoutedEventArgs e) // добавление к многострочному
        {

        }

        private void OutputToolBtn_Click(object sender, RoutedEventArgs e) // вставить в виде выноски
        {

        }

        private void DotPositionPlusBtn_Click(object sender, RoutedEventArgs e)
        {
            DotPositionTextBox.Text = (Convert.ToInt32(DotPositionTextBox.Text) + 1).ToString();
        }

        private void DotPositionMinBtn_Click(object sender, RoutedEventArgs e)
        {
            DotPositionTextBox.Text = Convert.ToInt32(DotPositionTextBox.Text) >= 1 ? (Convert.ToInt32(DotPositionTextBox.Text) - 1).ToString() : DotPositionTextBox.Text;
        }
    }
}
