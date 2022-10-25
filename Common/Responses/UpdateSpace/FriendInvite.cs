using System.ComponentModel.DataAnnotations;

namespace Common.Responses.UpdateSpace
{
    public class FriendInvite:BaseUpdateElement
    {
        [Key]
        public string InviteLink { get; init; }
    }
}
