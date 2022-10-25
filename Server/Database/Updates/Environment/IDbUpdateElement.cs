using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;

namespace Server.Database.Updates.Environment;

public interface IDbUpdateElement
{
    [JsonIgnore]
    string UpdateId { get; init; }
    [JsonIgnore]
    DbUpdate Update { get; init; }
    void NotifyUsers();
}
