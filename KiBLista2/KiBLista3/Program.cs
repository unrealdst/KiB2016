using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;
using System.Media;

using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Utilities.Encoders;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Paddings;

using System.Security.Cryptography.X509Certificates;

namespace KiBLista3
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = "C:\\Users\\Cebulotron3000\\Documents\\My Games\\FarmingSimulator2015\\mods\\cash.wav";
            string pathToKEyStore = "C:\\Users\\Cebulotron3000\\keystore.jks";
            string password = "qwerty";

            byte[] test = System.IO.File.ReadAllBytes(path);

            var zad1 = new Zad1(path, pathToKEyStore, "", System.Text.Encoding.UTF8.GetBytes(password));

            var encrypt = zad1.Encrypte(System.Text.Encoding.UTF8.GetBytes(password));


            var decrypt = zad1.Decrypt(encrypt, System.Text.Encoding.UTF8.GetBytes(password));

            MemoryStream ms = new MemoryStream(decrypt);
            SoundPlayer player = new SoundPlayer(ms);

            player.Play();
        }
    }

    class Zad1
    {
        private string _path;
        private byte[] salt = { 1, 2, 3, 4, 5, 67, 8, 5 };
        private byte[] key;
        private RijndaelManaged aes;
        private string _pathToKeyStore;
        private byte[] _pass;

        public Zad1(string path, string pathToKeyStore, string keyId, byte[] password)
        {
            this._path = path;
            this._pathToKeyStore = pathToKeyStore;

            this.aes = new RijndaelManaged()
            {
                BlockSize = 256,
                KeySize = 128,
                Mode = CipherMode.CBC
            };

            key = GetKey();


        }

        private byte[] GetKey()
        {
            //  X509Certificate cert = new X509Certificate(_pathToKeyStore.Split('\\').Last(),"qwerty");
            //    KeyStore ks = KeyStore.getInstance("JKS");
            
            var certificate = new X509Certificate(File.ReadAllBytes(_pathToKeyStore), "qwerty");

            var r = new Random();
            var tempKey = new byte[128 / 8];
            for (int i = 0; i < tempKey.Length; i++)
            {
                tempKey[i] = (byte)r.Next(100);
            }

            return tempKey;
        }


        public byte[] Encrypte(byte[] pass)
        {
            MemoryStream ms = new MemoryStream();
            byte[] encrypt = System.IO.File.ReadAllBytes(_path);
            //  var key = new Rfc2898DeriveBytes(pass, salt, 1000);
            //  aes.Key = key.GetBytes(aes.KeySize/8);

            aes.Key = key;

            var cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(encrypt, 0, encrypt.Length);
            cs.Close();

            return ms.ToArray();
        }

        internal byte[] Decrypt(byte[] toDecrypte, byte[] pass)
        {
            MemoryStream ms = new MemoryStream();

            //var key = new Rfc2898DeriveBytes(pass, salt, 1000);
            //aes.Key = key.GetBytes(aes.KeySize / 8);
            aes.Key = key;

            var cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Write);
            cs.Write(toDecrypte, 0, toDecrypte.Length);
            cs.Close();

            return ms.ToArray();
        }
    }
}
