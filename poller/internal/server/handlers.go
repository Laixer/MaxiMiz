package server

import (
	"bytes"
	"fmt"
	"io/ioutil"
	"net/http"
	"os"

	"github.com/Zanhos/MaxiMiz/poller/internal/poller"
)

type pollerHandler struct {
	pattern string
	Poller  *poller.Poller
}

func (ph pollerHandler) ServeHTTP(w http.ResponseWriter, r *http.Request) {

	// decoder := json.NewDecoder(r.Body)
	jsonStr := []byte("{ operations: [ { create: { name: \"new budget 234\", amount_micros: \"60000000\"} } ] } ")

	req, err := http.NewRequest(http.MethodPost, fmt.Sprintf("/v1/customers/%s/campaignBudgets:mutate", os.Getenv("googleTestCustomerAccount")), bytes.NewBuffer(jsonStr))

	if err != nil {
		panic(err)
	}

	req.Header.Set("Content-Type", "application/json")
	req.Header.Set("developer-token", os.Getenv("developerToken"))

	resp, err := ph.Poller.Do(nil)

	if err != nil {
		panic(err)
	}

	content, err := ioutil.ReadAll(resp.Body)

	if err != nil {
		panic(err)
	}

	println(string(content))

}
