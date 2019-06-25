package poller

import (
	"bytes"
	"context"
	"encoding/json"
	"fmt"
	"io/ioutil"
	"net/http"
	"time"

	"golang.org/x/oauth2"

	"github.com/Zanhos/MaxiMiz/poller/internal/config"
	"github.com/google/logger"
)

const taboolaName = "taboola"

const taboolaAccountID = "taboola_account_id"
const taboolaEndpoint = "taboola_endpoint"
const taboolaOAuth2ClientID = "taboola_oauth2_client_id"
const taboolaOAuth2ClientSecret = "taboola_oauth2_client_secret"
const taboolaRedirectURL = "taboola_redirect_url"
const taboolaOAuth2Token = "taboola_oauth2_token"
const taboolaOAuth2Scope = "taboola_oauth2_scope"
const taboolaOAuth2AuthURL = "taboola_oauth2_auth_url"
const taboolaOAuth2TokenURL = "taboola_oauth2_token_url"
const taboolaOAuth2Style = oauth2.AuthStyleInParams
const taboolaOAuth2Username = "taboola_oauth2_username"
const taboolaOAuth2Password = "taboola_oauth2_password"

type taboolaPoller struct {
	httpClient *http.Client
}

func NewTaboolaPoller(ctx context.Context) *taboolaPoller {
	oAuth2Config := &oauth2.Config{
		ClientID:     config.StringEnv(taboolaOAuth2ClientID),
		ClientSecret: config.StringEnv(taboolaOAuth2ClientSecret),
		Endpoint: oauth2.Endpoint{
			AuthURL: config.StringEnv(taboolaOAuth2AuthURL), TokenURL: config.StringEnv(taboolaOAuth2TokenURL), AuthStyle: taboolaOAuth2Style,
		},
		RedirectURL: config.StringEnv(taboolaRedirectURL),
		Scopes:      []string{config.StringEnv(taboolaOAuth2Scope)},
	}

	return &taboolaPoller{httpClientFromOAuth2PasswordCredentials(ctx, oAuth2Config, config.StringEnv(taboolaOAuth2Username), config.StringEnv(taboolaOAuth2Password))}

}

func (tp *taboolaPoller) Name() string {
	return taboolaName
}

func (tp *taboolaPoller) do(req *http.Request) (*http.Response, error) {
	return do(req, tp)
}

func (tp *taboolaPoller) baseURL() string {
	return config.StringEnv(taboolaEndpoint) + taboolaAccountID + "/"
}

func (tp *taboolaPoller) getHTTPClient() *http.Client {
	return tp.httpClient
}

func (tp *taboolaPoller) GetAllItemData() {

	type create struct {
		Name         string `json:"name"`
		AmountMicros int    `json:"amount_micros"`
	}

	reqContent := create{"", 2}

	b := new(bytes.Buffer)
	err := json.NewEncoder(b).Encode(reqContent)

	if err != nil {
		panic(err)
	}
	start := time.Now().AddDate(0, 0, 7)
	end := time.Now()
	time.Now().Format(time.RFC3339)

	req, err := http.NewRequest(http.MethodPost, fmt.Sprintf("reports/top-campaign-content/dimensions/item_breakdown?start_date=%s&end_date=%s", start.Format(time.RFC3339), end.Format(time.RFC3339)), b)

	if err != nil {
		panic(err)
	}

	// req.Header.Set("Content-Type", "application/json")

	resp, err := tp.do(req)

	if err != nil {
		logger.Errorf("%v,\n %v", resp, err)
	}

	content, err := ioutil.ReadAll(resp.Body)

	if err != nil {
		panic(err)
	}

	println(string(content))
}
