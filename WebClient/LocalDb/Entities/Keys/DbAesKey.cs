using Common;
using System.ComponentModel.DataAnnotations.Schema;
namespace WebClient.LocalDb.Entities.Keys
{
    public class DbAesKey<TOwner> :AesKey
    {
        public int Id { get; set; }
        public string OwnerId { get; set; }
        [ForeignKey(nameof(OwnerId))]
        public TOwner Owner { get; init; }
        public DbAesKey(byte[] Key, byte[] IV) : base(Key, IV) { }
    }
}
