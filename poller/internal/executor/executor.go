package executor

import (
	"os"

	"github.com/Zanhos/MaxiMiz/poller/internal/database"
	"github.com/Zanhos/MaxiMiz/poller/internal/poller"
	"github.com/google/logger"

	"context"
)

// ExecuteTasks starts the poller to get the corresponding data and executes the pending tasks
func ExecuteTasks(ctx context.Context) {
	lf, err := os.OpenFile("/dev/null", os.O_CREATE|os.O_WRONLY|os.O_APPEND, 0660)
	if err != nil {
		logger.Fatalf("Failed to open log file: %v", err)
	}
	defer lf.Close()

	pollerLogger := logger.Init("poller", true, false, lf)
	defer pollerLogger.Close()
	db := database.Open()
	defer db.Close()

	database.GetPendingTasks(db)

	// pollers := poller.GetPollers(ctx)

	pollert := poller.NewGooglePoller(ctx)
	pollert.GetAllItemData()
}
