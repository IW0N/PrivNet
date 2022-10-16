using Server.Database.Base;

namespace Server.Database.Base.Aliases
{
    public class UserAlias:Alias<User,long>
    {
        public UserAlias(string aliasId, User owner) : base(aliasId, owner, owner.Id) { }
        public UserAlias() : base() { }
    }
}
