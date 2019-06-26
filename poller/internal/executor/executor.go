package executor

import (
	"io/ioutil"
	"os"

	"github.com/Zanhos/MaxiMiz/poller/internal/config"
	"github.com/Zanhos/MaxiMiz/poller/internal/database"
	"github.com/Zanhos/MaxiMiz/poller/internal/poller"
	"github.com/google/logger"

	"context"
)

// Start the poller to get the corresponding data and executes the pending tasks
func ExecuteTasks(ctx context.Context, configFile *string) {
	tmpfile, err := ioutil.TempFile("", "poller")
	if err != nil {
		logger.Fatalf("Failed to open log file: %v", err)
	}
	defer os.Remove(tmpfile.Name())

	// Initialize the logger
	pollerLogger := logger.Init("poller", true, false, tmpfile)
	logger.Infof("Logging to %s", tmpfile.Name())
	defer pollerLogger.Close()

	// Configure configuration file
	if *configFile != "" {
		logger.Infof("Read configuration from %s", *configFile)
		config.SetConfigFile(configFile)
	}

	// Open database connection
	db := database.Open()
	defer db.Close()

	database.GetPendingTasks(db)

	// pollers := poller.GetPollers(ctx)

	pollert := poller.NewGooglePoller(ctx)
	pollert.GetAllItemData()
}
