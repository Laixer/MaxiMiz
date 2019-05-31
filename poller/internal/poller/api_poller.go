package poller

import (
	"fmt"
	"io/ioutil"
	"net/http"
	"net/url"

	"golang.org/x/oauth2"
	"golang.org/x/oauth2/google"
)

const googleAdsBaseURL = ""

type poller interface {
	getCPM() int
}

// GooglePoller ...
type GooglePoller struct {
	baseURL          *url.URL
	clientCustomerID int64 
	oAuth2Config     oauth2.Config
}

// NewGooglePoller ...
func NewGooglePoller() GooglePoller {
	url, _ := url.Parse(googleAdsBaseURL)
	oAuth2Config := oauth2.Config{
		ClientID:     "",
		ClientSecret: "",
		Endpoint:     google.Endpoint,
	}
	return GooglePoller{url, 0, oAuth2Config}
}

type TaboolaPoller struct {
}

type OutbrainPoller struct {
}

type RevcontentPoller struct {
}

func (gp GooglePoller) GetCampaignBudget() (budget int) {
	url := fmt.Sprintf("%s/customers/%d/campaignBudgets/%d", gp.baseURL, gp.clientCustomerID, 1)
	println(url)
	resp, _ := http.Get(url)
	respBody, _ := ioutil.ReadAll(resp.Body)
	println(string(respBody))
	return
}
