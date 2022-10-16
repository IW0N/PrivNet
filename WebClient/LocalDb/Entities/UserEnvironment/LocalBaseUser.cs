using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebClient.LocalDb.Entities.Keys;

namespace WebClient.LocalDb.Entities.UserEnvironment
{
    public abstract class LocalBaseUser
    {
        [Key]
        public string Nickname { get; protected set; }
        public byte[]? Avatar { get; protected set; }
        public string Alias { get; set; }
        public List<LocalChat> Chats { get; } = new();
    
        public List<FriendUser> Friends { get; } = new();
        public int CipherKeyId { get; set; }

        [ForeignKey(nameof(CipherKeyId))]
        public UserAesKey CipherKey { get; set; }
    }
}
