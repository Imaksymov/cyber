using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;

namespace lab7._2
{
    class Program
    {
        public void AssignNewKey(string publicKeyPath = "pub.xml", string privateKeyPath = "priv.xml")
        {
            using (var rsa = new RSACryptoServiceProvider(2048))
            {
                rsa.PersistKeyInCsp = false;
                File.WriteAllText(publicKeyPath, rsa.ToXmlString(false));
                File.WriteAllText(privateKeyPath, rsa.ToXmlString(true));
            }
        }

        public byte[] EncryptData(byte[] dataToEncrypt, string publicKeyPath = "pub.xml")
        {
            byte[] cipherbytes;
            using (var rsa = new RSACryptoServiceProvider(2048))
            {
                rsa.PersistKeyInCsp = false;
                rsa.FromXmlString(File.ReadAllText(publicKeyPath));
                cipherbytes = rsa.Encrypt(dataToEncrypt, false);
            }
            return cipherbytes;
        }
        public byte[] DecryptData(byte[] dataToEncrypt, string privateKeyPath = "priv.xml")
        {
            byte[] plain;
            using (var rsa = new RSACryptoServiceProvider(2048))
            {
                rsa.PersistKeyInCsp = false;
                rsa.FromXmlString(File.ReadAllText(privateKeyPath));
                plain = rsa.Decrypt(dataToEncrypt, false);
            }
            return plain;
        }
        static void Main(string[] args)
        {
            var rsaPairs = new Program();
            const string original = "something";
            rsaPairs.AssignNewKey();
            var encrypted = rsaPairs.EncryptData(Encoding.UTF8.GetBytes(original));
            var decrypted = rsaPairs.DecryptData(encrypted);
            Console.WriteLine("Text: " + original);
            Console.WriteLine("Encrypted: " + Convert.ToBase64String(encrypted));
            Console.WriteLine("Decrypted: " + Encoding.Default.GetString(decrypted));
            Console.ReadKey();
        }
    }
}

