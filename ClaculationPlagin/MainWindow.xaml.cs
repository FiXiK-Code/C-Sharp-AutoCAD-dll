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
        private bool historyOpen = true;
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Поле входных значений
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void inputTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if(inputTextBox.Text.Last() == ')')
                {
                    string value = "";
                    bool func = false;
                    for (int i = inputTextBox.Text.Length - 1; i >= 0; i--)
                    {
                        if (inputTextBox.Text[i] == '(') func = true;
                        if (func)
                        {
                            if (inputTextBox.Text[i] != '+' && inputTextBox.Text[i] != '-' && inputTextBox.Text[i] != '*' && inputTextBox.Text[i] != '/')
                            {
                                value = inputTextBox.Text[i] + value;
                            }
                            else
                            {
                                break;
                            }
                        }
                        else
                        {
                            value = inputTextBox.Text[i] + value;
                        }
                    }
                    inputTextBox.Text = inputTextBox.Text.Replace(value, CalculationClass.FunctionCalculation(value).ToString());
                }
            }catch(Exception) { }    
        }

        #region AlterFunction /////////

        /// <summary>
        /// Метод для вставки записи в журнал. При передаче одно из параметров "null", будет вставлен отолько один. При передаче двух - первый пример, второй результат.
        /// </summary>
        /// <param name="text1"></param>
        /// <param name="text2"></param>
        private void AddHistoryBlock(string text1, string text2)
        {
            try
            {
                var newElement = (StackPanel)((DataTemplate)FindResource("HistoryContentTemplate")).LoadContent();
                var val = BufferClass.SetTemlate(newElement, text1, text2);
                historyContentBlock.Children.Add(val);
            }
            catch (Exception) { }
        }

        /// <summary>
        /// Открытие окна для выбора формулы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void functionBtn_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                WindowState windowState = this.WindowState;

                this.WindowState = WindowState.Minimized;

                FormulWindow formul = new FormulWindow();
                formul.ShowDialog();

                inputTextBox.Text += BufferClass.formul != null ? BufferClass.formul : "";
                BufferClass.formul = null;

                this.WindowState = windowState;
            }
            catch (Exception) { }
            

        }

        /// <summary>
        /// Кнопка журнала
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void history_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (historyOpen)
                {
                    contentPanel.Width = 250;
                    history.Margin = new Thickness(90, 0, 0, 0);
                    historyOpen = false;
                }
                else
                {
                    contentPanel.Width = 450;
                    history.Margin = new Thickness(290, 0, 0, 0);
                    historyOpen = true;
                }
            }
            catch (Exception) { }
            

        }

        /// <summary>
        /// Очистка истории
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void remuveHistory_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                historyContentBlock.Children.Clear();
            }
            catch (Exception) { }
        }

        /// <summary>
        /// Вставка примера из журнала
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void historyContent_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                inputTextBox.Text = ((TextBlock)sender).Text;
            }
            catch (Exception) { }
           
        }

        /// <summary>
        /// Вставка рзультата из журнала
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void historyResult_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                resultTextBox.Text = ((TextBlock)sender).Text;
            }
            catch (Exception) { }
            
        }

        /// <summary>
        /// Закрытие окна
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Close_Click(object sender, MouseButtonEventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Добавление префикса к втавке зачения
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pref_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                BufferClass.pref = pref.Text;
            }
            catch (Exception) { }
            
        }

        /// <summary>
        /// Добавление суффикса в вставке значения
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void suff_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                BufferClass.suff = suff.Text;
            }
            catch (Exception) { }
           
        }

        /// <summary>
        /// Добавление знаков после запятой
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void plusRound_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                DotPositionTextBox.Text = (Convert.ToInt32(DotPositionTextBox.Text) + 1).ToString();
            }
            catch (Exception) { }
        }

        /// <summary>
        /// Уменьшение знаков после запятой
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void minusRound_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                DotPositionTextBox.Text = (Convert.ToInt32(DotPositionTextBox.Text) >= 1 ? Convert.ToInt32(DotPositionTextBox.Text) - 1 : Convert.ToInt32(DotPositionTextBox.Text)).ToString();

            }
            catch (Exception) { }
        }

        /// <summary>
        /// Cath изменения количества знаков после запятой
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DotPositionTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                BufferClass.roundItem = Convert.ToInt32(DotPositionTextBox.Text);
            }
            catch (Exception) { }
            
        }

        /// <summary>
        /// Скрытие появление кнопок для получения данных с чертежа
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void inoutDataSection_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                inputButton.Height = inputButton.Height == 22 ? 0 : 22;
                RotateTransform rotateTransform = new RotateTransform(inputButton.Height == 22 ? 0 : 180);

                inoutDataSection.RenderTransform = rotateTransform;
            } catch (Exception) { }
            
        }

        /// <summary>
        /// Несколько значений
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void multyCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (multyCheckBox.IsChecked == true)
                {
                    BufferClass.multyItem = true;
                }
                else
                {
                    BufferClass.multyItem = false;
                }
            }
            catch (Exception) { }
            
        }

        /// <summary>
        /// Округление
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void roundCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (roundCheckBox.IsChecked == true)
                {
                    BufferClass.round = true;
                }
                else
                {
                    BufferClass.round = false;
                }
            }
            catch (Exception) { }
           
        }

        /// <summary>
        /// Знак разделителя при выборе нескольких значений
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void symbolMultyComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                switch (symbolMultyComboBox.SelectedIndex)
                {
                    case 0:
                        BufferClass.znac = "*";
                        break;
                    case 1:
                        BufferClass.znac = "/";
                        break;
                    case 2:
                        BufferClass.znac = "+";
                        break;
                    case 3:
                        BufferClass.znac = "-";
                        break;
                }
            }
            catch (Exception) { }
            
        }

        #endregion ///////////

        #region InputAutoCad /////////////


        /// <summary>
        /// Получение расстояния между точками
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lengthInput_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                string length;
                if (!BufferClass.multyItem)
                {
                    WindowState windowState = this.WindowState;

                    this.WindowState = WindowState.Minimized;
                    length = CommandClass.GetSize(null);
                    this.WindowState = windowState;

                }
                else
                {
                    WindowState windowState = this.WindowState;

                    this.WindowState = WindowState.Minimized;
                    length = CommandClass.GetSize(BufferClass.znac);
                    this.WindowState = windowState;

                }
                try
                {
                    length = CalculationClass.Calculate(length.ToString()).ToString();
                    inputTextBox.Text += BufferClass.round ? Math.Round(Convert.ToDouble(length), BufferClass.roundItem).ToString() : length;
                }
                catch (Exception) { inputTextBox.Text += length; }
            }
            catch (Exception) { }
            


            
        }

        /// <summary>
        /// Получение суммы длин из объектов
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sumLengthInput_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                WindowState windowState = this.WindowState;

                this.WindowState = WindowState.Minimized;
                string length = CommandClass.GetPolySize();
                this.WindowState = windowState;

                inputTextBox.Text += BufferClass.round ? Math.Round(Convert.ToDouble(length), BufferClass.roundItem).ToString() : length;
            }
            catch (Exception) { }
            
        }

        /// <summary>
        /// Поличение координаты
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void coordinateInput_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                string nap = "x";
                string coordinate;
                if (!BufferClass.multyItem)
                {
                    WindowState windowState = this.WindowState;

                    this.WindowState = WindowState.Minimized;
                    coordinate = CommandClass.GetCoordinate(nap, null);
                    this.WindowState = windowState;

                }
                else
                {
                    WindowState windowState = this.WindowState;

                    this.WindowState = WindowState.Minimized;
                    coordinate = CommandClass.GetCoordinate(nap, BufferClass.znac);
                    this.WindowState = windowState;

                }
                try
                {
                    coordinate = CalculationClass.Calculate(coordinate.ToString()).ToString();
                    inputTextBox.Text += BufferClass.round ? Math.Round(Convert.ToDouble(coordinate), BufferClass.roundItem).ToString() : coordinate;
                }
                catch (Exception) { inputTextBox.Text += coordinate; }
            }
            catch (Exception) { }
            

            
        }

        /// <summary>
        /// Получени площади
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ploshInput_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                string length;
                if (!BufferClass.multyItem)
                {
                    WindowState windowState = this.WindowState;

                    this.WindowState = WindowState.Minimized;
                    length = CommandClass.GetPlosh(null);
                    this.WindowState = windowState;

                }
                else
                {
                    WindowState windowState = this.WindowState;

                    this.WindowState = WindowState.Minimized;
                    length = CommandClass.GetPlosh(BufferClass.znac);
                    this.WindowState = windowState;

                }

                try
                {
                    length = CalculationClass.Calculate(length.ToString()).ToString();
                    inputTextBox.Text += BufferClass.round ? Math.Round(Convert.ToDouble(length), BufferClass.roundItem).ToString() : length;
                }
                catch (Exception) { inputTextBox.Text += length; }
            }
            catch (Exception) { }
            
        }

        /// <summary>
        /// Получение значения из (мульти)-текста
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textInput_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                string length;
                if (!BufferClass.multyItem)
                {
                    WindowState windowState = this.WindowState;

                    this.WindowState = WindowState.Minimized;
                    length = CommandClass.GetTextValue(true, null);
                    this.WindowState = windowState;

                }
                else
                {
                    WindowState windowState = this.WindowState;

                    this.WindowState = WindowState.Minimized;
                    length = CommandClass.GetTextValue(true, BufferClass.znac);
                    this.WindowState = windowState;

                }

                try
                {
                    length = CalculationClass.Calculate(length.ToString()).ToString();
                    inputTextBox.Text += BufferClass.round ? Math.Round(Convert.ToDouble(length), BufferClass.roundItem).ToString() : length;
                }
                catch (Exception) { inputTextBox.Text += length; }
            }
            catch (Exception) { }
            
        }

        /// <summary>
        /// Получение значения из выноски
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void vinosInput_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                string length;
                if (!BufferClass.multyItem)
                {
                    WindowState windowState = this.WindowState;

                    this.WindowState = WindowState.Minimized;
                    length = CommandClass.GetTextValue(false, null);
                    this.WindowState = windowState;

                }
                else
                {
                    WindowState windowState = this.WindowState;

                    this.WindowState = WindowState.Minimized;
                    length = CommandClass.GetTextValue(false, BufferClass.znac);
                    this.WindowState = windowState;

                }
                try
                {
                    length = CalculationClass.Calculate(length.ToString()).ToString();
                    inputTextBox.Text += BufferClass.round ? Math.Round(Convert.ToDouble(length), BufferClass.roundItem).ToString() : length;
                }
                catch (Exception) { inputTextBox.Text += length; }
            }
            catch (Exception) { }
            
        }

        /// <summary>
        /// Получени значения из таблицы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tableInput_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                string length;
                if (!BufferClass.multyItem)
                {
                    WindowState windowState = this.WindowState;

                    this.WindowState = WindowState.Minimized;
                    length = CommandClass.GetTableValue(null);
                    this.WindowState = windowState;

                }
                else
                {
                    WindowState windowState = this.WindowState;

                    this.WindowState = WindowState.Minimized;
                    length = CommandClass.GetTableValue(BufferClass.znac);
                    this.WindowState = windowState;

                }
                try
                {
                    length = CalculationClass.Calculate(length.ToString()).ToString();
                    inputTextBox.Text += BufferClass.round ? Math.Round(Convert.ToDouble(length), BufferClass.roundItem).ToString() : length;
                }
                catch (Exception) { inputTextBox.Text += length; }
            }
            catch (Exception) { }
            
        }

        /// <summary>
        /// Получение данных из размера
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dimensionInput_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                string length;
                if (!BufferClass.multyItem)
                {
                    WindowState windowState = this.WindowState;

                    this.WindowState = WindowState.Minimized;
                    length = CommandClass.GetDimension(null);
                    this.WindowState = windowState;
                }
                else
                {
                    WindowState windowState = this.WindowState;

                    this.WindowState = WindowState.Minimized;
                    length = CommandClass.GetDimension(BufferClass.znac);
                    this.WindowState = windowState;
                }
                try
                {
                    length = CalculationClass.Calculate(length.ToString()).ToString();
                    inputTextBox.Text += BufferClass.round ? Math.Round(Convert.ToDouble(length), BufferClass.roundItem).ToString() : length;
                }
                catch (Exception) { inputTextBox.Text += length; }
            }
            catch (Exception) { }
            
        }


        #endregion //////////

        #region InsertValue /////////

        /// <summary>
        /// Вставка однострочного текста
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void onseTextOutput_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                string result = resultTextBox.Text;
                WindowState windowState = this.WindowState;

                this.WindowState = WindowState.Minimized;
                CommandClass.InsertOneText(BufferClass.pref + " " + result + " " + BufferClass.suff);
                this.WindowState = windowState;
            }
            catch (Exception) { }
            
            
        }

        /// <summary>
        /// Добавить результат к однострочному тексту
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void plusOnseTextOutput_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                string result = resultTextBox.Text;
                WindowState windowState = this.WindowState;

                this.WindowState = WindowState.Minimized;
                CommandClass.ReplaceOneText(BufferClass.pref + " " + result + " " + BufferClass.suff);
                this.WindowState = windowState;
            }
            catch (Exception) { }
            
            
        }

        /// <summary>
        /// Вставка результата в таблицу
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tableOutput_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                string result = resultTextBox.Text;
                WindowState windowState = this.WindowState;

                this.WindowState = WindowState.Minimized;
                CommandClass.InsertTableResult(BufferClass.pref + " " + result + " " + BufferClass.suff);
                this.WindowState = windowState;
            }
            catch (Exception) { }
           
            
        }

        /// <summary>
        /// Добавить результат к многострочному тексту
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void multyTextOutput_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                string result = resultTextBox.Text;
                WindowState windowState = this.WindowState;

                this.WindowState = WindowState.Minimized;
                CommandClass.InsertPolyText(BufferClass.pref + " " + result + " " + BufferClass.suff);
                this.WindowState = windowState;
            }
            catch (Exception) { }
            
            
        }

        /// <summary>
        /// Резильтат в виде выноски
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void vinosOuput_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                string result = resultTextBox.Text;
                WindowState windowState = this.WindowState;

                this.WindowState = WindowState.Minimized;
                CommandClass.LeaderInsert(BufferClass.pref + " " + result + " " + BufferClass.suff);
                this.WindowState = windowState;
            }
            catch (Exception) { }
            
            
        }

        #endregion //////////////

        #region ButtonPanel //////////

        /// <summary>
        /// Вставка значений из кнопок не требующих преобразования
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NumButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                inputTextBox.Text += ((Button)sender).Content.ToString();
            }
            catch (Exception) { }
            
        }

        /// <summary>
        /// Вставка значений из кнопок с необходимостью добавления скобки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormulButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                inputTextBox.Text += ((Button)sender).Content.ToString() + "(";
            }
            catch (Exception) { }
            
        }

        /// <summary>
        /// Преобразовании чисел (квадрат, корень, синус и тд.)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Func_Click(object sender, RoutedEventArgs e)
        {
            if (resultTextBox.Text != "" && resultTextBox.Text != "0")
            {
                try
                {
                    switch (((Button)sender).Content.ToString())
                    {
                        case "sin":
                            resultTextBox.Text = Math.Sin(Convert.ToDouble(resultTextBox.Text)).ToString();
                            break;
                        case "cos":
                            resultTextBox.Text = Math.Cos(Convert.ToDouble(resultTextBox.Text)).ToString();
                            break;
                        case "tg":
                            resultTextBox.Text = Math.Tan(Convert.ToDouble(resultTextBox.Text)).ToString();
                            break;
                        case "1/x":
                            resultTextBox.Text = (1 / Convert.ToDouble(resultTextBox.Text)).ToString();
                            break;
                        case "x²":
                            resultTextBox.Text = Math.Pow(Convert.ToDouble(resultTextBox.Text), 2).ToString();
                            break;
                        case "x³":
                            resultTextBox.Text = Math.Pow(Convert.ToDouble(resultTextBox.Text), 3).ToString();
                            break;
                        case "√":
                            resultTextBox.Text = Math.Sqrt(Convert.ToDouble(resultTextBox.Text)).ToString();
                            break;
                    }
                }catch (Exception)
                {
                    MessageBox.Show("Не корректное значение  вполе ответа!");
                }
                
            }
            else
            {
                try
                {
                    string num = "";
                    int startIndex = -1;
                    bool end = false;
                    var text = inputTextBox.Text;
                    for (int i = text.Length - 1; i >= 0; i--)
                    {
                        try
                        {
                            if (text[i] != '.')
                            {
                                num = Convert.ToInt32(text[i].ToString()) + num;
                                end = true;
                            }
                            else if (text[i] == '.')
                            {
                                num = '.' + num;
                                end = true;
                            }
                        }
                        catch (Exception)
                        {
                            if (end)
                            {
                                startIndex = i + 1;
                                break;
                            }
                        }
                    }
                    var newNum = num;
                    startIndex = startIndex == -1 ? 0 : startIndex;
                    switch (((Button)sender).Content.ToString())
                    {
                        case "sin":
                            newNum = Math.Sin(Convert.ToDouble(num)).ToString();
                            inputTextBox.Text = inputTextBox.Text.Remove(startIndex, num.Length);
                            inputTextBox.Text = inputTextBox.Text.Insert(startIndex, newNum);
                            break;
                        case "cos":
                            newNum = Math.Cos(Convert.ToDouble(num)).ToString();
                            inputTextBox.Text = inputTextBox.Text.Remove(startIndex, num.Length);
                            inputTextBox.Text = inputTextBox.Text.Insert(startIndex, newNum);
                            break;
                        case "tg":
                            newNum = Math.Tan(Convert.ToDouble(num)).ToString();
                            inputTextBox.Text = inputTextBox.Text.Remove(startIndex, num.Length);
                            inputTextBox.Text = inputTextBox.Text.Insert(startIndex, newNum);
                            break;
                        case "1/x":
                            newNum = (1 / Convert.ToDouble(num)).ToString();
                            inputTextBox.Text = inputTextBox.Text.Remove(startIndex, num.Length);
                            inputTextBox.Text = inputTextBox.Text.Insert(startIndex, newNum);
                            break;
                        case "x²":
                            newNum = Math.Pow(Convert.ToDouble(num), 2).ToString();
                            inputTextBox.Text = inputTextBox.Text.Remove(startIndex, num.Length);
                            inputTextBox.Text = inputTextBox.Text.Insert(startIndex, newNum);
                            break;
                        case "x³":
                            newNum = Math.Pow(Convert.ToDouble(num), 3).ToString();
                            inputTextBox.Text = inputTextBox.Text.Remove(startIndex, num.Length);
                            inputTextBox.Text = inputTextBox.Text.Insert(startIndex, newNum);
                            break;
                        case "√":
                            newNum = Math.Sqrt(Convert.ToDouble(num)).ToString();
                            inputTextBox.Text = inputTextBox.Text.Remove(startIndex, num.Length);
                            inputTextBox.Text = inputTextBox.Text.Insert(startIndex, newNum);
                            break;
                    }
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
                

            }
        }

        /// <summary>
        /// Добавление/удаление знака "-" в результате
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PM_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                resultTextBox.Text = resultTextBox.Text[0] != '-' ? '-' + resultTextBox.Text : resultTextBox.Text.Remove(0, 1);
            }
            catch (Exception) { }
        }

        /// <summary>
        /// Удаление последнего символа в строке ввода
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StrButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                inputTextBox.Text = inputTextBox.Text.Length != 0 ? inputTextBox.Text.Remove(inputTextBox.Text.Length - 1, 1) : inputTextBox.Text;
            }
            catch (Exception) { }
            
        }

        /// <summary>
        /// Очистка строки ввода
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RemoveInput_Click(object sender, RoutedEventArgs e)
        {
            inputTextBox.Text = "";
        }

        /// <summary>
        /// Начало вычисления (Enter or "="_Click)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StrtCalc_Click(object sender, RoutedEventArgs e)
        {
            double calculate = 0.0;
            try
            {
                calculate = CalculationClass.Calculate(inputTextBox.Text);
                resultTextBox.Text = BufferClass.round ? Math.Round(calculate, BufferClass.roundItem).ToString() : calculate.ToString();
                AddHistoryBlock(inputTextBox.Text, resultTextBox.Text);
            }
            catch (Exception) { MessageBox.Show("Ошибка входных данных"); }
            
        }

        /// <summary>
        /// обработка нажатия Enter для начала вычислений
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void inputTextBox_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.Key == Key.Enter)
            {
                double calculate = 0.0;
                try
                {
                    calculate = CalculationClass.Calculate(inputTextBox.Text);
                    resultTextBox.Text = BufferClass.round ? Math.Round(calculate, BufferClass.roundItem).ToString() : calculate.ToString();
                    AddHistoryBlock(inputTextBox.Text, resultTextBox.Text);
                }
                catch (Exception) { MessageBox.Show("Ошибка входных данных"); }
            }
        }

        #endregion //////////////

    }
}
