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
        public static string[] FaderTickStrings { get; private set; } = new string[]
{
            "OFF",  //0
            "-70",//1
            "","","","","","","","","","",
            "-25",//12
            "","","","","","","","",
            "-18",//21
            "","","","","","","","","",
            "-13",//31
            "","","","","","","","",
            "-9",//40
            "","","","","","","","","",
            "-6",//50
            "","","","","","","","","",
            "0",//60
            "","","","","","","","","","",
            "+6",//71
            "","","","","","","","",
            "+10",//80
};

        public MainWindow()
        {
            InitializeComponent();
            List<string> strings = new List<string>();
            for (int i = 0; i < 20; i++)
            {
                strings.Add((i*10).ToString());
            }
            int a = 0;
            //aaa.AxisTexts = strings;
            //bbb.TextCollection = strings;
            aaaa.TickTexts = FaderTickStrings;
            //ttt.CalculateValueFunction = Cal;
            s666.IsChecked = true;
            bbbb.ItemsSource = strings;
            //bbbb.Value = 6;
            //bbbb.ItemsSource = new string[] { "1" };
            //bbbb.ItemsSource = strings;
            //bbbb.Value = 6;
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
