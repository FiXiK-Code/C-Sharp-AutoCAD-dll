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


        /// <summary>
        /// Открытие окна для выбора формулы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void functionBtn_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        /// <summary>
        /// Добавление знаков после запятой
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void plusRound_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        /// <summary>
        /// Уменьшение знаков после запятой
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void minusRound_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        /// <summary>
        /// Cath изменения количества знаков после запятой
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DotPositionTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        /// <summary>
        /// Скрытие появление кнопок для получения данных с чертежа
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void inoutDataSection_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        /// <summary>
        /// Получение расстояния между точками
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lengthInput_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        /// <summary>
        /// Получение суммы длин из объектов
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sumLengthInput_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        /// <summary>
        /// Поличение координаты
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void coordinateInput_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        /// <summary>
        /// Получени площади
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ploshInput_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        /// <summary>
        /// Получение значения из (мульти)-текста
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textInput_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        /// <summary>
        /// Получение значения из выноски
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void vinosInput_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        /// <summary>
        /// Получени значения из таблицы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tableInput_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        /// <summary>
        /// Получение данных из размера
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dimensionInput_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        /// <summary>
        /// Несколько значений
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void multyCheckBox_Checked(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>
        /// Округление
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void roundCheckBox_Checked(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>
        /// Знак разделителя при выборе нескольких значений
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void symbolMultyComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        /// <summary>
        /// Поле входных значений
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void inputTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        /// <summary>
        /// Поле результата
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void resultTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        /// <summary>
        /// Вставка однострочного текста
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void onseTextOutput_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        /// <summary>
        /// Добавить результат к однострочному тексту
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void plusOnseTextOutput_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        /// <summary>
        /// Вставка результата в таблицу
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tableOutput_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        /// <summary>
        /// Добавить результат к многострочному тексту
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void multyTextOutput_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        /// <summary>
        /// Резильтат в виде выноски
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void vinosOuput_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        /// <summary>
        /// Кнопка журнала
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void history_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        /// <summary>
        /// Очистка истории
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void remuveHistory_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
