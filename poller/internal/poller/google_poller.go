package poller

import (
	"bytes"
	"context"
	"encoding/json"
	"fmt"
	"io/ioutil"
	"net/http"

	"golang.org/x/oauth2"

	"github.com/Zanhos/MaxiMiz/poller/internal/config"

	"golang.org/x/oauth2/google"
)

const googleName = "google"

const accountID = "google_ads_account_id"

const googleadsEndpoint = "google_ads_endpoint"
const googleOAuth2ClientID = "google_oauth2_client_id"
const googleOAuth2ClientSecret = "google_oauth2_client_secret"
const googleRedirectURL = "google_redirect_url"
const googleOAuth2Token = "google_oauth2_token"
const googleOAuth2Scope = "google_oauth2_scope"

type googlePoller struct {
	httpClient *http.Client
}

//TODO https://developers.google.com/identity/protocols/OAuth2ServiceAccount
func NewGooglePoller(ctx context.Context) *googlePoller {
	oAuth2Config := &oauth2.Config{
		ClientID:     config.StringEnv(googleOAuth2ClientID),
		ClientSecret: config.StringEnv(googleOAuth2ClientSecret),
		Endpoint:     google.Endpoint,
		RedirectURL:  config.StringEnv(googleRedirectURL),
		Scopes:       []string{config.StringEnv(googleOAuth2Scope)},
	}

	httpClient := httpClientFromOAuth2Authorization(ctx, oAuth2Config, config.StringEnv(googleOAuth2Token))

	return &googlePoller{httpClient}
}

func (gp *googlePoller) Name() string {
	return googleName
}

func (gp *googlePoller) do(req *http.Request) (*http.Response, error) {
	return do(req, gp)
}

func (gp *googlePoller) baseURL() string {
	return fmt.Sprintf("%s%d/", config.StringEnv(googleadsEndpoint), gp.accountID())
}

func (gp *googlePoller) getHTTPClient() *http.Client {
	return gp.httpClient
}

func (gp *googlePoller) accountID() int {
	return config.IntEnv(accountID)
}
func (gp *googlePoller) GetAllItemData() {

	type create struct {
		// operations
		Name         string `json:"name"`
		AmountMicros int    `json:"amount_micros"`
	}

	// type select
	reqContent := create{"", 2}

	b := new(bytes.Buffer)
	err := json.NewEncoder(b).Encode(reqContent)

	if err != nil {
		panic(err)
	}

	req, err := http.NewRequest(http.MethodPost, fmt.Sprintf("%GoogleAdsService", gp.baseURL()), b)

	if err != nil {
		panic(err)
	}

	req.Header.Set("Content-Type", "application/json")
	req.Header.Set("developer-token", config.StringEnv("google_developer_token"))

	resp, err := gp.do(req)

	if err != nil {
		panic(err)
	}

	content, err := ioutil.ReadAll(resp.Body)

	if err != nil {
		panic(err)
	}

	println(string(content))
}
