namespace WebClient
{
    using Common.Responses;
    using Microsoft.EntityFrameworkCore;
    using static ClientContext;
    public static class LocalDbUpdateHandler
    {
        public static void UpdateTemporaryData(BaseResponse response)
        {
            lock (Db)
            {
                Db = new LocalDb.PrivNetLocalDb();
                ActiveUser=Db.Users.
                    Include(user => user.CipherKey).
                    First(user=>user.Nickname==ActiveUser.Nickname);
                ActiveUser.CipherKey.IV = response.NextIV;
                ActiveUser.Alias = response.NextAlias;
                Db.SaveChanges();
                Db.Dispose();
            }
        }
    }
}
