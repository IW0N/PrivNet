using Server.Database.Base;

namespace Server.Database.Updates.Environment;

interface IDbUpdate
{
    public long AddresseeId { get; init; }
    public User Addressee { get; init; }
}
