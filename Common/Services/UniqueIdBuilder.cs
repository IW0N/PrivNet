using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Elskom.Generic.Libs;
using Common.Extensions;

namespace Common.Services
{
    public class UniqueIdBuilder
    {
        public const int RandomBytesCount = 5;
        public const int LastBytesCount = 2;
        BlowFish BuildCipher(string password, byte[] publicKey,int publicKeyShift=0)
        {
            byte[] passwordBytes = Convert.FromBase64String(password);
            
            const int keySize = 56;
            int passwordLen = keySize-LastBytesCount;
            byte[] key = new byte[keySize];
            passwordBytes.CopyTo(key,0);
            int arrLastI = publicKey.Length-1-publicKeyShift;
            int stopIndex = arrLastI-LastBytesCount;
            for (int i=arrLastI;i>stopIndex;i--)
            {
                key[arrLastI - i+passwordLen] = publicKey[i];
            }
            
            return new BlowFish(key);
        }
        string AddRandomBytesToAlias(string alias)
        {
            byte[] random = new byte[RandomBytesCount];
            RandomNumberGenerator.Fill(random);
            alias = random.ToBase64()+alias;
            return alias;
        }
        public string Build(string alias,string password, byte[] publicKey,int publicKeyShift= 0)
        {
            var encryptor = BuildCipher(password,publicKey,publicKeyShift);
            alias = AddRandomBytesToAlias(alias);
            string uniqueId=encryptor.EncryptECB(alias);
            encryptor.Dispose();
            return uniqueId;
        }
        public string Build(string alias, byte[] publicKey,int shift=0)
        {
            int idLength = alias.Length;
            byte[] id = new byte[idLength];
            for(int i=0;i<idLength;i++)
                id[i] = (byte)(alias[i] ^ publicKey[i+shift]);
            return Convert.ToBase64String(id);
        }
        public bool VerifyUniqueId(string encryptedAlias,string alias, byte[] publicKey,string password, int publicKeyShift = 0)
        {
            using BlowFish fish = BuildCipher(password,publicKey,publicKeyShift);
            string decryptedAlias=fish.DecryptECB(encryptedAlias);
            byte[] byteDecrAlias = decryptedAlias.FromBase64();
            byte[] bAlias = new byte[byteDecrAlias.Length-RandomBytesCount];
            for (int i=RandomBytesCount-1;i<bAlias.Length;i++)
            {
                byte b = byteDecrAlias[i];
                bAlias[i-RandomBytesCount+1] = b;
            }
            string gotAlias = bAlias.ToBase64();
            return gotAlias == alias;
        }
    }
}
