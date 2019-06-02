package poller

import (
	// "fmt"
	// "io/ioutil"
	"context"
	"net/http"
	// "encoding/json"

	"golang.org/x/oauth2"
)

// // Poller ...
// type Poller struct {
// 	httpClient *http.Client
// }

// NewPoller ...
func NewPoller(ctx context.Context, clientID string, clientSecret string, endpoint oauth2.Endpoint, redirectURL string, authToken string, scopes ...string) (p *http.Client) {
	config := &oauth2.Config{
		ClientID:     clientID,
		ClientSecret: clientSecret,
		Endpoint:     endpoint,
		RedirectURL:  redirectURL,
		Scopes:       scopes,
	}

	initialToken, err := config.Exchange(ctx, authToken)

	if err != nil {
		panic(err)
	}

	return config.Client(ctx, initialToken)
}