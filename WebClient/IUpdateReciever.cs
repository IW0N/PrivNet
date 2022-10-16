using Common.Responses.UpdateSpace;
namespace WebClient
{
    interface IUpdateReciever
    { 
        event Action<Update> OnUpdateRecieved;

        event Action<ChatInvite> OnChatInviteRecieved;
        event Action<ChatBan> OnBanRecieved;

        event Action<FriendInvite> OnNewFriendRequest;
        event Action<FriendDeletion> OnFriendDeleted;
        void Listen();
    }
    
}
