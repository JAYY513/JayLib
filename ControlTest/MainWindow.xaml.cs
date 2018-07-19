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

        }


    }
}
