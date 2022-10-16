using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebClient.LocalDb.Entities.UserEnvironment;

namespace WebClient.LocalDb.Entities.Keys
{
    public class UserAesKey:DbAesKey<LocalUser>
    {
        
        public UserAesKey(byte[] Key, byte[] IV) : base(Key, IV) { }
        public UserAesKey(AesKey key) : base(key.Key, key.IV) { }
    }
}
