namespace Poller.Taboola
{
    public class OAuth2
    {
        public string ClientId { get; set; }

        public string ClientSecret { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }
    }

    public class TaboolaPollerOptions
    {
        public string BaseUrl { get; set; }

        public string AccountId { get; set; }

        public OAuth2 OAuth2 { get; set; }
    }
}
