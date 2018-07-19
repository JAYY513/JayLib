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
    [TemplatePart(Name = "PART_TextBox", Type = typeof(TextBox))]
    public class JSpinner : Control
    {
        #region Construction
        public JSpinner()
        {

        }

        static JSpinner()
        {
            InitializeCommands();

            DefaultStyleKeyProperty.OverrideMetadata(typeof(JSpinner), new FrameworkPropertyMetadata(typeof(JSpinner)));
        }

        #endregion
        
        #region Commands

        public static RoutedCommand IncreaseCommand { get; private set; } = null;
        public static RoutedCommand DecreaseCommand { get; private set; } = null;
        public static RoutedCommand DoubleClickCommand { get; private set; } = null;
        public static RoutedCommand EndEditingCommand { get; private set; } = null;
        private static void InitializeCommands()
        {
            //create instance
            IncreaseCommand = new RoutedCommand("IncreaseCommand", typeof(JSpinner));
            DecreaseCommand = new RoutedCommand("DecreaseCommand", typeof(JSpinner));
            DoubleClickCommand = new RoutedCommand("DoubleClickCommand", typeof(JSpinner));
            EndEditingCommand = new RoutedCommand("EndEditingCommand", typeof(JSpinner));
            //register the command bindings, if the button gets clicked, the method will be called.
            CommandManager.RegisterClassCommandBinding(typeof(JSpinner), new CommandBinding(IncreaseCommand, IncreaseExecute, CanIncrease));
            CommandManager.RegisterClassCommandBinding(typeof(JSpinner), new CommandBinding(DecreaseCommand, DecreaseExecute, CanDecrease));
            CommandManager.RegisterClassCommandBinding(typeof(JSpinner), new CommandBinding(DoubleClickCommand, DoubleClickExecute));
            CommandManager.RegisterClassCommandBinding(typeof(JSpinner), new CommandBinding(EndEditingCommand, EndEditingExecute));
            //  lastly bind some inputs:  i.e. if the user presses up/down arrow 
            //  keys, call the appropriate commands.
            CommandManager.RegisterClassInputBinding(typeof(JSpinner), new InputBinding(IncreaseCommand, new KeyGesture(Key.Right)));
            CommandManager.RegisterClassInputBinding(typeof(JSpinner), new InputBinding(IncreaseCommand, new KeyGesture(Key.Up)));
            CommandManager.RegisterClassInputBinding(typeof(JSpinner), new InputBinding(DecreaseCommand, new KeyGesture(Key.Left)));
            CommandManager.RegisterClassInputBinding(typeof(JSpinner), new InputBinding(DecreaseCommand, new KeyGesture(Key.Down)));
            CommandManager.RegisterClassInputBinding(typeof(JSpinner), new InputBinding(DoubleClickCommand, new MouseGesture(MouseAction.LeftDoubleClick)));
            CommandManager.RegisterClassInputBinding(typeof(JSpinner), new InputBinding(EndEditingCommand, new KeyGesture(Key.Enter)));
        }

        private static void CanIncrease(object sender, CanExecuteRoutedEventArgs e)
        {
            if (sender is JSpinner spinner && !spinner.IsEditing && spinner._TextList != null && spinner.Value < spinner._TextList.Count-1)
            {
                e.CanExecute = true;
            }
            else
            {
                e.CanExecute = false;
            }
        }

        private static void CanDecrease(object sender, CanExecuteRoutedEventArgs e)
        {
            if (sender is JSpinner spinner && !spinner.IsEditing && spinner._TextList != null && spinner.Value > 0)
            {
                e.CanExecute = true;
            }
            else
            {
                e.CanExecute = false;
            }
        }

        private static void EndEditingExecute(object sender, ExecutedRoutedEventArgs e)
        {
            if (sender is JSpinner spinner)
            {
                spinner.IsEditing = false;
                if (spinner._TextBox != null)
                {
                    spinner._TextBox.Focusable = false;
                    spinner.Value = spinner.CalculateValueFunction(spinner.InputText);
                }
            }
        }

        private static void DoubleClickExecute(object sender, ExecutedRoutedEventArgs e)
        {
            if (sender is JSpinner spinner && spinner.CanEdit)
            {
                spinner.IsEditing = true;
                if (spinner._TextBox != null)
                {
                    spinner._TextBox.Text = spinner.Text;
                    spinner._TextBox.Focus();
                    spinner._TextBox.SelectAll();
                }
            }
        }

        private static void DecreaseExecute(object sender, ExecutedRoutedEventArgs e)
        {
            if (sender is JSpinner spinner && spinner.Value <= spinner._TextList.Count - 1 && spinner.Value > 0)
            {
                spinner._ClickFireEvent = true;
                if (!spinner.IsUIFireEventDirectly)
                {
                    spinner.Value--;
                }
                else
                {
                    spinner.ValueChangedEvent?.Invoke(spinner, spinner.Value - 1);
                }
            }
        }

        private static void IncreaseExecute(object sender, ExecutedRoutedEventArgs e)
        {
            if (sender is JSpinner spinner && spinner.Value < spinner._TextList.Count - 1 && spinner.Value >= 0)
            {
                spinner._ClickFireEvent = true;
                if (!spinner.IsUIFireEventDirectly)
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
        
        #region Event
        public delegate void ValueChangedEventDelegate(JSpinner spinner, int value);
        public event ValueChangedEventDelegate ValueChangedEvent;
        #endregion

        #region DependencyPropertys
        /*
         * TickMark support
         *
         *   - bool      IsUIFireEventDirectly
         */
        #region Connection support
            
        public bool IsUIFireEventDirectly
        {
            get { return (bool)GetValue(IsUIFireEventDirectlyProperty); }
            set { SetValue(IsUIFireEventDirectlyProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsUIFireEventDirectly.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsUIFireEventDirectlyProperty =
            DependencyProperty.Register("IsUIFireEventDirectly", typeof(bool), typeof(JSpinner), new PropertyMetadata(false));
        #endregion
        /*
         * Edit support
         *
         *   - bool     CanEdit
         *   - bool     IsEditing
         *   - Func<string,int> CalculateValueFunction
         */
        #region Edit support
        public bool CanEdit
        {
            get { return (bool)GetValue(CanEditProperty); }
            set { SetValue(CanEditProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CanEdit.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CanEditProperty =
            DependencyProperty.Register("CanEdit", typeof(bool), typeof(JSlider), new PropertyMetadata(false));



        public bool IsEditing
        {
            get { return (bool)GetValue(IsEditingProperty); }
            set { SetValue(IsEditingProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsEditing.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsEditingProperty =
            DependencyProperty.Register("IsEditing", typeof(bool), typeof(JSpinner), new PropertyMetadata(false));


        
        public Func<string,int> CalculateValueFunction
        {
            get { return (Func<string,int>)GetValue(CalculateValueFunctionProperty); }
            set { SetValue(CalculateValueFunctionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CalculateValueFunction.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CalculateValueFunctionProperty =
            DependencyProperty.Register("CalculateValueFunction", typeof(Func<string,int>), typeof(JSpinner), new PropertyMetadata(new Func<string,int>(DefaultCalculateFunction)));

        private static int DefaultCalculateFunction(string arg)
        {
            return 0;
        }
        #endregion

        /*
         * Value and text
         *
         *   - int     Value
         *   - IEumerable     ItemSource
         *   - string       Text
         *   - string       InputText
         */
        #region Value and Text

        public int Value
        {
            get { return (int)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Value.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(int), typeof(JSpinner), new FrameworkPropertyMetadata(-1, FrameworkPropertyMetadataOptions.Journal, OnValueChanged, OnValueChangedCallBack));

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
                if (!spinner.IsUIFireEventDirectly)
                {
                    spinner.ValueChangedEvent?.Invoke(spinner, newValue);
                }
                if (newValue == -1 || spinner._TextList == null)
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
            DependencyProperty.Register("ItemsSource", typeof(IEnumerable), typeof(JSpinner), new PropertyMetadata(null, OnItemsSourceChanged));

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


        public string InputText
        {
            get { return (string)GetValue(InputTextProperty); }
            set { SetValue(InputTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for InputText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty InputTextProperty =
            DependencyProperty.Register("InputText", typeof(string), typeof(JSpinner), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        #endregion
        
        #endregion

        #region Private fields
        private List<string> _TextList = null;
        private bool _ClickFireEvent = false;
        private TextBox _TextBox = null;
        private const string TextBoxName = "PART_TextBox";
        #endregion

        #region Private methods
        private void GetNewCollection()
        {
            Value = 0;
            Text = _TextList.First();
        }
        #endregion

        #region Override methods
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _TextBox = GetTemplateChild(TextBoxName) as TextBox;
        }
        #endregion
    }
}
