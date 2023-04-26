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
        #region massTabContrl
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //CommandClass.RunCommand2();
        }

        private void sizeBtn_Click(object sender, RoutedEventArgs e)
        {
            lengthFitting.Text = CommandClass.GetSize();
        }

        private void polySizeBtn_Click(object sender, RoutedEventArgs e)
        {
            lengthFitting.Text = CommandClass.GetPolySize();
        }


        private void onePMCheckBox_Click(object sender, RoutedEventArgs e)
        {
            // добавить передачу погонного метра (как)

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
                resStackPanel.Height = 17.6;
                resStackPanel.IsEnabled = true;
                lengthFitting.IsEnabled = true;
                BufferClass.massFitting = resultTextBox.Text;
            }
        }

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

        private void resToTableLink_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // вызов функции выбора ячейки таблицы (для массы) (передавать в парметры BufferClass.massFitting)
            // добавить учет галочки 1пм
            CommandClass.InsertTableText(BufferClass.massFitting, false);
        }

        private void nameToTableLink_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //  вызов функции выбора ячейки таблицы (для названия - по ГОСТу) (передавать в парметры BufferClass.nameFitting <проверка на символ>)
        }

        private void resAndNameToTableLink_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // вызов функции для вставки наименования и массы (отдельная функция - смотреть ТЗ)
        }
        #endregion

        private void diamComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(diamComboBox.SelectedItem != null)
            {
                masOnePMComboBox.SelectedIndex = diamComboBox.SelectedIndex;
                PSechTextBox.Text = (Math.Round((Math.PI * Math.Pow(Convert.ToInt32(((TextBlock)diamComboBox.SelectedItem).Text) ,2) / 4 ),1)/100). ToString();
            }
        }

        private void lengthFitting_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (lengthFitting.Text != "0" || lengthFitting.Text != "")
                {
                    if (diamComboBox.SelectedItem != null)
                        resultTextBox.Text = (Math.Round(Math.PI * Math.Pow(Convert.ToInt32(((TextBlock)diamComboBox.SelectedItem).Text), 2) / 4 * 0.7850 * Convert.ToInt32(lengthFitting.Text) / 100000 , 3) * Convert.ToInt32(BufferClass.countFitting)).ToString();
                    BufferClass.massFitting = resultTextBox.Text;
                }
            }catch(Exception) { }
            
        }

        private void sliderCountFitting_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            try
            {
                countFittingTextBox.Text = sliderCountFitting.Value.ToString();
                BufferClass.countFitting = countFittingTextBox.Text;
            }
            catch(Exception) { }
        }

        private void countFittingTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                resultTextBox.Text = (Convert.ToDouble(BufferClass.massFitting) * Convert.ToInt32(BufferClass.countFitting)).ToString();
            }
            catch(Exception) { }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
