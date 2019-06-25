package poller

import (
	"context"
	"net/http"
	"net/url"

	"github.com/google/logger"

	"golang.org/x/oauth2"
)

// Poller defines a poller that contains a client to poll webservices for advertisement data
type Poller interface {
	Name() string
	baseURL() string
	getHTTPClient() *http.Client
	do(req *http.Request) (*http.Response, error)
}

// GetPollers returns all the pollers that need to be registered
func GetPollers(ctx context.Context) []Poller {
	return []Poller{NewTaboolaPoller(ctx)}
}

// SaveTokens saves the auth token to the config file to prevent having to manually request a new token
func SaveTokens() {

}

func httpClientFromOAuth2PasswordCredentials(ctx context.Context, config *oauth2.Config, username string, password string) *http.Client {
	initialToken, err := config.PasswordCredentialsToken(ctx, username, password)

	if err != nil {
		logger.Errorf("could not exchange username and password for OAuth2 token. the OAuth2 server responded as follows:\n %v", err)
	}

	return config.Client(ctx, initialToken)
}

func httpClientFromOAuth2Authorization(ctx context.Context, config *oauth2.Config, authToken string) *http.Client {
	initialToken, err := config.Exchange(ctx, authToken)
	if err != nil {
		logger.Errorf("could not exchange authtoken for OAuth2 token. the OAuth2 server responded as follows:\n %v", err)
	}
	return config.Client(ctx, initialToken)

}

func do(req *http.Request, p Poller) (*http.Response, error) {
	var err error
	urlString := p.baseURL() + req.URL.String()
	req.URL, err = url.Parse(urlString)

	if err != nil {
		logger.Errorf("could not parse url: %s\n%v", urlString, err)
	}

	return p.getHTTPClient().Do(req)
}
