package database

import (
	"database/sql"
	"fmt"
	"log"

	// Sql driver has blank import
	_ "github.com/lib/pq"
)

const databaseType = ""
const databaseUsername = ""
const databaseName = ""
const sslMode = ""

// ConnectToDatabase connects to the database
func ConnectToDatabase() (db *sql.DB) {
	connStr := fmt.Sprintf("user=%s dbname=%s sslmode=%s", databaseUsername, databaseName, sslMode)
	db, err := sql.Open(databaseType, connStr)
	if err != nil {
		log.Fatal(err)
	}
	return db
}


