using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;
using System.Media;

namespace KiBLista3
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = "C:\\Users\\Cebulotron3000\\Documents\\My Games\\FarmingSimulator2015\\mods\\cash.wav";
            byte[] test = System.IO.File.ReadAllBytes(path);

            var zad1 = new Zad1(path,"","");

            var encrypt = zad1.Encrypte(new byte[]{2,3,4});

            var decrypt = zad1.Decrypt(encrypt, new byte[] { 2, 3, 4 });

            MemoryStream ms = new MemoryStream(decrypt);
            SoundPlayer player = new SoundPlayer(ms);

            player.Play();
        }
    }

    class Zad1
    {
        private string _path;
        private byte[] salt = { 1, 2, 3, 4, 5, 67, 8 ,5};
        private RijndaelManaged aes;    

        public Zad1(string path,string pathToKeyStore,string keyId)
        {
            this._path = path;

            this.aes = new RijndaelManaged()
            {
                BlockSize = 256,
                KeySize = 128,
                Mode = CipherMode.CBC
            };
            
            
        }


        public byte[] Encrypte(byte[] pass)
        {
            MemoryStream ms = new MemoryStream();
            byte[] encrypt = System.IO.File.ReadAllBytes(_path);
            var key = new Rfc2898DeriveBytes(pass, salt, 1000);
            aes.Key = key.GetBytes(aes.KeySize/8);

            var cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(encrypt,0,encrypt.Length);
            cs.Close();

            return ms.ToArray();

        }

        internal byte[] Decrypt(byte[] toDecrypte, byte[] pass)
        {
            MemoryStream ms = new MemoryStream();

            var key = new Rfc2898DeriveBytes(pass, salt, 1000);
            aes.Key = key.GetBytes(aes.KeySize / 8);

            var cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Write);
            cs.Write(toDecrypte, 0, toDecrypte.Length);
            cs.Close();

            return ms.ToArray();
        }
    }
}
