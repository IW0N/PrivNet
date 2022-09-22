
using System.Numerics;

namespace Server.Database.Entities
{
    public class FileGroup
    {
        public BigInteger GroupId { get; init; }
        public List<DbFile> Files { get; } = new();
        /*public string MessageId { get; init; }
        public Message Message { get; init; }*/
    }
}
