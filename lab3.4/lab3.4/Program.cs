using System;
using System.Security.Cryptography;
using System.Text;

namespace Lab4_4
{
    class Program
    {
        static byte[] ComputeHashSHA256(byte[] dataForHash)
        {
            using (var sha256 = SHA256.Create())
            {
                return sha256.ComputeHash(dataForHash);
            }
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Enter your login: ");
            string log = Convert.ToString(Console.ReadLine());
            Console.WriteLine("Enter your password: ");
            string pass = Convert.ToBase64String(ComputeHashSHA256(Encoding.Unicode.GetBytes(Convert.ToString(Console.ReadLine()))));

            Console.WriteLine("---------------------------------------------------");
            Console.WriteLine("Enter your login: ");
            string log1 = Convert.ToString(Console.ReadLine());
            Console.WriteLine("Enter your password: ");
            string pass1 = Convert.ToBase64String(ComputeHashSHA256(Encoding.Unicode.GetBytes(Convert.ToString(Console.ReadLine()))));

            if (log != log1)
            {
                Console.WriteLine("Wrong login");
            }
            else if (pass != pass1)
            {
                Console.WriteLine("Wrong password");
            }
            else
            {
                Console.WriteLine("Login and password are correct");
            }
            Console.ReadKey();
        }
    }
}