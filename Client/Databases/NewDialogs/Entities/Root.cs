using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace WebClient.Databases.NewDialogs.Entities
{
    public class Root
    {
        [Key]
        public string Username { get; set; }
        
        public virtual List<Link> Links { get; set; } = new();
   
    }
}
