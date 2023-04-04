namespace ConsoleApp2
{
    internal class Program
    {
        static async  Task Main(string[] args)
        {
            //带async的方法
            string str1 = await ReadText(0);
            string str2 = await ReadText(1);
            Console.WriteLine(str1);
            Console.WriteLine(str2);
            //不带async的方法
            str1 = await ReadTextNoAsync(0);
            str2 = await ReadTextNoAsync(1);
            Console.WriteLine(str1);
            Console.WriteLine(str2);
            Console.ReadKey();
        }
        private static async Task<string> ReadText(int num)
        {
            if(num == 0)
            {
                 return await File.ReadAllTextAsync(@"D:\VueAndNetCoreInstance\test.txt");
            }
            else if(num == 1)
            {
                return await File.ReadAllTextAsync(@"D:\VueAndNetCoreInstance\test2019.txt");
            }
            else
            {
                return "null";
            }
        }

        private static  Task<string> ReadTextNoAsync(int num)
        {
            if (num == 0)
            {
                return  File.ReadAllTextAsync(@"D:\VueAndNetCoreInstance\test.txt");
            }
            else if (num == 1)
            {
                return  File.ReadAllTextAsync(@"D:\VueAndNetCoreInstance\test2019.txt");
            }
            else
            {
                throw new ArgumentException();
            }
        }
    }
}