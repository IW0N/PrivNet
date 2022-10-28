using Common.Responses.UpdateSpace;
namespace WebClient
{
    //It uses interface, because events can't launch in children
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
