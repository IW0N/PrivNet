using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Server.Database.Base.Aliases;
using Server.Database.Base.ChatEnvironment;
namespace Server.Database.Base
{
    public partial class User
    {
        [Key]
        public long Id { get; set; }
        public string AliasId { get; set; }
        [ForeignKey("AliasId")]
        public UserAlias Alias { get; set; }
        public long AesKeyId { get; set; }
        [ForeignKey("AesKeyId")]
        //key for encrytion a base traffic (no messages to another users)
        public DbAesKey CipherKey { get; init; }
        public string Name { get; set; }
        public List<Chat> Chats { get; } = new();
    }
}
