using Common.Requests;
using Common.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;
using WebClient.LocalDb.Entities;

namespace WebClient.LocalDb
{
    public abstract class AbstractBuilder
    {
        protected Dictionary<string, Delegate> dict = new();
        //protected delegate dynamic DynamicDelegate(PrivNetLocalDb db,params object[] input);
       
        public T BuildEntity<T>(params object[] input)
        {
            var type=typeof(T);
            var typeName = type.Name;
            Delegate @delegate = dict[typeName];
            return (T)@delegate.DynamicInvoke(input);
        }
        protected void AddRange(Dictionary<string, Delegate> additionalDict)
        {
            foreach (var kvp in additionalDict)
            {
                dict.Add(kvp.Key, kvp.Value);
            }
        }
    }
    public class EntityBuilder : AbstractBuilder
    {
        public EntityBuilder()
        {
            var additionalDict = new Dictionary<string,Delegate>() 
            {
                { nameof(RSALock), BuildLock },
                //{ nameof(LocalChat),BuildChat }
               // {nameof(LocalUser), }
            };
            AddRange(additionalDict);
        }
        private RSALock BuildLock(RSACryptoServiceProvider rsa)
        {
           // var rsa = (RSACryptoServiceProvider)parames[0];
            RSALock lockOfAesKey=new()
            {
                Lock = rsa.ExportRSAPublicKey(),
                Key = rsa.ExportRSAPrivateKey()
            };
            
            return lockOfAesKey;
        }
       
    }
}
