using Server.Database.Base.ChatEnvironment;
using System.Numerics;
namespace Server.Database.Base
{
    public class Message
    {
        public BigInteger Id { get; init; }
        public byte[] EncryptedText { get; set; }
        public BigInteger FileGroupId { get; set; }
        public FileGroup FileGroup { get; set; }
        public string SenderName { get; init; }
        public string RecieverName { get; init; }
        public long ChatId { get; init; }
        public Chat Chat { get; init; }
    }
}
