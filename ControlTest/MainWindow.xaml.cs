using System;
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
using JayCustomControlLib;
namespace ControlTest
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            List<string> strings = new List<string>();
            for (int i = 0; i < 4; i++)
            {
                strings.Add((i*10).ToString());
            }
            int a = 0;
            //aaa.AxisTexts = strings;
            //bbb.TextCollection = strings;
            aaaa.TickTexts = strings;
            ttt.CalculateValueFunction = Cal;
        }

        public int Cal(string ss)
        {
            return 2;
        }

        private void JSwitcher_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Debug.WriteLine("double");
        }

        private void JSwitcher_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Debug.WriteLine("single");
        }
    }
}
