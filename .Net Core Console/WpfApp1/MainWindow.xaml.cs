using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
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

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void btn1_Click(object sender, RoutedEventArgs e)
        {
            using (HttpClient client = new HttpClient())
            {
                string content = await client.GetStringAsync("http://www.baidu.com/");
                content = content.Substring(0, 2000);
                this.txt1.Text = content;
                //Thread.Sleep(3000); //Sleep会阻塞当前主线程，如果是在窗体或者WPF程序中会同时阻塞UI线程。对于.Net Core来说如果Sleep数量过多会导致服务器卡死。
                await Task.Delay(3000);//异步线程等待。不会阻塞主线程。
                content = await client.GetStringAsync("http://zhigongyun.gnway.org:9999/");
                content = content.Substring(0, 2000);
                this.txt1.Text = content;

                //yield练习
                //IEnumerable<int> arr = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
                //IEnumerable<int> res = LinqWhereYield(arr, s => s > 6);
                //foreach (var i in res)
                //{
                //    this.txt1.Text += i;
                //}
            }
        }
        //自己封装一个Linq 使用yield
        static IEnumerable<int> LinqWhereYield(IEnumerable<int> arr, Func<int, bool> func)
        {
            foreach (int i in arr)
            {
                if (func(i) == true)
                {
                    yield return i;
                }
            }
        }
    }
}
