using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Database.Base.ChatEnvironment
{
    public class DbRSALock:ChatAddition
    {
        public byte[] Lock { get; init; }
    }
}
