using System;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;

namespace lab5._4
{
    public class PBKDF2
    {
        public static byte[] GenerateSalt()
        {
            using (var randomNumberGenerator = new RNGCryptoServiceProvider())
            {
                var randomNumber = new byte[32];
                randomNumberGenerator.GetBytes(randomNumber);
                return randomNumber;
            }
        }
        public static byte[] HashPassword(byte[] toBeHashed, byte[] salt, int numberOfRounds, System.Security.Cryptography.HashAlgorithmName hashAlgorithm)
        {
            using (var rfc2898 = new Rfc2898DeriveBytes(toBeHashed, salt, numberOfRounds, hashAlgorithm))
            {
                return rfc2898.GetBytes(20);
            }
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            const string password = "something";
            HashPassword(password, 150000);
            HashPassword(password, 210000);
            HashPassword(password, 260000);
            HashPassword(password, 310000);
            HashPassword(password, 360000);
            HashPassword(password, 410000);
            HashPassword(password, 460000);
            HashPassword(password, 510000);
            HashPassword(password, 560000);
            HashPassword(password, 610000);
            Console.ReadLine();
        }
        private static void HashPassword(string password, int numberOfRounds)
        {
            var sw = new Stopwatch();
            sw.Start();
            var hashedPassword = PBKDF2.HashPassword(Encoding.UTF8.GetBytes(password), PBKDF2.GenerateSalt(), numberOfRounds, HashAlgorithmName.SHA512);
            sw.Stop();
            Console.WriteLine();
            Console.WriteLine("Password : " + password);
            Console.WriteLine("Hashed Password : " + Convert.ToBase64String(hashedPassword));
            Console.WriteLine("Iterations <" + numberOfRounds + "> Elapsed Time: " + sw.ElapsedMilliseconds + "ms");
        }
    }
}
