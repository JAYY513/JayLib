using JayCustomControlLib.CommonBasicClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
    ///     <MyNamespace:JSwitcher/>
    ///
    /// </summary>
    public class JSwitcher : ToggleButton
    {
        static JSwitcher()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(JSwitcher), new FrameworkPropertyMetadata(typeof(JSwitcher)));
        }
        #region Command
        public static RoutedCommand ChangeContentCommand { get; private set; } = null;
        static void InitializeCommands()
        {
            ChangeContentCommand = new RoutedCommand("ChangeContentCommand",typeof(JSwitcher));
            MyCommandHelper.RegisterCommandHandler(typeof(JSwitcher), ChangeContentCommand, new ExecutedRoutedEventHandler(ChangeContentExecute), new MouseGesture(MouseAction.LeftDoubleClick));
        }

        private static void ChangeContentExecute(object sender, ExecutedRoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
        #endregion
        #region Propertys and Events


        public Brush ActiveBrush
        {
            get { return (Brush)GetValue(ActiveBrushProperty); }
            set { SetValue(ActiveBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ActiveBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ActiveBrushProperty =
            DependencyProperty.Register("ActiveBrush", typeof(Brush), typeof(JSwitcher), new PropertyMetadata(Brushes.Red));



        public Brush InactiveBrush
        {
            get { return (Brush)GetValue(InactiveBrushProperty); }
            set { SetValue(InactiveBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for InactiveBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty InactiveBrushProperty =
            DependencyProperty.Register("InactiveBrush", typeof(Brush), typeof(JSwitcher), new PropertyMetadata(Brushes.Red));



        public string GroupName
        {
            get { return (string)GetValue(GroupNameProperty); }
            set { SetValue(GroupNameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for GroupName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty GroupNameProperty =
            DependencyProperty.Register("GroupName", typeof(string), typeof(JSwitcher), new PropertyMetadata(string.Empty,OnGroupNameChanged));

        private static void OnGroupNameChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is JSwitcher switcher)
            {
                if (e.NewValue is string newGroup && !string.IsNullOrWhiteSpace(newGroup))
                {
                    if (JSwitcherGroup.TryGetValue(newGroup,out List<JSwitcher> switchers))
                    {
                        if (!switchers.Contains(switcher))
                        {
                            switchers.Add(switcher);
                        }
                    }
                    else
                    {
                        JSwitcherGroup[newGroup] = new List<JSwitcher>() { switcher };
                    }
                }
                if (e.OldValue is string oldGroup && !string.IsNullOrWhiteSpace(oldGroup))
                {
                    if (JSwitcherGroup.TryGetValue(oldGroup,out List<JSwitcher> switchers))
                    {
                        if (switchers.Contains(switcher))
                        {
                            switchers.Remove(switcher);
                        }
                    }
                }
            }
        }
        
        #endregion

        #region Static Propertys
        public static Dictionary<string, List<JSwitcher>> JSwitcherGroup { get; set; } = new Dictionary<string, List<JSwitcher>>();
        #endregion

        #region Override methods
        protected override void OnChecked(RoutedEventArgs e)
        {
            UpdateSwitcherGroup();
            base.OnChecked(e);
        }
        #endregion

        #region Private methods
        private void UpdateSwitcherGroup()
        {
            if (JSwitcherGroup.TryGetValue(GroupName, out List<JSwitcher> swithcers) && swithcers.Count != 0)
            {
                for (int i = 0; i < swithcers.Count;)
                {
                    if (swithcers[i] == null)
                    {
                        swithcers.RemoveAt(i);
                    }
                    else
                    {
                        if (swithcers[i].IsChecked == true && swithcers[i] != this)
                        {
                            swithcers[i].IsChecked = false;
                        }
                        i++;
                    }
                }
            }
        }
        #endregion
    }
}
