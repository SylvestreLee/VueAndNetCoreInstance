using System.Text;

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
            await File.WriteAllTextAsync(fileName, sb.ToString());
            string res = await File.ReadAllTextAsync(fileName);
            Console.WriteLine(res);
            Console.ReadLine();
        }
    }
}