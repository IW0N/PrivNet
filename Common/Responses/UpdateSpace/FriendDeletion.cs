using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Responses.UpdateSpace
{
    public class FriendDeletion:BaseUpdateElement
    {
        public long FriendId { get; init; }
        public string? Description { get; init; }
    }
}
