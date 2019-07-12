using Google.Ads.GoogleAds.Config;
using Poller.Model;

namespace Poller.GoogleAds
{
    /// <summary>
    /// Taboola configuration options.
    /// </summary>
    public class GoogleAdsPollerOptions
    {
        public readonly GoogleAdsConfig Config = new GoogleAdsConfig();

        public string LoginCustomerId { get => Config.LoginCustomerId; set => Config.LoginCustomerId = value; }

        public string DeveloperToken { get => Config.DeveloperToken; set => Config.DeveloperToken = value; }


        /// <summary>
        /// Authentication settings.
        /// </summary>
        public OAuth2 OAuth2
        {
            get
            {
                return new OAuth2
                {
                    ClientId = Config.OAuth2ClientId,
                    ClientSecret = Config.OAuth2ClientSecret,
                    RefreshToken = Config.OAuth2RefreshToken
                };
            }
            set
            {
                Config.OAuth2ClientId = value.ClientId;
                Config.OAuth2ClientSecret = value.ClientSecret;
                Config.OAuth2RefreshToken = value.RefreshToken;
            }
        }
    }
}
