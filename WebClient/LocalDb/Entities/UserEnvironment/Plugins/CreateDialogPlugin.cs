using Common.Database.Chat;
using Common.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace WebClient.LocalDb.Entities.UserEnvironment.Plugins
{
    class CreateDialogPlugin:CreateChatPlugin
    {
        public CreateDialogPlugin(LocalUser user) : base(user) { }
        public async Task<LocalChat> CreateDialog(string companion)
        {
            string chatName = $"{UserName}-{companion} dialog";
            List<string> participants = new() { companion };
            ChatType chatType = ChatType.Dialog;
            var result = await CreateChat(chatName, participants, chatType);
            return result;
        }
    }
}
