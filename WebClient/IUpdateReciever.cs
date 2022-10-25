using Common.Responses.UpdateSpace;
namespace WebClient
{
    interface IUpdateReciever
    { 
        event Action<Update> OnNewUpdate;

        event Action<ChatInvite> OnNewChatInvite;
        event Action<ChatBan> OnBanRecieved;

        event Action<FriendInvite> OnNewFriendRequest;
        event Action<FriendDeletion> OnFriendDeleted;
        void Listen();
    }
    
}
