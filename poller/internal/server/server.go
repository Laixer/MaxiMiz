package server

import (
	"context"
	"fmt"
	"os"
	"time"

	"github.com/google/logger"
	"github.com/gorilla/mux"

	"net/http"

	"github.com/Zanhos/MaxiMiz/poller/internal/config"
	"github.com/Zanhos/MaxiMiz/poller/internal/poller"
)

// StartServer sets up a logger, registers routes
func StartServer(ctx context.Context) {
	// TODO give the logpath env getter a different function since it throws an error since the logger init relies on the file which is not opened yet.
	logPath := config.StringEnv("server_log_location")
	f, err := os.OpenFile(logPath, os.O_CREATE|os.O_WRONLY|os.O_APPEND, 0660)
	if err != nil {
		logger.Fatalf("Failed to open log file: %v", err)
	}
	defer f.Close()

	l := logger.Init("poller", true, true, f)
	defer l.Close()

	router := mux.NewRouter()
	srv := &http.Server{
		Addr:         fmt.Sprintf("%s:%d", config.StringEnv("server_address"), config.IntEnv("server_port")),
		WriteTimeout: time.Duration(config.IntEnv("server_write_timeout_in_milliseconds")) * time.Second,
		ReadTimeout:  time.Duration(config.IntEnv("server_read_timeout_in_milliseconds")) * time.Second,
		IdleTimeout:  time.Duration(config.IntEnv("server_idle_timeout_in_milliseconds")) * time.Second,
		Handler:      router,
	}
	router.Use(enableLogging)

	pollers := poller.GetPollers(ctx)

	// handle(nil, nil)
	registerHandlers(router, pollers)
	logger.Fatal(srv.ListenAndServe())

	defer srv.Shutdown(ctx)
	// defer saveTokens()
}

func registerHandlers(router *mux.Router, pollers []poller.Poller) {
	for _, poller := range pollers {
		router.HandleFunc("/", poller.GetCPMHandler())
	}
}

func enableLogging(handler http.Handler) http.Handler {
	return http.HandlerFunc(func(w http.ResponseWriter, r *http.Request) {
		logger.Info(r.URL.String())
		handler.ServeHTTP(w, r)
	})
}
