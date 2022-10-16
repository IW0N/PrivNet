using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Common.Services;

namespace Server.Database.Base
{
    public class Alias<T, TKey>
    {
        [Key]
        public string AliasId { get; set; }
        public TKey TableId { get; set; }
        [ForeignKey("TableId")]
        public T Table { get; init; }
        public Alias(string alias,T owner,TKey ownerId)
        {
            TableId=ownerId;
            Table = owner;
            AliasId = alias;
        }
        public Alias() { }
        public static Alias<T, TKey> Generate<TAlias>(T parent, TKey parentKey)
            where TAlias : Alias<T, TKey>, new()
        {
            TokenGenerator generator = new();
            string aliasId = generator.GenerateToken();
            return new TAlias() { AliasId = aliasId, Table = parent, TableId = parentKey };
        }
    }
}
