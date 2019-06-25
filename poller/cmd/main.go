package main

import (
	"context"

	"github.com/Zanhos/MaxiMiz/poller/internal/executor"
)

func main() {
	ctx := context.Background()
	executor.ExecuteTasks(ctx)
}
