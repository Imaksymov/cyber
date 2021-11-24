using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace lab7
{
    class Program
    {
        private RSAParameters _publicKey;
        private RSAParameters _privateKey;

        public void AssignNewKey()
        {
            using (var rsa = new RSACryptoServiceProvider(2048))
            {
                rsa.PersistKeyInCsp = false;
                _publicKey = rsa.ExportParameters(false);
                _privateKey = rsa.ExportParameters(true);
            }
        }
        public byte[] EncryptData(byte[] dataToEncrypt)
        {
            byte[] cipherbytes;
            using (var rsa = new RSACryptoServiceProvider())
            {
                rsa.PersistKeyInCsp = false;
                rsa.ImportParameters(_publicKey);
                cipherbytes = rsa.Encrypt(dataToEncrypt, true);
            }
            return cipherbytes;
        }
        public byte[] DecryptData(byte[] dataToEncrypt)
        {
            byte[] plain;
            using (var rsa = new RSACryptoServiceProvider())
            {
                rsa.PersistKeyInCsp = false;
                rsa.ImportParameters(_privateKey);
                plain = rsa.Decrypt(dataToEncrypt, true);
            }
            return plain;
        }
        static void Main(string[] args)
        {

            var rsaPairs = new Program();
            const string original = "something";
            rsaPairs.AssignNewKey();
            var en = rsaPairs.EncryptData(Encoding.UTF8.GetBytes(original));
            var de = rsaPairs.DecryptData(en);
            Console.WriteLine("Text: " + original);
            Console.WriteLine("Encrypted: " + Convert.ToBase64String(en));
            Console.WriteLine("Decrypted: " + Encoding.Default.GetString(de));
            Console.ReadKey();
        }
    }
}
