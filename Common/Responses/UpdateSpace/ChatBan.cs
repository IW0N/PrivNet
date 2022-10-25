using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Common.Responses.UpdateSpace
{
    public class ChatBan:BaseUpdateElement
    {
        public int LocalBannerId { get; init; }
        public long ChatId { get; init; }
        public DateTime Period { get; init; }
    }
}
