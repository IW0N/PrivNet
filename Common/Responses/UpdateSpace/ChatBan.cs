using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Responses.UpdateSpace
{
    public class ChatBan:BaseUpdateElement
    {
        //user local id in chat
        public long ChatId { get; init; }
        public DateTime Period { get; init; }
    }
}
