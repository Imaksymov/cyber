using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Linq;



namespace lab8
{
    class Program
    {
        private readonly static string CspContainerName = "RSAKeyContainer";
        public static void AssignNewKey(string publicKeyPath)
        {
            CspParameters cspParameters = new CspParameters(1)
            {
                KeyContainerName = CspContainerName,

                ProviderName = "Microsoft Strong Cryptographic Provider"
            };
            var rsa = new RSACryptoServiceProvider(cspParameters)
            {
                PersistKeyInCsp = true
            };
            File.WriteAllText(publicKeyPath, rsa.ToXmlString(false));
        }
        public static byte[] EncryptData(string publicKeyPath, byte[] dataToEncrypt)
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
        public static byte[] DecryptData(byte[] dataToDecrypt)
        {
            byte[] plainBytes;
            var cspParams = new CspParameters
            {
                KeyContainerName = CspContainerName,
            };
            using (var rsa = new RSACryptoServiceProvider(cspParams))
            {
                rsa.PersistKeyInCsp = true;
                plainBytes = rsa.Decrypt(dataToDecrypt, false);
            }
            return plainBytes;
        }

        static void Main(string[] args)
        {
            string myMassage = "Congratulations!!!";
            //AssignNewKey("MaksymovIvan.xml");


            byte[] data = File.ReadAllBytes("MyMessage.dat").ToArray();

            //var encrypted = EncryptData("publicVovk.xml", Encoding.Unicode.GetBytes(myMassage));
            //File.WriteAllBytes("myMassage.dat", encrypted);
            //Console.WriteLine(" Original Text = " + myMassage);
            //Console.ReadKey();
            var decrypted = DecryptData(data);
            Console.WriteLine();
            //Console.WriteLine(" Encrypted Text = " + Convert.ToBase64String(encrypted));
            Console.WriteLine(" Decrypted Text = " + Encoding.Unicode.GetString(decrypted));
            Console.ReadKey();

        }
    }
}
