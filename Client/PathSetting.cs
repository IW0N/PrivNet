namespace WebClient
{
    public struct PathSettings
    {
        public string AppRoot { get; init; }
        public string Users => AppRoot + "/Users";
    }
}
