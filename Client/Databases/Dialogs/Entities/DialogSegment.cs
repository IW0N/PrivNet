namespace WebClient.Databases.Dialogs.Entities
{
    public class DialogSegment
    {
        public int Id { get; init; }
        public int MessageIndexStart { get; init; }
        public int MessageIndexEnd { get; set; }
        //It needs for encryption aes key at the time exchange of aes keys by RSA algorithm
        public string AesPublicKey { get; init; }
        public string AesKey { get; init; }
        public string AesIV { get; init; }
        public string DialogId { get; init; }
        public virtual Dialog Dialog { get; set; }
        
    }
}
