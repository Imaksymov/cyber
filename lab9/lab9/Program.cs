using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace lab9
{
    class Program
    {

        private readonly static string CspContainerName = "RsaKeyContainer";
        public void AssignNewKey()
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
            File.WriteAllText("key.xml", rsa.ToXmlString(false));
        }

        public byte[] SignData(byte[] hashOfData)
        {

            var cspParams = new CspParameters
            {
                KeyContainerName = CspContainerName,
            };

            using (var rsa = new RSACryptoServiceProvider(cspParams))
            {
                rsa.PersistKeyInCsp = true;
                var rsaFormatter = new RSAPKCS1SignatureFormatter(rsa);
                rsaFormatter.SetHashAlgorithm("SHA512");
                return rsaFormatter.CreateSignature(hashOfData);
            }
        }
        public static byte[] Hashing(byte[] dataToSign)
        {
            using (var sha512 = SHA512.Create())
            {
                return sha512.ComputeHash(dataToSign);
            }
        }

        public bool VerifySignature(byte[] hashOfDataToSign, byte[] signature)
        {
            using (var rsa = new RSACryptoServiceProvider())
            {
                rsa.PersistKeyInCsp = false;
                rsa.FromXmlString(File.ReadAllText("key.xml"));
                var rsaDeformatter = new RSAPKCS1SignatureDeformatter(rsa);
                rsaDeformatter.SetHashAlgorithm("SHA512");
                return rsaDeformatter.VerifySignature(hashOfDataToSign, signature);
            }
        }

        static void Main(string[] args)
        {
            var message = Encoding.UTF8.GetBytes("Some information");
            byte[] hashedMessage = Hashing(message);
            var digitalSignature = new Program();
            digitalSignature.AssignNewKey();
            var digsig = digitalSignature.SignData(hashedMessage);
            var verify = digitalSignature.VerifySignature(hashedMessage, digsig);
            Console.WriteLine(" Original Text = " + Encoding.Default.GetString(message));

            Console.WriteLine("==========================================================");


            Console.WriteLine("Digital Signature = " + Convert.ToBase64String(digsig));

            Console.WriteLine("==========================================================");

            if(verify == true)
            {
                Console.WriteLine("Correctly verified.");
            }
            else
            {
                Console.WriteLine("Not verified.");
            }
          
            Console.ReadKey();

        }

    }
}
