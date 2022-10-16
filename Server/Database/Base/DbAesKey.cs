using Common;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Database.Base
{
    public class DbAesKey : AesKey
    {
        public DbAesKey(byte[] Key, byte[] IV) : base(Key, IV)
        {
            
        }
        [Key]
        public long Id { get; init; }
        public string? Username { get; init; }
        [ForeignKey("Username")]
        public User? User { get; init; }
        public AesKey GetParent() => new(Key,IV);

    }
}
