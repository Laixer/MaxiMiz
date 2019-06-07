package server

import (
	"context"
	"os"

	"golang.org/x/oauth2/google"

	"net/http"

	"github.com/Zanhos/MaxiMiz/poller/internal/logger"
	"github.com/Zanhos/MaxiMiz/poller/internal/poller"
)

func saveTokens() {
	
}

func readTokensFromFile() []string {
	return make([]string, 1)
}

func StartServer(ctx context.Context) {
	auth := readTokensFromFile()
	googlePoller := poller.New(ctx, os.Getenv("googleAdsBaseURL"), os.Getenv("googleClientID"), os.Getenv("googleClientSecret"), google.Endpoint, os.Getenv("googleRedirectURL"), auth[0], os.Getenv("adwordsScope"))

	handlers := []pollerHandler{pollerHandler{"/", &googlePoller}}

	// handle(nil, nil)
	registerHandlers(handlers)
	defer saveTokens()
}

func registerHandlers(handlers []pollerHandler) {
	for _, handler := range handlers {
		http.HandleFunc("handlerFunc.pattern", enableLogging(handler))
	}
}

func enableLogging(handler http.Handler) http.HandlerFunc {
	return http.HandlerFunc(func(w http.ResponseWriter, r *http.Request) {
		logger.Info("")
		handler.ServeHTTP(w, r)
	})
}
