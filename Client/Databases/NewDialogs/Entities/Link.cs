
namespace WebClient.Databases.NewDialogs.Entities
{
    public class Link
    {
        public string Id { get; init; }
        public virtual DbPrivateKey? PrivateKey { get; init; }
        public string RootParentId { get; set; }
        public virtual Root? RootParent { get; set; }
    }
}
