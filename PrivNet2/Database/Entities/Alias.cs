using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace Server.Database.Entities
{
    public class Alias<T,TKey>
    {
        [Key]
        public string AliasId { get; init; }
        public TKey TableId { get; init; }
        [ForeignKey("TableId")]
        public T Table { get; init; }
    }
}
