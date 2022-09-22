using System.ComponentModel.DataAnnotations;
namespace WebClient.Databases.Dialogs.Entities
{
    public class DialogPaticipant
    {
        [Key]
        public string Username { get; set; }
        public virtual List<Dialog> Dialogs { get; } = new();
    }
}
