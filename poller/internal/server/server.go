package server

import (
	// "github.com/Zanhos/MaxiMiz/poller/internal/database"
	// "github.com/Zanhos/MaxiMiz/poller/internal/logger"
	"net/http"

	"github.com/Zanhos/MaxiMiz/poller/internal/poller"
)

// StartServer starts the server
func StartServer() {
	// database.ConnectToDatabase()
	// registerHandlers()
	poller.NewGooglePoller().GetCampaignBudget()
	// print("test")
}

func registerHandlers() {
	handlers := make(map[string]http.HandlerFunc)
	for pattern, handlerFunc := range handlers {
		http.HandleFunc(pattern, handlerFunc)
	}
}

// func enableLoggingForHandler(h http.Handler) http.HandlerFunc {
// 	logger.Info()

// }
