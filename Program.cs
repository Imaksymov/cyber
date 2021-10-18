using System;
using System.Security.Cryptography;

namespace ConsoleApp8
{
    class Program
    {
        static byte[] func(int length = 10)
        {
            var a = new RNGCryptoServiceProvider();
            var d = new byte[length];
            a.GetBytes(d);
            return d;
        }
        static void Main(string[] args)
        {
            for (int j = 0; j < 2; j++)
            {
                Console.WriteLine("--------------------------------------------");
                Random d = new Random(245);
                for (int i = 0; i < 10; i++)
                {
                    Console.WriteLine(d.Next(0, 100));
                }

            }
            Console.WriteLine("---------------------------------------------------");
            for (int i = 0; i < 4; i++)
            {
                string text = Convert.ToBase64String(func());
                Console.WriteLine(text);
            }
        }
    }
}
