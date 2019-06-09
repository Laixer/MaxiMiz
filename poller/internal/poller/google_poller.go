package poller

import (
	"bytes"
	"context"
	"fmt"
	"io/ioutil"
	"net/http"

	"github.com/Zanhos/MaxiMiz/poller/internal/config"

	"golang.org/x/oauth2/google"
)

type googlePoller struct {
	httpClient *http.Client
}

func newGooglePoller(ctx context.Context) *googlePoller {

	httpClient := httpClientFromOAuth2(ctx, config.StringEnv("google_ads_endpoint"), config.StringEnv("google_oauth2_client_id"), config.StringEnv("google_oauth2_client_secret"), google.Endpoint, config.StringEnv("google_redirect_url"), config.StringEnv("google_initial_token"), config.StringEnv("google_oauth2_scope"))

	return &googlePoller{httpClient}
}

func (gp *googlePoller) do(req *http.Request) (*http.Response, error) {
	return do(req, gp)
}

func (gp *googlePoller) getBaseURL() string {
	return config.StringEnv("google_ads_endpoint")
}

func (gp *googlePoller) getHTTPClient() *http.Client {
	return gp.httpClient
}

func (gp *googlePoller) accountID() int {
	return config.IntEnv("google_ads_account_id")
}

func (gp *googlePoller) GetCPMHandler() http.HandlerFunc {
	return func(w http.ResponseWriter, r *http.Request) {

		// decoder := json.NewDecoder(r.Body)
		jsonStr := []byte("{ operations: [ { create: { name: \"new budget 234\", amount_micros: \"60000000\"} } ] } ")

		req, err := http.NewRequest(http.MethodPost, fmt.Sprintf("/v1/customers/%d/campaignBudgets:mutate", gp.accountID()), bytes.NewBuffer(jsonStr))

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
}
