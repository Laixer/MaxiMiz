package main

import (
	"context"

	"github.com/Zanhos/MaxiMiz/poller/internal/server"
)

func main() {
	ctx := context.Background()
	server.StartServer(ctx)
}
