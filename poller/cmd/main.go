package main

import (
	"bytes"
	"context"
	"fmt"
	"io/ioutil"
	"net/http"

	"github.com/Zanhos/MaxiMiz/poller/internal/poller"
	"golang.org/x/oauth2/google"
	// "net/url"
	// "github.com/Zanhos/MaxiMiz/poller/internal/server"
)

const googleAdsBaseURL = ""
const googleTestCustomerAccount = 0
const googleClientID = ""
const googleClientSecret = ""
const auth = ""

const googleRedirectURL = ""
const adwordsScope = ""
const developerToken = ""

func main() {
	ctx := context.Background()

	jsonStr := []byte("{ operations: [ { create: { name: \"new budget 234\", amount_micros: \"60000000\"} } ] } ")

	client := poller.NewPoller(ctx, googleClientID, googleClientSecret, google.Endpoint, googleRedirectURL, auth)

	req, err := http.NewRequest(http.MethodPost, fmt.Sprintf("https://googleads.googleapis.com/v1/customers/%d/campaignBudgets:mutate", googleTestCustomerAccount), bytes.NewBuffer(jsonStr))

	if err != nil {
		panic(err)
	}

	req.Header.Set("Content-Type", "application/json")
	req.Header.Set("developer-token", developerToken)

	resp, err := client.Do(req)

	if err != nil {
		panic(err)
	}

	content, err := ioutil.ReadAll(resp.Body)

	if err != nil {
		panic(err)
	}

	println(string(content))
	// server.StartServer()
}
