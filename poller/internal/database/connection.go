package database

import (
	"database/sql"
	"fmt"

	"github.com/google/logger"

	"github.com/Zanhos/MaxiMiz/poller/internal/config"

	// Sql driver has blank import
	_ "github.com/lib/pq"
)

// OpenDatabase uses variables in the environment to open a connection to the database and returns that connection.
func OpenDatabase() (db *sql.DB) {
	connStr := fmt.Sprintf("user=%s dbname=%s sslmode=%s", config.StringEnv("database_username"), config.StringEnv("database_name"), config.StringEnv("database_ssl_mode"))
	db, err := sql.Open(config.StringEnv("database_type"), connStr)
	if err != nil {
		logger.Fatalf("could not open database %v", err)
	}
	return db
}
