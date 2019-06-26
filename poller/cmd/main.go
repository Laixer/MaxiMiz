package main

import (
	"flag"
	"fmt"
	"os"
	"context"

	"github.com/Zanhos/MaxiMiz/poller/internal/executor"
)

const prog_ver = "0.2"

func main() {
	var config = flag.String("config", "config.toml", "Provide the configuration file")
	var version = flag.Bool("version", false, "Show version")
	flag.Parse()

	// Show version and exit
	if *version != false {
		fmt.Println("Version:", prog_ver)
		os.Exit(0)
	}

	ctx := context.Background()
	executor.ExecuteTasks(ctx, config)
}
