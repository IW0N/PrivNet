using System.ComponentModel.DataAnnotations;
namespace WebClient.Databases.NewDialogs.Entities
{
   
   // public record DbPrivateKey(int DialogId,byte[] PrivateKey);
    public class DbPrivateKey
    {
        [Key]
        public int DialogId { get; set; }
        public string PrivateKey { get; set; }
        public string LinkParentId { get; set; }
        public virtual Link? LinkParent { get; set; }
    }
}
