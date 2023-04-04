using System.Text;
using static System.Threading.Thread;

namespace ConsoleApp1
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            string fileName = "D:\\公司培训\\自学\\.Net Core Console\\1.txt";
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < 10000; i++)
            {
                sb.AppendLine("Hello World!!!");
            }
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
            Console.WriteLine(CurrentThread.ManagedThreadId);
            await File.WriteAllTextAsync(fileName, sb.ToString());
            Console.WriteLine(CurrentThread.ManagedThreadId);
            string res = await File.ReadAllTextAsync(fileName);
            Console.WriteLine(CurrentThread.ManagedThreadId);
            //Console.WriteLine(res);
            Console.ReadLine();
        }
    }
}