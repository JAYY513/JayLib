using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace JayCustomControlLib
{
    /// <summary>
    /// 依照步驟 1a 或 1b 執行，然後執行步驟 2，以便在 XAML 檔中使用此自訂控制項。
    ///
    /// 步驟 1a) 於存在目前專案的 XAML 檔中使用此自訂控制項。
    /// 加入此 XmlNamespace 屬性至標記檔案的根項目為 
    /// 要使用的: 
    ///
    ///     xmlns:MyNamespace="clr-namespace:JayCustomControlLib"
    ///
    ///
    /// 步驟 1b) 於存在其他專案的 XAML 檔中使用此自訂控制項。
    /// 加入此 XmlNamespace 屬性至標記檔案的根項目為 
    /// 要使用的: 
    ///
    ///     xmlns:MyNamespace="clr-namespace:JayCustomControlLib;assembly=JayCustomControlLib"
    ///
    /// 您還必須將 XAML 檔所在專案的專案參考加入
    /// 此專案並重建，以免發生編譯錯誤: 
    ///
    ///     在 [方案總管] 中以滑鼠右鍵按一下目標專案，並按一下
    ///     [加入參考]->[專案]->[瀏覽並選取此專案]
    ///
    ///
    /// 步驟 2)
    /// 開始使用 XAML 檔案中的控制項。
    ///
    ///     <MyNamespace:JSpinner/>
    ///
    /// </summary>
    public class JSpinner : Control
    {

        static JSpinner()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(JSpinner), new FrameworkPropertyMetadata(typeof(JSpinner)));
        }

        #region Event
        public delegate void ValueChangedEventDelegate(JSpinner spinner, int value);
        public event ValueChangedEventDelegate ValueChangedEvent;
        #endregion

        #region DependencyPropertys

        #region Connection support
        /*
         * TickMark support
         *
         *   - bool             IsOnlyClickFireEvent
         *   - bool             ClickUpateValue
         */

        public bool IsOnlyClickFireEvent
        {
            get { return (bool)GetValue(IsOnlyClickFireEventProperty); }
            set { SetValue(IsOnlyClickFireEventProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsOnlyClickFireEvent.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsOnlyClickFireEventProperty =
            DependencyProperty.Register("IsOnlyClickFireEvent", typeof(bool), typeof(JSpinner), new PropertyMetadata(false));



        public bool ClickUpateValue
        {
            get { return (bool)GetValue(IsOnlyClickUpateValueProperty); }
            set { SetValue(IsOnlyClickUpateValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsOnlyClickUpateValue.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsOnlyClickUpateValueProperty =
            DependencyProperty.Register("ClickUpateValue", typeof(bool), typeof(JSpinner), new PropertyMetadata(true));

        #endregion



        public bool IsReadOnly
        {
            get { return (bool)GetValue(IsReadOnlyProperty); }
            set { SetValue(IsReadOnlyProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsReadOnly.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsReadOnlyProperty =
            DependencyProperty.Register("IsReadOnly", typeof(bool), typeof(JSpinner), new PropertyMetadata(true));



        public int Value
        {
            get { return (int)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Value.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(int), typeof(JSpinner), new PropertyMetadata(-1, OnValueChanged, OnValueChangedCallBack));

        private static object OnValueChangedCallBack(DependencyObject d, object baseValue)
        {
            if (d is JSpinner spinner && spinner.ItemsSource != null)
            {
                if ((int)baseValue == -1)
                {
                    return -1;
                }
                else
                {
                    int newValue = (int)baseValue;
                    newValue = Math.Max(0, Math.Min(newValue, spinner._TextList.Count - 1));
                    return newValue;
                }
            }
            else
            {
                return 0;
            }
        }

        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is JSpinner spinner)
            {
                int newValue = (int)e.NewValue;
                if (!spinner.IsOnlyClickFireEvent)
                {
                    spinner.ValueChangedEvent?.Invoke(spinner, newValue);
                }
                else if (spinner._ClickFireEvent)
                {
                    spinner.ValueChangedEvent?.Invoke(spinner, newValue);
                    spinner._ClickFireEvent = false;
                }
                if (newValue == -1)
                {
                    spinner.Text = string.Empty;
                }
                else
                {
                    spinner.Text = spinner._TextList[newValue].ToString();
                }

            }
        }



        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Text.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(JSpinner), new PropertyMetadata(default(string)));



        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(TextCollectionProperty); }
            set { SetValue(TextCollectionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TextCollection.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextCollectionProperty =
            DependencyProperty.Register("ItemsSource", typeof(IEnumerable), typeof(JSpinner), new PropertyMetadata(default(IEnumerable), OnItemsSourceChanged));

        private static void OnItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            JSpinner spinner = d as JSpinner;
            if (e.NewValue is IEnumerable<string> newCollection && newCollection != null)
            {
                spinner._TextList = newCollection.ToList();
                spinner.GetNewCollection();
            }
            else if (e.NewValue is IEnumerable enumerable && enumerable != null)
            {
                spinner._TextList = new List<string>();
                foreach (var item in enumerable)
                {
                    spinner._TextList.Add(item.ToString());
                }
                spinner.GetNewCollection();
            }
        }



        public ICommand IncreaseCommand
        {
            get { return (ICommand)GetValue(IncreaseCommandProperty); }
            set { SetValue(IncreaseCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IncreaseCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IncreaseCommandProperty =
            DependencyProperty.Register("IncreaseCommand", typeof(ICommand), typeof(JSpinner), new PropertyMetadata(null));



        public ICommand DecreaseCommand
        {
            get { return (ICommand)GetValue(DecreaseCommandProperty); }
            set { SetValue(DecreaseCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DecreaseCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DecreaseCommandProperty =
            DependencyProperty.Register("DecreaseCommand", typeof(ICommand), typeof(JSpinner), new PropertyMetadata(null));



        public ICommand DoubleClickCommand
        {
            get { return (ICommand)GetValue(DoubleClickCommandProperty); }
            set { SetValue(DoubleClickCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DoubleClickCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DoubleClickCommandProperty =
            DependencyProperty.Register("DoubleClickCommand", typeof(ICommand), typeof(JSpinner), new PropertyMetadata(null));



        #endregion

        #region Command
        
        private void InitializeCommands()
        {
            //create instance
            IncreaseCommand = new RoutedCommand("IncreaseCommand", typeof(JSpinner));
            DecreaseCommand = new RoutedCommand("DecreaseCommand", typeof(JSpinner));
            DoubleClickCommand = new RoutedCommand("DoubleClickCommand", typeof(JSpinner));

            //register the command bindings, if the button gets clicked, the method will be called.
            CommandManager.RegisterClassCommandBinding(typeof(JSpinner), new CommandBinding(IncreaseCommand, IncreaseExecute));
            CommandManager.RegisterClassCommandBinding(typeof(JSpinner), new CommandBinding(DecreaseCommand, DecreaseExecute));
            CommandManager.RegisterClassCommandBinding(typeof(JSpinner), new CommandBinding(DoubleClickCommand, DoubleClickExecute));

            //  lastly bind some inputs:  i.e. if the user presses up/down arrow 
            //  keys, call the appropriate commands.
            CommandManager.RegisterClassInputBinding(typeof(JSpinner), new InputBinding(IncreaseCommand, new KeyGesture(Key.Right)));
            CommandManager.RegisterClassInputBinding(typeof(JSpinner), new InputBinding(IncreaseCommand, new KeyGesture(Key.Up)));
            CommandManager.RegisterClassInputBinding(typeof(JSpinner), new InputBinding(DecreaseCommand, new KeyGesture(Key.Left)));
            CommandManager.RegisterClassInputBinding(typeof(JSpinner), new InputBinding(DecreaseCommand, new KeyGesture(Key.Down)));
        }
        
        private void DoubleClickExecute(object sender, ExecutedRoutedEventArgs e)
        {
            if (sender is JSpinner spinner)
            {
                spinner.IsReadOnly = false;
                if (e.Parameter is TextBox textBox)
                {
                    textBox.Focusable = true;
                    textBox.Focus();
                    textBox.SelectAll();
                }
            }
        }

        private void DecreaseExecute(object sender, ExecutedRoutedEventArgs e)
        {
            if (sender is JSpinner spinner && spinner.Value <= spinner._TextList.Count - 1 && spinner.Value > 0)
            {
                _ClickFireEvent = true;
                if (spinner.ClickUpateValue)
                {
                    spinner.Value--;
                }
                else
                {
                    spinner.ValueChangedEvent?.Invoke(spinner, spinner.Value - 1);
                }
            }
        }

        private void IncreaseExecute(object sender, ExecutedRoutedEventArgs e)
        {
            if (sender is JSpinner spinner && spinner.Value < spinner._TextList.Count - 1 && spinner.Value >= 0)
            {
                _ClickFireEvent = true;
                if (spinner.ClickUpateValue)
                {
                    spinner.Value++;
                }
                else
                {
                    spinner.ValueChangedEvent?.Invoke(spinner, spinner.Value + 1);
                }
            }
        }
        #endregion

        #region Private fields
        private IList _TextList = null;
        private bool _ClickFireEvent = false;
        #endregion
        #region Private methods
        private void GetNewCollection()
        {
            Value = 0;
        }
        #endregion

        #region Construction
        public JSpinner()
        {
            InitializeCommands();
        }
        #endregion
        
    }
}
