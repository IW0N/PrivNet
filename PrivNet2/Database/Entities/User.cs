namespace Server.Database.Entities
{
    using ChatEnv;
    using Common;
    using Server.Database.Entities.Aliases;
    using System.ComponentModel.DataAnnotations.Schema;

    public class User
    {
        public string AliasId { get; set; }
        [ForeignKey("AliasId")]
        public UserAlias Alias { get; set; }
        public long AesKeyId { get; init; }
        [ForeignKey("AesKeyId")]
        //key for encrytion a base traffic (no messages to other users)
        public DbAesKey CipherKey { get; init; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string ServerCert { get; set; }
        public List<Chat> Chats { get; } = new();
    }
}
