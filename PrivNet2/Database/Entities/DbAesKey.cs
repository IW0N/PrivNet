using Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Database.Entities
{
    public class DbAesKey
    {
        public long Id { get; init; }
        public byte[] Key { get; set; }
        public byte[] IV { get; set; }
        public string Username { get; init; }
        [ForeignKey("Username")]
        public User? User { get; init; }

        public static implicit operator AesKey(DbAesKey dbKey) => new(dbKey.Key,dbKey.IV);
        public static implicit operator DbAesKey(AesKey key) => new() { Key=key.Key, IV=key.IV };
    }
}
