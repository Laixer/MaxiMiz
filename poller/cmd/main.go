package main

import (
	"flag"
	"fmt"
	"os"
	"context"

	"github.com/Zanhos/MaxiMiz/poller/internal/executor"
)

const (
	prog_ver = "0.2"
)

func main() {
	config := flag.String("config", "", "Provide the configuration file")
	version := flag.Bool("version", false, "Show version")
	flag.Parse()

	// Show version and exit
	if *version {
		fmt.Println("Version:", prog_ver)
		os.Exit(0)
	}

	ctx := context.Background()
	executor.ExecuteTasks(ctx, config)
}
