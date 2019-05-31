package server

import (
	"github.com/Zanhos/MaxiMiz/poller/internal/database"
	"github.com/Zanhos/MaxiMiz/poller/internal/logger"
	"net/http"
)

// StartServer starts the server
func StartServer() {
	database.ConnectToDatabase()
	print("test")
}

func registerHandlers() {
	handlers := make(map[string]http.HandlerFunc)
	for pattern, handlerFunc := range handlers {
		logger.Info("")
		http.HandleFunc(pattern, handlerFunc)
	}
}

// func enableLoggingForHandler(h http.Handler) http.HandlerFunc {
// 	logger.Info()

// }
