using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;

namespace Server.Database.Base
{
    using FileAlias = Alias<DbFile, BigInteger>;
    public class DbFile
    {
        public BigInteger Id { get; set; }
        public byte[] FileContent { get; init; }//encrypted with aes 256 key
        public BigInteger GroupId { get; init; }
        public FileGroup Group { get; init; }
        public byte[] FileName { get; init; }//encrypted with aes 256 key for file content

        public string AliasId { get; set; }
        [ForeignKey("AliasId")]
        public FileAlias Alias { get; set; }
    }
}
