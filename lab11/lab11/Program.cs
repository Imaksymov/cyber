using System;
using System.Collections.Generic;
using System.Security;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;


namespace Lab11
{
    class User
    {
        public string Login { get; set; }
        public string PasswordHash { get; set; }
        public byte[] Salt { get; set; }
        public string[] Roles { get; set; }
    }
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

        public static byte[] hashPass(byte[] toBeHashed, byte[] salt, int numberOfRounds)
        {
            using (var rfc2898 = new Rfc2898DeriveBytes(toBeHashed, salt, numberOfRounds, HashAlgorithmName.SHA256))
            {
                return rfc2898.GetBytes(32);
            }
        }
    }
    class Protector
    {
        private static Dictionary<string, User> _users = new Dictionary<string, User>();

        public static void Register(string userName, string password, string[] roles = null)
        {
            if (_users.ContainsKey(userName))
            {
                Console.WriteLine("User is already exist!");
            }
            else
            {
                byte[] salt = PBKDF2.GenerateSalt();
                byte[] hashedPass = PBKDF2.hashPass(Encoding.UTF8.GetBytes(password), salt, 15000);

                User newUser = new User();
                newUser.Login = userName;
                newUser.PasswordHash = Convert.ToBase64String(hashedPass);
                newUser.Salt = salt;
                newUser.Roles = roles;
                _users.Add(userName, newUser);
                Console.WriteLine("Complited");
            }
        }

        public static bool CheckPassword(string userName, string password)
        {
            if (_users.ContainsKey(userName))
            {
                User user = _users[userName];
                byte[] hashedPass = PBKDF2.hashPass(Encoding.UTF8.GetBytes(password), user.Salt, 15000);
                if (Convert.ToBase64String(hashedPass) == user.PasswordHash)
                {
                    Console.WriteLine("Password is correct");
                    return true;
                }
                else
                {
                    Console.WriteLine("Password is incorrect");
                    return false;
                }
            }
            else
            {
                Console.WriteLine("There is no registered user with this name!");
                return false;
            }
        }
        public static void LogIn(string userName, string password)
        {
            if (CheckPassword(userName, password))
            {
                var identity = new GenericIdentity(userName, "OIAuth");
                var principal = new GenericPrincipal(identity, _users[userName].Roles);
                System.Threading.Thread.CurrentPrincipal = principal;
            }
        }
        public static void OnlyForAdminsFeature()
        {
            if (Thread.CurrentPrincipal == null)
            {
                throw new SecurityException("Thread.CurrentPrincipal cannot be null.");
            }

            if (!Thread.CurrentPrincipal.IsInRole("Admins"))
            {
                throw new SecurityException("User must be a member of Admins to access this feature.");
            }

            Console.WriteLine("You are an admin!");
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            for (int i = 0; i < 4; i++)
            {
                Console.WriteLine("Enter your login: ");
                string login = Console.ReadLine();
                Console.WriteLine("Enter your password: ");
                string password = Console.ReadLine();
                Console.WriteLine("Enter your roles: ");
                string rolesString = Console.ReadLine();
                Regex sWhitespace = new Regex(@"\s+");
                string rolesWithoutSpaces = sWhitespace.Replace(rolesString, "");
                string[] roles = rolesWithoutSpaces.Split(',');
                Protector.Register(login, password, roles);
                Console.WriteLine();
            }
            Console.WriteLine("===================================================");
            Console.WriteLine();
            Console.WriteLine("Enter login: ");
            string enteredLogin = Convert.ToString(Console.ReadLine());
            Console.WriteLine("Enter password: ");
            string enteredPassword = Convert.ToString(Console.ReadLine());

            if (Protector.CheckPassword(enteredLogin, enteredPassword))
            {
                Protector.LogIn(enteredLogin, enteredPassword);

                try
                {
                    Protector.OnlyForAdminsFeature();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"{ex.GetType()}: {ex.Message}");
                }
            }
            Console.ReadKey();
        }
    }
}
