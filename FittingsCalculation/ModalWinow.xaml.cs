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

namespace FittingsCalculation
{
    public partial class ModalWinow
    {
        public ModalWinow()
        {
            InitializeComponent();

            BufferClass.countFitting = "1";
            BufferClass.synbol = false;
        }


        #region massTabContrl ////////////////

        /// <summary>
        /// Закрытие окна
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void closeBtn_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }


        /// <summary>
        /// Результат 1 п.м. или рассчет (для вставки в таблицу)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void onePMCheckBox_Click(object sender, RoutedEventArgs e)
        {
            if (onePMCheckBox.IsChecked == true)
            {
                resToTableLink.Content = "Массу 1 п.м. в таблицу";
                resStackPanel.Height = 0;
                resStackPanel.IsEnabled = false;
                lengthFitting.IsEnabled = false;
                BufferClass.massFitting = masOnePMComboBox.Text;
            }
            else
            {
                resToTableLink.Content = "Результат в таблицу";
                resStackPanel.Height = 25;
                resStackPanel.IsEnabled = true;
                lengthFitting.IsEnabled = true;
                BufferClass.massFitting = resultTextBox.Text;
            }
        }


        /// <summary>
        /// Проверка на добавление символа
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addSymbolCheckBox_Click(object sender, RoutedEventArgs e)
        {
            if(addSymbolCheckBox.IsChecked == true)
            {
                BufferClass.synbol = true;
            }
            else
            {
                BufferClass.synbol = false;
            }
        }

        /// <summary>
        /// Ограничение ввода симолов в строку длинны арматуры
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lengthFitting_KeyDown(object sender, KeyEventArgs e)
        {

            char number = e.Key.ToString().Last();


            if (!Char.IsDigit(number))
            {
                e.Handled = true;
            }
        }


        /// <summary>
        /// Вставка результата в таблицу + возможноть вставки 1 п.м.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void resToTableLink_MouseDown(object sender, MouseButtonEventArgs e)
        {
            WindowState windowState = this.WindowState;

            this.WindowState = WindowState.Minimized;
            CommandClass.InsertTableText(BufferClass.massFitting, (bool)onePMCheckBox.IsChecked);
            this.WindowState = windowState;
            
        }


        /// <summary>
        /// Вставка наименования в таблицу
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void nameToTableLink_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (BufferClass.synbol) BufferClass.nameFitting += " Ø";
            WindowState windowState = this.WindowState;

            this.WindowState = WindowState.Minimized;
            CommandClass.InsertTableText(BufferClass.nameFitting, false);
            this.WindowState = windowState;
        }


        /// <summary>
        /// Вставка наименования и результата в таблицу
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void resAndNameToTableLink_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (BufferClass.synbol) BufferClass.nameFitting += " Ø";
            WindowState windowState = this.WindowState;

            this.WindowState = WindowState.Minimized;
            CommandClass.InsertTableText(BufferClass.nameFitting, BufferClass.massFitting);
            this.WindowState = windowState;
        }

        


        /// <summary>
        /// Получение размера с чертежа
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void getLengthBtn_MouseDown(object sender, MouseButtonEventArgs e)
        {
            WindowState windowState = this.WindowState;
            
            this.WindowState = WindowState.Minimized;
            lengthFitting.Text = CommandClass.GetSize();
            this.WindowState = windowState;
        }


        /// <summary>
        /// Получение суммы размеров с чертежаы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void getPolyLengthBtn_MouseDown(object sender, MouseButtonEventArgs e)
        {
            WindowState windowState = this.WindowState;

            this.WindowState = WindowState.Minimized;
            lengthFitting.Text = CommandClass.GetPolySize();
            this.WindowState = windowState;
        }


        


        /// <summary>
        /// При выборе диаметра пересчитывается площадь сечения
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void diamComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(diamComboBox.SelectedItem != null)
            {
                masOnePMComboBox.SelectedIndex = diamComboBox.SelectedIndex;
                PSechTextBox.Text = CalculationClass.GetPloshSech(((TextBlock)diamComboBox.SelectedItem).Text).ToString();
            }
        }


        /// <summary>
        /// рассчет массы при изменения значения в поле ввода
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lengthFitting_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (lengthFitting.Text != "0" || lengthFitting.Text != "")
                {
                    if (diamComboBox.SelectedItem != null)
                        resultTextBox.Text = CalculationClass.GetMass(((TextBlock)diamComboBox.SelectedItem).Text, lengthFitting.Text).ToString();
                    BufferClass.massFitting = resultTextBox.Text;
                }
            }
            catch (Exception) { }

        }


        /// <summary>
        /// Слайдер количества арматуры
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sliderCountFitting_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            try
            {
                if (Convert.ToInt32(countFittingTextBox.Text) > 10) BufferClass.countFitting = countFittingTextBox.Text;
                else
                {
                    countFittingTextBox.Text = sliderCountFitting.Value.ToString();
                    BufferClass.countFitting = countFittingTextBox.Text;
                }
            }
            catch(Exception) { }
        }

        /// <summary>
        /// Ограничение ввода символов в поле количества арматуры
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void countFittingTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            char number = e.Key.ToString().Last();


            if (!Char.IsDigit(number))
            {
                e.Handled = true;
            }
        }

        /// <summary>
        /// Пересчет результата в зависимости от введенног количества стержней
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void countFittingTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                BufferClass.countFitting = countFittingTextBox.Text;
                resultTextBox.Text = (Convert.ToDouble(BufferClass.massFitting) * Convert.ToInt32(BufferClass.countFitting)).ToString();
            }
            catch (Exception) { }
        }


        #endregion massTabContrl


        #region ploshTabControl ///////////

        /// <summary>
        /// Рассчет площади сечения в зависимости от количества стержней
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void countFittindComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            resultPloshSechOnStepTextBox.Text = CalculationClass.GetPlosh(Convert.ToDouble(((TextBlock)diamComboBox.SelectedItem).Text), Convert.ToDouble(BufferClass.countFitting)).ToString();

        }

        /// <summary>
        /// Рассчет площади сечения в зависимости от шага стержней
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void stepFittingComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            resultPloshSechOnStepTextBox.Text = CalculationClass.GetPlosh(Convert.ToDouble(((TextBlock)diamComboBox.SelectedItem).Text), Convert.ToDouble(BufferClass.countFitting), Convert.ToInt32(((TextBlock)stepFittingComboBox.SelectedItem).Text)).ToString();
        }


        #endregion ploshTabControl


    }
}
