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
	getBaseURL() string
	getHTTPClient() *http.Client
	do(req *http.Request) (*http.Response, error)

	//TODO make this a list of handlerfuncs instead of declaring them seperately
	GetCPMHandler() http.HandlerFunc
}

// GetPollers returns all the pollers that need to be registered
func GetPollers(ctx context.Context) []Poller {
	return []Poller{newGooglePoller(ctx)}
}

// SaveTokens saves the auth token to the config file to prevent having to manually request a new token
func SaveTokens() {

}

// ReadTokens reads the previously token from file to prevent manually getting the token again.
func readTokensFromFile() []string {
	return make([]string, 1)
}

func httpClientFromOAuth2(ctx context.Context, baseURL string, clientID string, clientSecret string, endpoint oauth2.Endpoint, redirectURL string, authToken string, scopes ...string) *http.Client {

	config := &oauth2.Config{
		ClientID:     clientID,
		ClientSecret: clientSecret,
		Endpoint:     endpoint,
		RedirectURL:  redirectURL,
		Scopes:       scopes,
	}
	initialToken, err := config.Exchange(ctx, authToken)

	if err != nil {
		logger.Fatalf("could not exchange authtoken for OAuth2 token. the server responded as follows:\n %v", err)
		panic(err)
	}

	return config.Client(ctx, initialToken)
}

func do(req *http.Request, p Poller) (*http.Response, error) {
	var err error
	urlString := p.getBaseURL() + req.URL.String()
	req.URL, err = url.Parse(urlString)

	if err != nil {
		logger.Fatalf("could not parse url: %s\n%v", urlString, err)
	}

	return p.getHTTPClient().Do(req)
}
