using System;
using System.Text;
using System.Security.Cryptography;
using System.Collections.Generic;

namespace _4part
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
        public static byte[] HashPassword(byte[] toBeHashed, byte[] salt, int numberOfRounds, System.Security.Cryptography.HashAlgorithmName hashAlgorithm, Int32 NumberOfBytes)
        {
            using (var rfc2898 = new Rfc2898DeriveBytes(toBeHashed, salt, numberOfRounds, hashAlgorithm))
            {
                return rfc2898.GetBytes(NumberOfBytes);
            }
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            List<string> logins = new List<string>();
            List<string> passwords = new List<string>();
            List<string> salts = new List<string>();
            while (true)
            {

                Console.Write("Write 'l' to login or 'r' to register: ");
                var temp = Console.ReadLine();
                if (temp == "l")
                {
                    Console.Write("Enter your login: ");
                    var login = Console.ReadLine();
                    if (logins.Contains(login))
                    {
                        var ind = logins.IndexOf(login);
                        Console.Write("Enter your password: ");
                        var pass = Console.ReadLine();
                        var salt = Convert.FromBase64String(salts[ind]);
                        var sha256pass = Convert.ToBase64String(PBKDF2.HashPassword(Encoding.Unicode.GetBytes(pass), salt, 150000, HashAlgorithmName.SHA256, 32));
                        if (passwords[ind] == sha256pass)
                        {
                            Console.WriteLine("Loggined");
                        }
                        else
                        {
                            Console.WriteLine("Wrong password");
                        }

                    }
                    else
                    {
                        Console.WriteLine("Wrong login");
                    }
                }
                else if (temp == "r")
                {
                    Console.Write("Enter your login: ");
                    var login = Console.ReadLine();
                    if (!logins.Contains(login))
                    {
                        logins.Add(login);
                        Console.Write("Enter your password: ");
                        var pass = Console.ReadLine();
                        var salt = PBKDF2.GenerateSalt();
                        var sha256pass = Convert.ToBase64String(PBKDF2.HashPassword(Encoding.Unicode.GetBytes(pass), salt, 150000, HashAlgorithmName.SHA256, 32));
                        passwords.Add(sha256pass);
                        salts.Add(Convert.ToBase64String(salt));
                        Console.WriteLine("Registered");

                    }
                    else
                    {
                        Console.WriteLine("Login already in base");
                        continue;
                    }
                }
                else
                {
                    Console.WriteLine("Write correct please");
                }
            }
        }
    }
}

