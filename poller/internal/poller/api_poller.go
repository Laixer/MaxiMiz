package poller

import (
	"context"
	"net/http"
	"net/url"

	"golang.org/x/oauth2"
)

type Poller struct {
	baseURL    string
	httpClient *http.Client
}

func New(ctx context.Context, baseURL string, clientID string, clientSecret string, endpoint oauth2.Endpoint, redirectURL string, authToken string, scopes ...string) Poller {
	config := &oauth2.Config{
		ClientID:     clientID,
		ClientSecret: clientSecret,
		Endpoint:     endpoint,
		RedirectURL:  redirectURL,
		Scopes:       scopes,
	}

	println(authToken)

	initialToken, err := config.Exchange(ctx, authToken)
	print(initialToken)

	if err != nil {
		panic(err)
	}

	return Poller{baseURL, config.Client(ctx, initialToken)}
}

func (p *Poller) Do(req *http.Request) (*http.Response, error) {
	var err error
	req.URL, err = url.Parse(p.baseURL + req.URL.String())

	if err != nil {
		return nil, err
	}

	return p.httpClient.Do(req)
}
