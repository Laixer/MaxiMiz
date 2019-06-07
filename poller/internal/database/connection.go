package database

import (
	"database/sql"
	"fmt"
	"log"
	"os"

	// Sql driver has blank import
	_ "github.com/lib/pq"
)

func OpenDatabase() (db *sql.DB) {
	connStr := fmt.Sprintf("user=%s dbname=%s sslmode=%s", os.Getenv("databaseUsername"), os.Getenv("databaseName"), os.Getenv("sslMode"))
	db, err := sql.Open(os.Getenv("databaseType"), connStr)
	if err != nil {
		log.Fatal(err)
	}
	return db
}
