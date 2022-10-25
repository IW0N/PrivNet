using Server.Database.Updates;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Database.Base
{
    public partial class User
    {
        public string UpdateId { get; set; } = new Guid().ToString();
        [ForeignKey(nameof(UpdateId))]
        public DbUpdate Update { get; set; } = new();
    }
}
